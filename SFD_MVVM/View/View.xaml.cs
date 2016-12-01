using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.IO;
using SFD_MVVM.Model;
using SFD_MVVM.ViewModel;

namespace SFD_MVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Common_ViewModel m_commonVM;
        private Calculation_ViewModel m_calcVM;
        
        public MainWindow()
        {
            InitializeComponent();
            
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;

            Directory.CreateDirectory("Calculation/CO_Data");
            Directory.CreateDirectory("Calculation/Experimental_Data");
            Directory.CreateDirectory("Calculation/Background/Images");

            Directory.CreateDirectory("Calculation/CO_Data/XML_Files");
            Directory.CreateDirectory("Calculation/Experimental_Data/XML_Files");

            Directory.CreateDirectory("Calculation/CO_Data/Images");
            Directory.CreateDirectory("Calculation/Experimental_Data/Images");

            m_commonVM = this.Resources["CommonViewModel"] as Common_ViewModel;
            var hdriVM = this.Resources["HDRIViewModel"] as HDRI_ViewModel;
            var strobeVm = this.Resources["StrobViewModel"] as Stroboscope_ViewModel;
            var calcVM = this.Resources["CalculationViewModel"] as Calculation_ViewModel;
            m_calcVM = calcVM;

            VideoStreamProcessor processor = new VideoStreamProcessor(m_commonVM, calcVM, strobeVm, hdriVM);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_commonVM.Processor.DeleteCurrentCamera();
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SldrTo.Value = SldrTo.Maximum;
        }

        private void useSelectionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Image.MouseUp += SetCursorPosition;
        }

        private void useSelectionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Image.MouseUp -= SetCursorPosition;
        }

        private void SetCursorPosition(object sender, MouseButtonEventArgs e)
        {
            m_calcVM.CursorPosition = e.GetPosition(Image);
        }

        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            if (GC.WaitForFullGCComplete() == GCNotificationStatus.Succeeded)
                Application.Current.Shutdown();
        }
        
    }

    public class DoubleToStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double)value;
            val = Math.Round(val, 3);
            return val.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value as string;

            if (str != null)
            {
                double toReturn;

                if (Double.TryParse(str, out toReturn))
                    return toReturn;
                else
                    return DependencyProperty.UnsetValue;
            }
            else
                return null;
        }

        #endregion
    }

    /// <summary>
    /// Преобразует value в !value, где value это значение типа bool.
    /// Используется в привязке данных в xaml коде в виде:
    /// {Binding Source=*Источник_Данных*, Path=*Свойства_в_источнике данных*, Converter=BoolToBoolConverter}
    /// Экземпляр этого класса является ресурсом главного окна. Поэтому на самом деле пишется так:
    /// {Binding...Converter={StaticResource *имя_ресурса*}} где имя_ресурса это значение атрибута x:Key в ресурсах окна
    /// </summary>
    public class BoolToBoolConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion
    }

    public class MultiBoolAndConverter : IMultiValueConverter
    {

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = (from o in values select o).OfType<bool>().Any<bool>((b) => b == false);

            return !result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Это решение было взято отсюда: http://stackoverflow.com/questions/1083224/pushing-read-only-gui-properties-back-into-viewmodel
    /// Нужно для того чтобы связать свойства ActualHeight и ActualWidth контрола Image (которые только для чтения) с CalculationViewModel
    /// Стандартные средства WPF не позволяют биндить (связывать) readonly свойства
    /// </summary>
    #region Data piping for xaml

    public class DataPiping
    {
        public static readonly DependencyProperty DataPipesProperty = DependencyProperty.RegisterAttached("DataPipes",
            typeof(DataPipeCollection),
            typeof(DataPiping),
            new UIPropertyMetadata(null));

        public static void SetDataPipes(DependencyObject o, DataPipeCollection value)
        {
            o.SetValue(DataPipesProperty, value);
        }

        public static DataPipeCollection GetDataPipes(DependencyObject o)
        {
            return (DataPipeCollection)o.GetValue(DataPipesProperty);
        }
    }

    public class DataPipeCollection : FreezableCollection<DataPipe>
    {

    }

    public class DataPipe : Freezable
    {
        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(DataPipe),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((DataPipe)o).OnSourceChanged(e);
        }

        protected virtual void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            Target = e.NewValue;
        }

        public object Target
        {
            get { return (object)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(object),
            typeof(DataPipe), new FrameworkPropertyMetadata(null));

        protected override Freezable CreateInstanceCore()
        {
            return new DataPipe();
        }
    } 

    #endregion
}
