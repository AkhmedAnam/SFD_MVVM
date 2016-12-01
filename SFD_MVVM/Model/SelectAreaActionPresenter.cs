using System;
using System.Windows.Media.Imaging;
using System.Windows;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    /// <summary>
    ///     Представитель действия - выделения квадрата на видеопотоке.
    ///     ЗАМЕЧАНИЕ: этот класс не берёт на себя ответсвенность за
    ///     обработку данных: нормировка, подсчёта концентрации и т.д.
    ///     Этим занимается класс SelectionDataPresenter, который имеет 
    ///     ссылку на этот класс
    /// </summary>
    public class SelectAreaActionPresenter : ActionPresenter,
                                             IActionPresenterChangingResultImage,
                                             IActionPresenterNeedingCameraProperty<float>
    {
        public SelectAreaActionPresenter(string id, Point cursorPosition, double videoStreamAreaHeight, double videoStreamAreaWidth)
            : base(id)
        {
            m_cursorPosition = cursorPosition; m_videoStreamAreaHeight = videoStreamAreaHeight;
            m_videoStreamAreaWidth = videoStreamAreaWidth;
            m_cursorPositionLocker = new object(); m_videoStreamAreaWidthLocker = new object(); m_videoStreamAreaHeightLocker = new object();
            m_shutterLocker = new object();
            m_pixelOffset = 10;
            m_action = InitializeNewResultImage;

        }

        //Событие, которое позволяет классу SelectionDataPresenter подписываться на обновление
        //текущего массива байтов и делать с ними что угодно: считать их фоном, КО и т.д.
        public event EventHandler CurrentBytesUpdated;

        //public event EventHandler CurrentAreaPositionChanged;

        //public Point CursorPosition
        //{
        //    set
        //    {
        //        m_cursorPosition = value;
        //        OnCurrentAreaPositionChanged();
        //    }
        //}

        //public bool IsNeedToSaveCurrentBytes
        //{
        //    set { m_isNeedToSaveCurrentBytes = value; }
        //}

        public byte[] CurrentBytes
        {
            get { return (byte[])m_currentBytes.Clone(); }
        }

        public float CurrentShutter
        {
            get { lock (m_shutterLocker) return m_currentShutter; }
        }

        public PixelFormat PixFormat
        {
            get { return m_pixelsFormat; }
        }

        public double VideoStreamAreaActualHeight
        {
            get { lock (m_videoStreamAreaHeightLocker) return m_videoStreamAreaHeight; }

            set { lock (m_videoStreamAreaHeightLocker) m_videoStreamAreaHeight = value; }
        }

        public double VideoStreamAreaActualWidth
        {
            get { lock (m_videoStreamAreaWidthLocker) return m_videoStreamAreaWidth; }

            set { lock (m_videoStreamAreaWidthLocker) m_videoStreamAreaWidth = value; }
        }

        public Point CurrentCursorPosition
        {
            get
            {
                lock (m_cursorPositionLocker) return m_cursorPosition;
            }
            set
            {
                lock (m_cursorPositionLocker) m_cursorPosition = value;
            }
        }

        #region IActionPresenterChangingResultImage Members

        public event EventHandler NewResultImageIsReady;

        public System.Windows.Media.ImageSource NewResultImage
        {
            get { return m_newResultImage; }

            set
            {
                m_newResultImage = value;
                OnNewResultImageIsReady();
            }
        }

        #endregion

        #region IActionPresenterNeedingCameraProperty<float> Members

        public event EventHandler SetNeededCameraProperty;

        public PropertyType NeededPropertyType { get; set; }

        public float CurrentNeededPropertyValue
        {
            set { lock (m_shutterLocker) m_currentShutter = value; }
        }

        #endregion

        public override object Clone()
        {
            return new SelectAreaActionPresenter(PossibleActionPresenters.AreaSelection.ToString(), new Point(0, 0), m_videoStreamAreaHeight, m_videoStreamAreaWidth);
        }

        private unsafe void InitializeNewResultImage(ManagedImage mImg)
        {
            //byte* managedImageDataPtr = mImg.data; IntPtr dataPtr = new IntPtr(managedImageDataPtr);
            m_pixelsFormat = mImg.pixelFormat;
            ManagedImage manImg = new ManagedImage();

            if (m_pixelsFormat == PixelFormat.PixelFormatMono12 || m_pixelsFormat == PixelFormat.PixelFormatMono16 || m_pixelsFormat == PixelFormat.PixelFormatMono8 || m_pixelsFormat == PixelFormat.PixelFormatSignedMono16)
                mImg.Convert(PixelFormat.PixelFormatMono8, manImg);
            else
                mImg.Convert(PixelFormat.PixelFormatBgr, manImg);

            
            int height = (int)manImg.rows, width = (int)manImg.cols, stride = (int)manImg.stride, dataSize = height * stride, bpp = (int)manImg.bitsPerPixel / 8;
            byte* imageData = manImg.data;
            //byte[] imgData = new byte[dataSize];

            //Marshal.Copy(dataPtr, imgData, 0, dataSize);
            Point cursorPosition = this.CurrentCursorPosition;
            int xRec = (int)(cursorPosition.X / this.VideoStreamAreaActualWidth * width), yRec = (int)(cursorPosition.Y / this.VideoStreamAreaActualHeight * height);
            Int32Rect rec = GetValidRectangle(xRec, yRec, width, height);
            m_currentRec = rec;

            byte[] currBytes = new byte[rec.Height * rec.Width * bpp];

            ManagedImage managImgForWrbmp = new ManagedImage();
            manImg.ConvertToWriteAbleBitmap(managImgForWrbmp);
            WriteableBitmap bmpTemp = managImgForWrbmp.writeableBitmap;
            bmpTemp.CopyPixels(rec, currBytes, rec.Width * bpp, 0);
            m_currentBytes = currBytes;
            OnSetNeededCameraProperty();
            OnCurrentBytesUpdated();

            int xBegin = rec.X, xEnd = rec.X + rec.Width, yBegin = rec.Y, yEnd = rec.Y + rec.Height;

            for (int xPix = xBegin; xPix <= xEnd; xPix++)
            {
                for (int yPix = yBegin; yPix <= yEnd; yPix++)
                {
                    if (yPix == yBegin || yPix == yEnd || xPix == xBegin || xPix == xEnd)
                    {
                        int pixelPosition = yPix * stride + xPix * bpp;

                        if (manImg.pixelFormat == PixelFormat.PixelFormatBgr)
                        {
                            imageData[pixelPosition] = 0;
                            imageData[pixelPosition + 1] = 0;
                            imageData[pixelPosition + 2] = 0;
                        }
                        else
                            imageData[pixelPosition] = 0;
                    }
                }
            }

            ManagedImage temp = new ManagedImage();
            manImg.ConvertToBitmapSource(temp);
            temp.bitmapsource.Freeze();
            this.NewResultImage = temp.bitmapsource;
            OnNewResultImageIsReady();
        }

        private void OnNewResultImageIsReady()
        {
            if (NewResultImageIsReady != null)
                NewResultImageIsReady(this, EventArgs.Empty);
        }
        private void OnSetNeededCameraProperty()
        {
            if (SetNeededCameraProperty != null)
                SetNeededCameraProperty(this, EventArgs.Empty);
        }
        private void OnCurrentBytesUpdated()
        {
            if (CurrentBytesUpdated != null)
                CurrentBytesUpdated(this, EventArgs.Empty);
        }
        //private void OnCurrentAreaPositionChanged()
        //{
        //    if (CurrentAreaPositionChanged != null)
        //        CurrentAreaPositionChanged(this, EventArgs.Empty);
        //}

        private Int32Rect GetValidRectangle(int pixelX, int pixelY, int imagePixelWidth, int imagePixelHeight)
        {
            int recX, recY, recLen = 2 * m_pixelOffset;

            if ((pixelX - m_pixelOffset) < 0)
                recX = 0;
            else if ((pixelX + m_pixelOffset) > imagePixelWidth)
                recX = imagePixelWidth - 2 * m_pixelOffset;
            else
                recX = pixelX - m_pixelOffset;

            if ((pixelY - m_pixelOffset) < 0)
                recY = 0;
            else if ((pixelY + m_pixelOffset) > imagePixelHeight)
                recY = imagePixelHeight - 2 * m_pixelOffset;
            else
                recY = pixelY - m_pixelOffset;

            return new Int32Rect(recX, recY, recLen, recLen);
        }
        //private unsafe void RestoreLastAreaBytes(byte* data, int stride, int bpp)
        //{
        //    int xBegin = m_lastRec.X, xEnd = m_lastRec.X + m_lastRec.Width, yBegin = m_lastRec.Y, yEnd = m_lastRec.Y + m_lastRec.Height;

        //    for (int xPix = xBegin; xPix <= xEnd; xPix++)
        //    {
        //        for (int yPix = yBegin; yPix <= yEnd; yPix++)
        //        {
        //            if (yPix == yBegin || yPix == yEnd || xPix == xBegin || xPix == xEnd)
        //            {
        //                int pixelPosition = (yPix - 1) * stride + (xPix - 1) * bpp;
        //                int positionInArray = (yPix - yBegin) * 2 * m_pixelOffset + (xPix - xBegin) * bpp;
        //                //data[pixelPosition] = 0;
        //                //data[pixelPosition + 1] = 0;
        //                //data[pixelPosition + 2] = 0;
        //                data[pixelPosition] = m_lastAreaBytes[positionInArray];
        //                data[pixelPosition + 1] = m_lastAreaBytes[positionInArray + 1];
        //                data[pixelPosition + 2] = m_lastAreaBytes[positionInArray + 2];
        //            }
        //        }
        //    }
        //}
        //private unsafe void SetAreaBytesToArray(Int32Rect rec, byte[] destinationArr, byte* data, int stride, int bpp)
        //{
        //    int xBegin = rec.X, xEnd = rec.X + rec.Width, yBegin = rec.Y, yEnd = rec.Y + rec.Height;

        //    for (int xPix = xBegin; xPix <= xEnd; xPix++)
        //    {
        //        for (int yPix = yBegin; yPix <= yEnd; yPix++)
        //        {
        //            if (yPix == yBegin || yPix == yEnd || xPix == xBegin || xPix == xEnd)
        //            {
        //                int pixelPosition = (yPix - 1) * stride + (xPix - 1) * bpp;
        //                int positionInArray = (yPix - yBegin) * 2 * m_pixelOffset + (xPix - xBegin) * bpp;
        //                //data[pixelPosition] = 0;
        //                //data[pixelPosition + 1] = 0;
        //                //data[pixelPosition + 2] = 0;
        //                destinationArr[positionInArray] = data[pixelPosition];
        //                destinationArr[positionInArray + 1] = data[pixelPosition + 1];
        //                destinationArr[positionInArray + 2] = data[pixelPosition + 2];
        //            }
        //        }
        //    }
        //}
        //private void CalcCorrectionRatio()
        //{

        //}

        private object m_cursorPositionLocker, m_videoStreamAreaWidthLocker, m_videoStreamAreaHeightLocker, m_shutterLocker;
        private Point m_cursorPosition;
        private Int32Rect m_currentRec;
        private System.Windows.Media.ImageSource m_newResultImage;
        private byte[] m_currentBytes;
        private float m_currentShutter;
        private int m_pixelOffset;
        private double m_videoStreamAreaHeight, m_videoStreamAreaWidth;
        private PixelFormat m_pixelsFormat;
        //private bool m_isNeedToSaveCurrentBytes;
    }

}
