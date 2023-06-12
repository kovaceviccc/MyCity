using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Library.Helpers;

public class ButtonCommandParameterConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {

        return values.Clone();
        /*bool booleanValue = (bool)values[0];
        object clickedObject = values[1];

        return new { BooleanValue = booleanValue, ClickedObject = clickedObject };*/
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
