using System;
using System.Windows.Media;

namespace SFD_MVVM.Model
{
    /// <summary>
    ///     Представитель действия (Action Presenter) реализующий этот интерфейс
    ///     берёт на себя ответственность за предоставление результирующего изображения
    ///     которое должно отображатся в видео потоке
    /// </summary>
    interface IActionPresenterChangingResultImage
    {
        event EventHandler NewResultImageIsReady;

        ImageSource NewResultImage { get; set; }
    }
}
