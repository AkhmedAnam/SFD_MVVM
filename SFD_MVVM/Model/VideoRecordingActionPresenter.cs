using System;
using System.Windows;
using System.Collections.Generic;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    public class VideoRecordingActionPresenter : ActionPresenter, 
                                                 IActionPresenterHavingDataStorage<ManagedImage>,
                                                 IActionPresenterHavingFinalState
    {
        public VideoRecordingActionPresenter(string id, string directory = "current")
            : base(id)
        {
            this.Data = new List<ManagedImage>();

            if (directory == "current" || directory == null || directory == String.Empty)
                this.DirectoryToSaveVideo = System.IO.Directory.GetCurrentDirectory();
            else
                this.DirectoryToSaveVideo = directory;

            this.IsVideoRecording = true;
            //m_locker = new object();
            m_action = Action;
        }

        public string DirectoryToSaveVideo { private get; set; }

        public List<ManagedImage> Data { get; set; }

        public bool IsVideoRecording { get; set; }
        
        public void DoFinalAction()
        {
            m_isFinalActionRuning = true;

            if (this.Data.Count != 0)
            {
                AviOption option = new AviOption();
                option.frameRate = 7.5F;
                string fileName = this.DirectoryToSaveVideo + @"\" + "PG_Video" + "_" +
                        DateTime.Now.Year.ToString() + "_" +
                        DateTime.Now.Month.ToString() + "_" +
                        DateTime.Now.Day.ToString() + "_" +
                        DateTime.Now.Hour.ToString() + "_" +
                        DateTime.Now.Minute.ToString() + "_" +
                        DateTime.Now.Second.ToString() + ".avi";

                using (ManagedAVIRecorder managedRecoder = new ManagedAVIRecorder())
                {
                    managedRecoder.AVIOpen(fileName, option);

                    foreach (ManagedImage mi in this.Data)
                    {
                        managedRecoder.AVIAppend(mi);
                    }

                    managedRecoder.AVIClose();
                }
            }

            OnFinalStateIsReached();
            this.IsNeedImage = false;
            MessageBox.Show("The video has been recorded successfully", "Video recording is done", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public override object Clone()
        {
            return new VideoRecordingActionPresenter(PossibleActionPresenters.VideoRecording.ToString());
        }

        public event EventHandler FinalStateIsReached;


        private void OnFinalStateIsReached()
        {
            if (FinalStateIsReached != null)
                FinalStateIsReached(this, EventArgs.Empty);
        }

        private void Action(ManagedImage mImg)
        {
            if (this.IsVideoRecording)
            {
                ManagedImage localManagedImage = new ManagedImage(mImg);

                try
                {
                    this.Data.Add(localManagedImage);
                }
                catch (OutOfMemoryException)
                {
                    DoFinalAction();
                }
            }
            else
            {
                if(!m_isFinalActionRuning)
                    DoFinalAction();
            }
                
        }

        private bool m_isFinalActionRuning;
    }
}
