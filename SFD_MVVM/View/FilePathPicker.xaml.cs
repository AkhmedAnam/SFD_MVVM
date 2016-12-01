using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using forms = System.Windows.Forms;

namespace SFD_MVVM.View
{
    /// <summary>
    /// Interaction logic for FilePathPicker.xaml
    /// </summary>

    [System.Windows.Markup.ContentProperty("FilePath")]
    public partial class FilePathPicker : UserControl
    {
        public FilePathPicker()
        {
            InitializeComponent();
            txtBox.TextChanged += txtBox_TextChanged;
        }

        public static readonly string DefualtDirectory = System.IO.Directory.GetCurrentDirectory();

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(FilePathPicker),
                                     new FrameworkPropertyMetadata(FilePathPicker.DefualtDirectory, null, Coerce), null);

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }

            set { SetValue(FilePathProperty, value); }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            forms.FolderBrowserDialog brwDialog = new forms.FolderBrowserDialog();

            if (brwDialog.ShowDialog() == forms.DialogResult.OK)
                this.FilePath = brwDialog.SelectedPath;
        }

        public void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FilePath = txtBox.Text;

            e.Handled = true;
        }

        public static bool FilePathValidation(object valueToSet)
        {
            string filePath = valueToSet as string;

            if (filePath != null)
                return System.IO.Directory.Exists(filePath);

            return false;
        }

        private static object Coerce(DependencyObject obj, object value)
        {
            FilePathPicker filePicker = obj as FilePathPicker;
            string filePath = value as string;

            if(System.IO.Directory.Exists(filePath))
            {
                var brush = filePicker.BorderBrush as SolidColorBrush;
                if (brush != null)
                {
                    if(brush.Color == Colors.Red)
                    {
                        filePicker.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        filePicker.BorderThickness = new Thickness(0);
                    }
                }
                return filePath;
            }
            else
            {
                filePicker.BorderThickness = new Thickness(1);
                filePicker.BorderBrush = new SolidColorBrush(Colors.Red);
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
