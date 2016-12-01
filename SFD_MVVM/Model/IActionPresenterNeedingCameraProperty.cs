using System;
using FlyCapture2Managed;

namespace SFD_MVVM.Model
{
    /// <summary>
    ///     Представитель действия, реализующий этот интерфейс,
    ///     требует от процессора получения значения некого свойства камеры.
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    interface IActionPresenterNeedingCameraProperty<TPropertyType>
    {
        event EventHandler SetNeededCameraProperty;
        PropertyType NeededPropertyType { get; set; }
        TPropertyType CurrentNeededPropertyValue { set; }
    }
}
