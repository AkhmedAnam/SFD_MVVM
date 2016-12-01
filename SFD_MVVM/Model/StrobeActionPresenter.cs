using System;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    public class StrobeActionPresenter : ActionPresenter, IActionPresenterChangingResultImage
    {
        public StrobeActionPresenter(string id, string directory)
            : base(id)
        {
            m_oldShutter = m_oldGain = 0;
            this.DirectoryToSaveFiles = (directory == String.Empty || directory == null) ? Directory.GetCurrentDirectory() : directory;
            m_oldManagedIMage = new ManagedImage();
            m_processedManagedImage = new ManagedImage();
            m_action = ActionWork;
        }


        public bool IsNeedSaveAllImages { get; set; }
        public string DirectoryToSaveFiles { get; set; }

        public override object Clone()
        {
            return new StrobeActionPresenter(PossibleActionPresenters.Stroboscope.ToString(), this.DirectoryToSaveFiles);
        }

        #region IActionPresenterChangingResultImage Members

        public event EventHandler NewResultImageIsReady;

        public ImageSource NewResultImage
        {
            get { return m_newResultImage; }

            set
            {
                m_newResultImage = value as WriteableBitmap;
            }
        }

        #endregion

        private void OnNewResultImageIsReady()
        {
            if (NewResultImageIsReady != null)
                NewResultImageIsReady(this, EventArgs.Empty);
        }
        private unsafe void CalcIMage()
        {
            if (m_processedManagedImage == null)
                return;

            if (m_oldManagedIMage == null)
                return;

            int height = (int)m_processedManagedImage.rows, width = (int)m_processedManagedImage.cols, stride = (int)m_processedManagedImage.stride, dataSize = height * stride;

            if (height != m_oldManagedIMage.rows || width != m_oldManagedIMage.cols || stride != m_oldManagedIMage.stride)
                return;

            byte* processedImgPointer = m_processedManagedImage.data, oldImgPointer = m_oldManagedIMage.data;

            for (int y = 0, c = 0; y < height; y++, c += stride - width * 3)
                for (int x = 0; x < width; x++, c += 3)
                {
                    byte ch = (byte)((processedImgPointer[c] > oldImgPointer[c]) ? (processedImgPointer[c] - oldImgPointer[c]) : (oldImgPointer[c] - processedImgPointer[c]));
                    processedImgPointer[c + 1] = processedImgPointer[c + 2] = (byte)((processedImgPointer[c] > oldImgPointer[c] ? processedImgPointer[c] : oldImgPointer[c]) /*<< shift*/);
                    processedImgPointer[c] = processedImgPointer[c + 1] = (byte)(processedImgPointer[c] < oldImgPointer[c] ? oldImgPointer[c] : processedImgPointer[c])/*<<shift) - (ch << m_valueShift)*/;
                }

            byte[] dataForFinalImage = new byte[dataSize];

            IntPtr ptr = new IntPtr(processedImgPointer);
            Marshal.Copy(source: ptr, destination: dataForFinalImage, startIndex: 0, length: dataSize);



            m_newResultImage = new WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Bgr24, null);
            Int32Rect rec = new Int32Rect(0, 0, width, height);
            m_newResultImage.WritePixels(rec, dataForFinalImage, stride, 0);
            OnNewResultImageIsReady();
        }
        private void ActionWork(ManagedImage managedImage)
        {
            m_oldManagedIMage = new ManagedImage(m_processedManagedImage);
            m_processedManagedImage = new ManagedImage();

            managedImage.Convert(FlyCapture2Managed.PixelFormat.PixelFormatBgr, m_processedManagedImage);

            uint gain = managedImage.imageMetadata.embeddedGain, shutter = managedImage.imageMetadata.embeddedShutter;
            bool isNeedCorrect = (m_oldGain != gain) && (m_oldShutter != shutter);

            if (isNeedCorrect)
                CalcIMage();
            else
            {
                ManagedImage temp = new ManagedImage();
                m_processedManagedImage.ConvertToWriteAbleBitmap(temp);
                temp.writeableBitmap.Freeze();
                m_newResultImage = temp.writeableBitmap;
                OnNewResultImageIsReady();
            }

            m_oldGain = gain; m_oldShutter = shutter;

            if (this.IsNeedSaveAllImages)
                Task.Run(() => SaveImage());
        }

        private void SaveImage()
        {
            string fileFullName = this.DirectoryToSaveFiles + @"\" + "StrobeImage_" + DateTime.Now.Year.ToString() + "_" +
                    DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                    DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + "_" +
                    DateTime.Now.Second.ToString() + ".jpeg";

            int stride = m_newResultImage.BackBufferStride, height = m_newResultImage.PixelHeight;
            byte[] data = new byte[height * stride];

            m_newResultImage.CopyPixels(data, stride, 0);

            using (FileStream fs = new FileStream(fileFullName, FileMode.Create))
            {
                //fs.Write(data, 0, data.Length);
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(m_newResultImage));
                encoder.Save(fs);
            }
        }

        private WriteableBitmap m_newResultImage;
        private ManagedImage m_oldManagedIMage, m_processedManagedImage;
        private uint m_oldGain, m_oldShutter;
    }
}
