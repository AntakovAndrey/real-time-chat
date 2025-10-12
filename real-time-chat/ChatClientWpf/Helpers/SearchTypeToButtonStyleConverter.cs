
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ChatClientWpf.Enums;
using ChatClientWpf.Models;

namespace ChatClientWpf.Helpers
{
    public class SearchTypeToButtonStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SearchType currentType && parameter is string paramString)
            {
                if (Enum.TryParse(paramString, out SearchType paramType))
                {
                    return currentType == paramType ? 
                        Application.Current.FindResource("ActiveSearchTypeButton") : 
                        Application.Current.FindResource("SearchTypeButton");
                }
            }
            return Application.Current.FindResource("SearchTypeButton");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}