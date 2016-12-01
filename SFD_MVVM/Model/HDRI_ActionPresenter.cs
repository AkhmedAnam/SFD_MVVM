using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    /// <summary>
    /// Представляет HDRI в процессоре
    /// </summary>
    public class HDRI_ActionPresenter : ActionPresenter,
                                        IActionPresenterHavingDataStorage<ManagedImage>, 
                                        IActionPresenterHavingFinalState,
                                        IActionPresenterNeededChangingCameraProperty
                                        
    {
        public HDRI_ActionPresenter(string id, int framesNumber, int seriesNumber, double shutterRangeBegin, double shutterRangeEnd, string directoryToSave)
            : base(id)
        {
            m_framesInSeriaNumber = framesNumber; m_seriesNumber = seriesNumber;
            m_directoryToSaveFile = directoryToSave;
            m_seriesShutterValues = new List<float>(m_seriesNumber);
            this.Data = new List<ManagedImage>();
            m_imgsInSeriaCounter = 0; m_seriaCounter = 0;
            m_shutterRangeBegin = shutterRangeBegin; m_shutterRangeEnd = shutterRangeEnd;
            //m_seriesShutterValues.Add((float)m_shutterRangeBegin);
            m_isCalculationContinue = false;
            //m_shutterArgs = new HDRIShutterArgs(shutterRangeBegin, shutterRangeEnd);

            m_action = Act;
        }

        public List<ManagedImage> Data { get; set; }

        public void DoFinalAction()
        {
            bool isDataValid = CheckPixelFormatOfAllImagesInData() && CheckSizeOfAllImagesInData();

            if (isDataValid)
            {
                m_isCalculationContinue = true;

                Task.Run(() =>
                {
                    Filter();
                    UnionFilteredImages();

                    string fileNameToSave = m_directoryToSaveFile;

                    fileNameToSave += "HDRImage_" + DateTime.Now.Year.ToString() + "_" +
                        DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                        DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + "_" +
                        DateTime.Now.Second.ToString() + ".jpeg";

                    this.Final_HDR_Image.Save(fileNameToSave, ImageFileFormat.Jpeg);
                    OnFinalStateIsReached();
                    MessageBox.Show("HDR Image has been saved in:" + Environment.NewLine + Environment.NewLine + fileNameToSave, "Task is done", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.IsNeedImage = false;
                });
            }

        }

        public override object Clone()
        {
            return new HDRI_ActionPresenter(PossibleActionPresenters.HDRI.ToString(), m_framesInSeriaNumber, m_seriesNumber, m_shutterRangeBegin, m_shutterRangeEnd, m_directoryToSaveFile);
        }

        public event EventHandler FinalStateIsReached;

        public ManagedImage Final_HDR_Image { get; set; }

        #region IActionPresenterNeededChangingCameraProperty Members

        public event EventHandler<ChangeCameraPropertyArgs> ChangeCameraProperty;

        #endregion


        #region Implementing HDRI

        private void Filter()
        {
            foreach (ManagedImage img in this.Data)
                FilterSingleImage(img);
        }

        private unsafe void FilterSingleImage(ManagedImage image)//пока только для изображений 24bpp 
        {
            Int32 height = (int)image.rows, width = (int)image.cols, stride = (int)image.stride;

            byte* dataPointer = image.data;

            for (int h = 0, posInPix = 0; h < height; h++, posInPix += stride - 3 * width)
                for (int w = 0; w < width; w++, posInPix += 3)
                {
                    bool isPixelWhite = ((dataPointer[posInPix] >= m_topBorderForColorIntesity) && (dataPointer[posInPix + 1] >= m_topBorderForColorIntesity) && (dataPointer[posInPix + 2] >= m_topBorderForColorIntesity));
                    bool isPixelBlack = ((dataPointer[posInPix] <= m_bottomBorderForColorIntesity) && (dataPointer[posInPix + 1] <= m_bottomBorderForColorIntesity) && (dataPointer[posInPix + 2] <= m_bottomBorderForColorIntesity));

                    bool isPixelInvalid = isPixelWhite && isPixelBlack;

                    if (isPixelInvalid)//красим засвеченный/затемнённый (неккоректный) пиксель в чёрный
                    {
                        dataPointer[posInPix] = (byte)0;
                        dataPointer[posInPix + 1] = (byte)0;
                        dataPointer[posInPix + 2] = (byte)0;
                    }
                }
        }

        private unsafe void UnionFilteredImages()
        {
            byte*[] imagesDataPointers = new byte*[this.Data.Count];

            for (int i = 0; i < this.Data.Count; i++)
                imagesDataPointers[i] = this.Data[i].data;

            int height = (int)this.Data[0].rows, width = (int)this.Data[0].cols, stride = (int)this.Data[0].stride;
            int[] validPixelsCounters = new int[height * width];
            double[] resultImageDataStorage = new double[height * stride];

            for (int h = 0, posInPix = 0; h < height; h++, posInPix += stride - 3 * width)
                for (int w = 0; w < width; w++, posInPix += 3)
                {
                    for (int i = 0; i < this.Data.Count; i++)
                    {
                        int imageNumber = i + 1;
                        int currentSeria = GetSeriaNumberFromImageNumber(imageNumber);

                        try
                        {
                            resultImageDataStorage[posInPix] += ((double)imagesDataPointers[i][posInPix] / m_seriesShutterValues[currentSeria - 1]);
                            resultImageDataStorage[posInPix + 1] += ((double)imagesDataPointers[i][posInPix + 1] / m_seriesShutterValues[currentSeria - 1]);
                            resultImageDataStorage[posInPix + 2] += ((double)imagesDataPointers[i][posInPix + 2] / m_seriesShutterValues[currentSeria - 1]);
                        }
                        catch (DivideByZeroException)
                        {
                            this.Final_HDR_Image = null;
                            return;
                        }

                        if ((imagesDataPointers[i][posInPix] != 0) || (imagesDataPointers[i][posInPix + 1] != 0) || (imagesDataPointers[i][posInPix + 2] != 0))
                            validPixelsCounters[h * width + w]++;

                        //tempInt[posInPix] = (bmpPointers[i][posInPix] == 0) ? tempInt[posInPix] : (tempInt[posInPix] + 1);
                        //tempInt[posInPix + 1] = (bmpPointers[i][posInPix + 1] == 0) ? tempInt[posInPix + 1] : (tempInt[posInPix + 1] + 1);
                        //tempInt[posInPix + 2] = (bmpPointers[i][posInPix + 2] == 0) ? tempInt[posInPix + 2] : (tempInt[posInPix + 2] + 1);
                    }
                }

            for (int h = 0, posInPix = 0; h < height; h++, posInPix += stride - 3 * width)
                for (int w = 0; w < width; w++, posInPix += 3)
                {
                    int divider = validPixelsCounters[h * width + w];

                    if (divider != 0)
                    {
                        resultImageDataStorage[posInPix] /= divider;
                        resultImageDataStorage[posInPix + 1] /= divider;
                        resultImageDataStorage[posInPix + 2] /= divider;
                    }
                }

            for (int i = 0; i < resultImageDataStorage.Length; i++)
            {
                if (resultImageDataStorage[i] != 0)
                    resultImageDataStorage[i] = Math.Log10(resultImageDataStorage[i]);
            }

            byte[] finalData = new byte[resultImageDataStorage.Length];

            double max = resultImageDataStorage.Max(), min = resultImageDataStorage.Min(), range = max - min;
            for (int i = 0; i < resultImageDataStorage.Length; i++)
            {
                finalData[i] = (byte)(((resultImageDataStorage[i] - min) / range) * Byte.MaxValue);
            }
            
            fixed (byte* finalDataPointer = &finalData[0])
            {
                this.Final_HDR_Image = new ManagedImage((uint)height, (uint)width, (uint)stride, finalDataPointer, (uint)finalData.Length, PixelFormat.PixelFormatBgr);
            }
        }


        #endregion

        private void Act(ManagedImage managedImage)
        {
            if (m_seriaCounter < m_seriesNumber)
            {
                ManagedImage localManagedImage = new ManagedImage();
                managedImage.Convert(PixelFormat.PixelFormatBgr, localManagedImage);

                try
                {
                    this.Data.Add(localManagedImage);
                }
                catch (OutOfMemoryException)//If there is no memory to store images, final action has to be done. If at this moment current series is not fully filled, images in this current series must be removed
                {
                    if ((m_imgsInSeriaCounter % m_framesInSeriaNumber) != 0)
                    {
                        for (int i = 1; i <= m_imgsInSeriaCounter; i++)
                        {
                            int index = m_seriesNumber * m_seriaCounter + i;
                            this.Data.RemoveAt(index);
                        }
                    }

                    m_imgsInSeriaCounter = -1; // Next logical condition is gonna be false because m_framesInSeriaNumber can't be less than 0. And last logical condition is gonna be true if final action has not been invoked yet (m_isCalculationContinue = false)
                    m_seriaCounter = m_seriesNumber; //that provide final action execution 
                }

                m_imgsInSeriaCounter++;

                if (m_imgsInSeriaCounter == m_framesInSeriaNumber)
                {
                    m_imgsInSeriaCounter = 0;
                    m_seriaCounter++;
                    float shutterValToSet = (float)(m_shutterRangeBegin + ((m_shutterRangeEnd - m_shutterRangeBegin) / m_seriesNumber) * m_seriaCounter);
                    m_seriesShutterValues.Add(shutterValToSet);
                    OnChangeCameraProperty(shutterValToSet);
                }
            }
            else if(m_seriaCounter == m_seriesNumber && !m_isCalculationContinue)
                DoFinalAction();
        }


        private int GetSeriaNumberFromImageNumber(int imageNumber)
        {
            return ((imageNumber % m_framesInSeriaNumber)) == 0 ? (imageNumber / m_framesInSeriaNumber) : (imageNumber / m_framesInSeriaNumber + 1);
        }
        private bool CheckSizeOfAllImagesInData()
        {
            uint height = this.Data[0].rows, width = this.Data[0].cols;

            IEnumerable<ManagedImage> listWithImagesHavingSameSize = from img in this.Data
                                                                     where (img.cols == width) && (img.rows == height)
                                                                     select img;

            return listWithImagesHavingSameSize.Count<ManagedImage>() == this.Data.Count;
        }
        private bool CheckPixelFormatOfAllImagesInData()
        {
            PixelFormat format = this.Data[0].pixelFormat;

            IEnumerable<ManagedImage> imagesWithSameFormat = from img in this.Data
                                                             where img.pixelFormat == format
                                                             select img;

            return imagesWithSameFormat.Count<ManagedImage>() == this.Data.Count;
        }
        private void OnChangeCameraProperty(float value)
        {
            if (ChangeCameraProperty != null)
                ChangeCameraProperty(this, new ChangeCameraPropertyArgs(PropertyType.Shutter, value));
        }
        private void OnFinalStateIsReached()
        {
            if (FinalStateIsReached != null)
                FinalStateIsReached(this, EventArgs.Empty);
        }

        private readonly int m_framesInSeriaNumber, m_seriesNumber;
        private const byte m_topBorderForColorIntesity = (byte)242, m_bottomBorderForColorIntesity = (byte)38;
        private int m_imgsInSeriaCounter, m_seriaCounter;
        private List<float> m_seriesShutterValues;
        private string m_directoryToSaveFile;
        private readonly double m_shutterRangeBegin, m_shutterRangeEnd;
        private bool m_isCalculationContinue;
        //private HDRIShutterArgs m_shutterArgs;

    }
}
