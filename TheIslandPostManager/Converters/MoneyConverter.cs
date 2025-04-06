using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TheIslandPostManager.Converters;
internal class MoneyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            ConvertToMoney(value.ToString());
        }
        catch (Exception ex)
        {

        }
        return $"$0.00";
    }

    private string ConvertToMoney(string input, bool convert = true)
    {
        var cultureInfo = Thread.CurrentThread.CurrentCulture;   // You can also hardcode the culture, e.g. var cultureInfo = new CultureInfo("fr-FR"), but then you lose culture-specific formatting such as decimal point (. or ,) or the position of the currency symbol (before or after)
        var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
        numberFormatInfo.CurrencySymbol = "$"; // Replace with "$" or "£" or whatever you need

        if (decimal.TryParse(input, out decimal result))
        {
            decimal price = 0m;

            if (convert)
            {
                price = ConvertToDecimal(result);
            }
            else
            {
                price = result;
            }

            return price.ToString("C", numberFormatInfo); // Output: "€ 12.30" if the CurrentCulture is "en-US", "12,30 €" if the CurrentCulture is "fr-FR".
        }

        return "$0";
    }


    private decimal ConvertToDecimal(decimal result)
    {
        return result * 0.01m;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
