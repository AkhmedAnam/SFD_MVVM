using System;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    /// <summary>
    ///     Базовый класс для представления действия над изображением в классе
    ///     VideoStreamProccessor
    /// </summary>
    public class ActionPresenter : ICloneable
    {
        public ActionPresenter(string id)
        {
            IsNeedImage = true;
            m_ID = id;
        }

        public void ExecuteAction(ManagedImage img)
        {
            m_action(img);
        }

        public bool IsNeedImage
        {
            get { lock (m_locker) return m_isNeedImage; }

            set
            {
                lock (m_locker) m_isNeedImage = value;
                OnIsNeedImageChanged();
            }
        }

        public string ID
        {
            get { return m_ID; }
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion

        public event EventHandler IsNeedImageChanged;

        protected void OnIsNeedImageChanged()
        {
            if (IsNeedImageChanged != null)
                IsNeedImageChanged(this, EventArgs.Empty);
        }
        protected Action<ManagedImage> m_action;
        protected bool m_isNeedImage;
        protected readonly string m_ID;

        private object m_locker = new object();
    }

}
