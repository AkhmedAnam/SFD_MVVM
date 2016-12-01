using System;
using System.Collections.Generic;
using System.Linq;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    /// <summary>
    /// Класс, отвечающий за обработку данных в режиме фармакокинетики.
    /// </summary>
    public class SelectionDataPresenter
    {
        public SelectionDataPresenter(SelectAreaActionPresenter presenter, int imgsNumberInNormalization)
        {
            m_actionPresenter = presenter;
            m_pixelsFormat = m_actionPresenter.PixFormat;
            m_counterB = 0; m_counterCO = 0; m_counterEx = 0;
            m_numberOfImagesInNormalization = imgsNumberInNormalization;
            m_bckgroundBytes = new List<byte[]>(m_numberOfImagesInNormalization); m_coBytes = new List<byte[]>(m_numberOfImagesInNormalization);
            m_experDataBytes = new List<byte[]>(m_numberOfImagesInNormalization); m_experDataShutter = new List<float>(m_numberOfImagesInNormalization);
            m_coShutter = new List<float>(m_numberOfImagesInNormalization); m_bckgroundShutter = new List<float>(m_numberOfImagesInNormalization);
            m_correctRaio = m_etalonConcentration = m_currentConcentration = 0;

            m_areCODataFilling = false; m_areBckGroundDataFilling = false;
        }


        public double EtalonConcentration
        {
            set { m_etalonConcentration = value; }
        }

        public double CurrentConcentration
        {
            get { return m_currentConcentration; }
        }

        public event EventHandler CurrentConcentrationChanged;

        public void PrepareBackgroundData()
        {
            if (m_areBckGroundDataFilling == false)
            {
                if (m_bckgroundBytes.Count != 0 || m_coBytes.Count != 0)
                {
                    RemoveExperCalculation();
                    m_bckgroundBytes.Clear();
                    m_bckgroundShutter.Clear();
                    m_coBytes.Clear();
                    m_coShutter.Clear();
                    m_backgroundMinus = null;
                    m_correctRaio = 0; m_currentConcentration = 0;
                }

                m_areBckGroundDataFilling = true;
                m_actionPresenter.CurrentBytesUpdated += GetBckGroundDataFromPresenter;
            }
        }
        public void PrepareCOData()
        {
            if (m_areCODataFilling == false)
            {
                if (m_coBytes.Count != 0)
                {
                    RemoveExperCalculation();
                    m_coBytes.Clear();
                    m_coShutter.Clear();
                }

                m_areCODataFilling = true;
                m_actionPresenter.CurrentBytesUpdated += GetCODataFromPresenter;
            }
        }

        public SelectAreaActionPresenter ActionPresenter
        {
            get { return m_actionPresenter; }
        }

        private void DoExperCalculation()
        {
            m_actionPresenter.CurrentBytesUpdated += GetExperimentalDataFromPresenter;
        }
        private void RemoveExperCalculation()
        {
            m_actionPresenter.CurrentBytesUpdated -= GetExperimentalDataFromPresenter;
        }
        private void GetBckGroundDataFromPresenter(object sender, EventArgs e)
        {
            SelectAreaActionPresenter presenter = sender as SelectAreaActionPresenter;

            if (m_counterB < m_numberOfImagesInNormalization)
            {
                m_bckgroundBytes.Add(presenter.CurrentBytes);
                m_bckgroundShutter.Add(presenter.CurrentShutter);
                m_counterB++;
            }
            else
            {
                presenter.CurrentBytesUpdated -= GetBckGroundDataFromPresenter;
                m_counterB = 0;
                m_areBckGroundDataFilling = false;
            }
        }
        private void GetCODataFromPresenter(object sender, EventArgs e)
        {
            SelectAreaActionPresenter presenter = sender as SelectAreaActionPresenter;

            if (m_counterCO < m_numberOfImagesInNormalization)
            {
                m_coBytes.Add(presenter.CurrentBytes);
                m_coShutter.Add(presenter.CurrentShutter);
                m_counterCO++;
            }
            else
            {
                presenter.CurrentBytesUpdated -= GetCODataFromPresenter;
                m_counterCO = 0;
                m_areCODataFilling = false;
                CalcCorrectRatio();
                DoExperCalculation();
            }
        }
        private void GetExperimentalDataFromPresenter(object sender, EventArgs e)
        {
            SelectAreaActionPresenter presenter = sender as SelectAreaActionPresenter;

            if (m_counterEx < m_numberOfImagesInNormalization)
            {
                m_experDataBytes.Add(presenter.CurrentBytes);
                m_experDataShutter.Add(presenter.CurrentShutter);
                m_counterEx++;
            }
            else
            {
                //presenter.CurrentBytesUpdated -= GetExperimentalDataFromPresenter;
                m_counterEx = 0;
                CalcCurrentConc();
                m_experDataBytes.Clear();
                m_experDataShutter.Clear();
            }
        }


        private void CalcCorrectRatio()
        {
            int bytesCount = m_bckgroundBytes[0].Length, imagesCount = m_bckgroundBytes.Count, pixelsCount;

            if (m_actionPresenter.PixFormat == PixelFormat.PixelFormatBgr)
                pixelsCount = bytesCount / 3;
            else
                pixelsCount = bytesCount;

            m_backgroundMinus = new float[pixelsCount];
            float[] coNormolized = new float[pixelsCount];

            if (m_actionPresenter.PixFormat == PixelFormat.PixelFormatBgr)
            {
                for (int pixelPos = 0, posInPixel = 0; pixelPos < pixelsCount; pixelPos++, posInPixel += 3)
                {
                    for (int j = 0; j < imagesCount; j++)
                    {
                        m_backgroundMinus[pixelPos] = (((float)m_bckgroundBytes[j][posInPixel] + (float)m_bckgroundBytes[j][posInPixel + 1] + (float)m_bckgroundBytes[j][posInPixel + 2]) / 3.0f)
                                                         / (m_bckgroundShutter[j] * (float)imagesCount);

                        coNormolized[pixelPos] = (((float)m_coBytes[j][posInPixel] + (float)m_coBytes[j][posInPixel + 1] + (float)m_coBytes[j][posInPixel + 2]) / 3.0f)
                                                         / (m_coShutter[j] * (float)imagesCount);
                    }
                }
            }
            else
            {
                for (int pixelPosition = 0; pixelPosition < pixelsCount; pixelPosition++)
                {
                    for (int j = 0; j < imagesCount; j++)
                    {
                        m_backgroundMinus[pixelPosition] = ((float)m_bckgroundBytes[j][pixelPosition] / m_bckgroundShutter[j] * (float)imagesCount);

                        coNormolized[pixelPosition] = ((float)m_coBytes[j][pixelPosition] / m_coShutter[j] * (float)imagesCount);
                    }
                }
            }

            m_correctRaio = Math.Abs(coNormolized.Sum() - m_backgroundMinus.Sum());
        }
        private void CalcCurrentConc()
        {
            int bytesCount = m_experDataBytes[0].Length, imagesCount = m_experDataBytes.Count, pixelsCount;

            if (m_actionPresenter.PixFormat == PixelFormat.PixelFormatBgr)
                pixelsCount = bytesCount / 3;
            else
                pixelsCount = bytesCount;

            float[] averageExperData = new float[pixelsCount];

            if (m_pixelsFormat == PixelFormat.PixelFormatBgr)
            {
                for (int pixelPos = 0, posInPixel = 0; pixelPos < pixelsCount; pixelPos++, posInPixel += 3)
                {
                    for (int j = 0; j < imagesCount; j++)
                    {
                        averageExperData[pixelPos] = (((float)m_experDataBytes[j][posInPixel] + (float)m_experDataBytes[j][posInPixel + 1] + (float)m_experDataBytes[j][posInPixel + 2]) / 3.0f)
                                                         / (m_experDataShutter[j] * (float)imagesCount);
                    }
                }
            }
            else
            {
                for (int pixelPosition = 0; pixelPosition < pixelsCount; pixelPosition++)
                {
                    for (int j = 0; j < imagesCount; j++)
                    {
                        averageExperData[pixelPosition] = ((float)m_experDataBytes[j][pixelPosition] / m_experDataShutter[j] * (float)imagesCount);
                    }
                }
            }

            for (int i = 0; i < pixelsCount; i++)
            {
                averageExperData[i] -= m_backgroundMinus[i];
            }

            m_currentConcentration = averageExperData.Sum() / m_correctRaio * m_etalonConcentration;
            OnCurrentConcentrationChanged();
        }

        private void OnCurrentConcentrationChanged()
        {
            if (CurrentConcentrationChanged != null)
                CurrentConcentrationChanged(this, EventArgs.Empty);
        }

        //private bool IsImageColored()
        //{
        //    switch (m_pixelsFormat)
        //    {
        //        case PixelFormat.PixelFormatMono12:
        //            return false;
        //        case PixelFormat.PixelFormatMono16:
        //            return false;
        //        case PixelFormat.PixelFormatMono8:
        //            return false;
        //        case PixelFormat.PixelFormatSignedRgb16:
        //            return false;
        //        default:
        //            return true;
        //    }
        //}

        private List<byte[]> m_bckgroundBytes, m_coBytes, m_experDataBytes;
        private List<float> m_bckgroundShutter, m_coShutter, m_experDataShutter;
        private double m_etalonConcentration, m_currentConcentration, m_correctRaio;
        private float[] m_backgroundMinus;
        private int m_counterB, m_counterCO, m_counterEx;
        private readonly SelectAreaActionPresenter m_actionPresenter;
        private readonly int m_numberOfImagesInNormalization;
        private bool m_areBckGroundDataFilling, m_areCODataFilling;
        private readonly PixelFormat m_pixelsFormat;
    }
}
