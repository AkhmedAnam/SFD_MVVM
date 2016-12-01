using System;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    
    /// <summary>
    /// Представитель действия, реализующий этот интерфейс,
    /// требует от процессора возможности изменения свойств камеры.
    /// Например, когда выполняется HDRI величина шаттера постоянно меняется
    /// и новое значение должно отображаться на камере
    /// </summary>
    public interface IActionPresenterNeededChangingCameraProperty
    {
        event EventHandler<ChangeCameraPropertyArgs> ChangeCameraProperty;
    }

    public class ChangeCameraPropertyArgs : EventArgs
    {
        public ChangeCameraPropertyArgs(PropertyType propertyType, float value)
        {
            this.ValueToSet = value; this.PropertyType = propertyType;
        }
        public float ValueToSet { get; set; }

        public PropertyType PropertyType { get; set; }
    }
}
