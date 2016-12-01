using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;

namespace SFD_MVVM.View
{
    /// <summary>
    /// Interaction logic for UpDown.xaml
    /// </summary>
    
    [System.Windows.Markup.ContentProperty("CurrentValue")]
    public partial class UpDown : UserControl
    {
        public UpDown()
        {
            InitializeComponent();

            CurrentValue = 1;
        }

        public static readonly DependencyProperty CurrentValueProperty = DependencyProperty.Register("CurrentValue", typeof(Int64), typeof(UpDown), new FrameworkPropertyMetadata((Int64)1));

        public Int64 CurrentValue
        {
            get { return (Int64)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        private void Btn_Up_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentValue = Int64.Parse(txt.Text);
            this.CurrentValue++;
            //txt.Text = this.CurrentValue.ToString();
        }

        private void Btn_Down_Click(object sender, RoutedEventArgs e)
        {
            this.CurrentValue = Int64.Parse(txt.Text);
            Int64 newVal = this.CurrentValue - 1;

            if (newVal >= 0)
            {
                this.CurrentValue = newVal;
                //txt.Text = this.CurrentValue.ToString();
            }
        }

        private void txt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string pressedKey = e.Key.ToString();
            Regex regularExpression = new Regex(@"(?:\bD\d\b){1}", RegexOptions.Singleline);

            if (!regularExpression.IsMatch(pressedKey) && pressedKey != "Back")
                e.Handled = true;
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt.Text != String.Empty)
                this.CurrentValue = Int64.Parse(txt.Text);

        }
    }

    public class Int64ToStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Int64 toRet;

            if (Int64.TryParse((string)value, out toRet))
                return toRet;
            else
                return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}
