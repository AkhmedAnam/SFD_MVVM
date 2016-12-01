using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SFD_MVVM.ViewModel
{
    public class HDRI_ViewModel : ViewModelBase
    {
        public HDRI_ViewModel()
        {
            this.FrameCount = this.SeriesCount = 4;
            this.IsHDRI_Processing = false;
            this.IsHDRIPossible = this.HDRIShutterRangeBegin < this.HDRIShutterRangeEnd && this.FrameCount > 0 && this.SeriesCount > 0 && !this.IsHDRI_Processing;
            this.MakeHDRImage = new CommandDelegate(MakeHDRI);
        }

        public Int32 FrameCount
        {
            get { return m_frameCount; }

            set
            {
                m_frameCount = value;
                OnPropertyChanged(this, "FrameCount");
                this.IsHDRIPossible = this.HDRIShutterRangeBegin < this.HDRIShutterRangeEnd && this.FrameCount > 0 && this.SeriesCount > 0 && !this.IsHDRI_Processing;
            }
        }

        public Int32 SeriesCount
        {
            get { return m_seriesCount; }

            set
            {
                m_seriesCount = value;
                OnPropertyChanged(this, "SeriesCount");
                this.IsHDRIPossible = this.HDRIShutterRangeBegin < this.HDRIShutterRangeEnd && this.FrameCount > 0 && this.SeriesCount > 0 && !this.IsHDRI_Processing;
            }
        }

        public bool IsHDRI_Processing
        {
            get { return m_isHDRIProcessing; }

            set
            {
                m_isHDRIProcessing = value;
                OnPropertyChanged(this, "IsHDRI_Processing");
                this.IsHDRIPossible = this.HDRIShutterRangeBegin < this.HDRIShutterRangeEnd && this.FrameCount > 0 && this.SeriesCount > 0 && !this.IsHDRI_Processing;
            }
        }

        public bool IsHDRIPossible
        {
            get { return m_isHDRIPossible; }

            set
            {
                m_isHDRIPossible = value;
                OnPropertyChanged(this, "IsHDRIPossible");
            }
        }

        public double HDRIShutterRangeBegin
        {
            get { return m_HDRIshutterRangeBegin; }

            set
            {
                m_HDRIshutterRangeBegin = value;
                OnPropertyChanged(this, "ShutterRangeBegin");
                this.IsHDRIPossible = this.HDRIShutterRangeBegin < this.HDRIShutterRangeEnd && this.FrameCount > 0 && this.SeriesCount > 0 && !this.IsHDRI_Processing;
            }
        }

        public double HDRIShutterRangeEnd
        {
            get { return m_HDRIshutterRangeEnd; }

            set
            {
                m_HDRIshutterRangeEnd = value;
                OnPropertyChanged(this, "ShutterRangeEnd");
                this.IsHDRIPossible = this.HDRIShutterRangeBegin < this.HDRIShutterRangeEnd && this.FrameCount > 0 && this.SeriesCount > 0 && !this.IsHDRI_Processing;
            }
        }


        public CommandDelegate MakeHDRImage { get; set; }

        public string DirectoryToSaveResultImage { get; set; }


        private void MakeHDRI(object param)
        {
            this.IsHDRI_Processing = true;
            m_processor.AddHDRI_Action(this.FrameCount, this.SeriesCount, this.HDRIShutterRangeBegin, this.HDRIShutterRangeEnd, this.DirectoryToSaveResultImage);
        }

        private int m_frameCount, m_seriesCount;
        private bool m_isHDRIProcessing, m_isHDRIPossible;
        private double m_HDRIshutterRangeBegin, m_HDRIshutterRangeEnd;
    }
}
