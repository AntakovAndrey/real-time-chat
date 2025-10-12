using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChatClientWpf.Helpers
{
    public class BoolToMessageStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isMine)
            {
                return isMine ? 
                    Application.Current.FindResource("MyMessageStyle") : 
                    Application.Current.FindResource("OtherMessageStyle");
            }
            return Application.Current.FindResource("OtherMessageStyle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}