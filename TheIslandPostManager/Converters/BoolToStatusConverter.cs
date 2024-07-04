using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TheIslandPostManager.Converters;

[ValueConversion(typeof(bool), typeof(string))]
internal class BoolToStatusConverter : IValueConverter
{
    #region Implementation of IValueConverter

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value">Bolean value controlling wether to apply color change</param>
    /// <param name="targetType"></param>
    /// <param name="parameter">A CSV string on the format [ColorNameIfTrue;ColorNameIfFalse;OpacityNumber] may be provided for customization, default is [LimeGreen;Transperent;1.0].</param>
    /// <param name="culture"></param>
    /// <returns>A SolidColorBrush in the supplied or default colors depending on the state of value.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
        {
            return "COMPLETED";
        }
        return "PENDING";
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion
}