using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SFD_MVVM.View
{
    /// <summary>
    /// Interaction logic for ExperementalDataFormView.xaml
    /// </summary>
    public partial class ExperementalDataFormView : Window
    {
        public ExperementalDataFormView()
        {
            InitializeComponent();
        }

        public ImageSource Image
        {
            get
            {
                return Img.Source;
            }

            set
            {
                Img.Source = value;
            }
        }
    }

    

    public class EnteredDoseValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string enteredValue = value.ToString();
            double val;
            bool isValid = Double.TryParse(enteredValue, out val);

            if (isValid)
            {
                isValid = isValid && val > 0;

                if(isValid)
                    return new ValidationResult(true, null);
                else
                    return new ValidationResult(false, "Entered concentration is not valid (Concentration must be more than zero)");
            }
            else
                return new ValidationResult(false, "Entered concentration is not valid");
        }
    }

    public class EnteredTimeSpanValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string str = value.ToString();
            str.Replace('.', ':');
            str.Replace(',', ':');
            TimeSpan ts = new TimeSpan();
            bool isValid = TimeSpan.TryParse(str, out ts);

            if (isValid)
            {
                isValid = isValid && ts != TimeSpan.Zero;
                return new ValidationResult(true, null);
            }
            else
                return new ValidationResult(false, "Time interval is not valid");

        }
    }

    public class EnteredMouseNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int val;
            bool isValid = Int32.TryParse(value.ToString(), out val);

            if (isValid && val > 0)
                return new ValidationResult(true, null);
            else
                return new ValidationResult(false, "Ordinal number of a mouse must be bigger than zero");
        }
    }

    public class TimeSpanToStringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value.ToString();
            str.Replace('.', ':');
            str.Replace(',', ':');

            TimeSpan ts = new TimeSpan();
            bool isValid = TimeSpan.TryParse(str, out ts);

            if (isValid)
                return ts;
            else
                return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}
