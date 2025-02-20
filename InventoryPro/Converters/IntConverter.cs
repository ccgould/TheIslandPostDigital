using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace InventoryPro.Converters;

public class IntConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (int.TryParse(text, out var result))
            return result;
        return 0;
    }
}