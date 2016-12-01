using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlyCapture2Managed;
using System.Threading;

namespace SFD_MVVM.Model
{
    public class SynchronizedCamera
    {
        public SynchronizedCamera(ManagedCamera camera)
        {
            m_camera = camera;
        }


        public ManagedCamera Camera
        {
            get { return m_camera; }
        }

        public float Shutter
        {
            get
            {
                m_locker.EnterReadLock();
                try
                {
                    CameraProperty cp = m_camera.GetProperty(PropertyType.Shutter);
                    return cp.absValue;
                }
                finally
                {
                    m_locker.ExitReadLock();
                }
            }

            set
            {
                CameraProperty toSet = m_camera.GetProperty(PropertyType.Shutter);
                toSet.absValue = value;

                m_locker.EnterWriteLock();
                try
                {
                    m_camera.SetProperty(toSet);
                }
                finally
                {
                    m_locker.ExitWriteLock();
                }
            }
        }

        public float Gain
        {
            get
            {
                m_locker.EnterReadLock();
                try
                {
                    return (m_camera.GetProperty(PropertyType.Gain)).absValue;
                }
                finally
                {
                    m_locker.ExitReadLock();
                }
            }
        }

        public float FrameRate
        {
            get
            {
                m_locker.EnterReadLock();
                try
                {
                    return m_camera.GetProperty(PropertyType.FrameRate).absValue;
                }
                finally
                {
                    m_locker.ExitReadLock();
                }
            }
        }

        public bool ShutterMode
        {
            get
            {
                m_locker.EnterReadLock();
                try
                {
                    return (m_camera.GetProperty(PropertyType.Shutter)).autoManualMode;
                }
                finally
                {
                    m_locker.ExitReadLock();
                }
            }
            set
            {
                CameraProperty cp = m_camera.GetProperty(PropertyType.Shutter);
                cp.autoManualMode = value;


                m_locker.EnterWriteLock();
                try
                {
                    m_camera.SetProperty(cp);
                }
                finally
                {
                    m_locker.ExitWriteLock();
                }
            }
        }

        public bool GainMode
        {
            get
            {
                m_locker.EnterReadLock();
                try
                {
                    return (m_camera.GetProperty(PropertyType.Gain)).autoManualMode;
                }
                finally
                {
                    m_locker.ExitReadLock();
                }
            }
            set
            {
                CameraProperty cp = m_camera.GetProperty(PropertyType.Gain);
                cp.autoManualMode = value;

                m_locker.EnterWriteLock();
                try
                {
                    m_camera.SetProperty(cp);
                }
                finally
                {
                    m_locker.ExitWriteLock();
                }
            }
        }

        public bool FrameRateMode
        {
            get
            {
                m_locker.EnterReadLock();
                try
                {
                    return (m_camera.GetProperty(PropertyType.FrameRate)).autoManualMode;
                }
                finally
                {
                    m_locker.ExitReadLock();
                }
            }

            set
            {
                CameraProperty cp = m_camera.GetProperty(PropertyType.FrameRate);
                cp.autoManualMode = value;

                m_locker.EnterWriteLock();
                try
                {
                    m_camera.SetProperty(cp);
                }
                finally
                {
                    m_locker.ExitWriteLock();
                }
            }
        }

        public bool AutoExposer
        {
            set
            {
                CameraProperty p = new CameraProperty(PropertyType.AutoExposure);
                p.autoManualMode = value;

                m_camera.SetProperty(p);
            }
        }

        public CameraPropertyInfo FrameRateInfo
        {
            get
            {
                m_locker.EnterWriteLock();
                try
                {
                    return m_camera.GetPropertyInfo(PropertyType.FrameRate);
                }
                finally
                {
                    m_locker.ExitWriteLock();
                }
            }
        }

        public void SetShutter(float value = 1.0F, bool autoManualMode = false)
        {
            CameraProperty toSet = m_camera.GetProperty(PropertyType.Shutter);
            toSet.absValue = value; toSet.autoManualMode = autoManualMode;

            m_locker.EnterWriteLock();
            try
            {
                m_camera.SetProperty(toSet);
            }
            finally
            {
                m_locker.ExitWriteLock();
            }

        }

        //public float GetShutter()
        //{
        //    _locker.EnterReadLock();
        //    try
        //    {
        //        CameraProperty cp = _camera.GetProperty(PropertyType.Shutter);
        //        return cp.absValue;
        //    }
        //    finally
        //    {
        //        _locker.ExitReadLock();
        //    }
        //}

        public void SetGain(float value = 1.0F, bool autoManualMode = false)
        {
            CameraProperty toSet = m_camera.GetProperty(PropertyType.Gain);
            toSet.absValue = value; toSet.autoManualMode = autoManualMode;

            m_locker.EnterWriteLock();
            try
            {
                m_camera.SetProperty(toSet);
            }
            finally
            {
                m_locker.ExitWriteLock();
            }

        }

        public void SetFrameRate(float value = 1.0F, bool autoManualMode = false)
        {
            CameraProperty toSet = m_camera.GetProperty(PropertyType.FrameRate);
            toSet.absValue = value; toSet.autoManualMode = autoManualMode;

            m_locker.EnterWriteLock();
            try
            {
                m_camera.SetProperty(toSet);
            }
            finally
            {
                m_locker.ExitWriteLock();
            }

        }







        //Потоково-безопасное получение изображения из буфера камеры
        public void RetrieveImage(ManagedImage img)
        {
            m_locker.EnterWriteLock();
            try
            {
                m_camera.RetrieveBuffer(img);
            }
            finally
            {
                m_locker.ExitWriteLock();
            }
        }

        private ManagedCamera m_camera;
        private ReaderWriterLockSlim m_locker = new ReaderWriterLockSlim();
    }
}
