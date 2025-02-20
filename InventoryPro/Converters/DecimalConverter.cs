using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace InventoryPro.Converters;

public class DecimalConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (decimal.TryParse(text, out var result))
            return result;
        return 0.0;
    }
}
