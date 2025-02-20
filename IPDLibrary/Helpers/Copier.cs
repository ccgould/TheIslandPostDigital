namespace IPDLibrary.Helpers;

public static class Copier
{
    public static async Task CopyFiles(List<Tuple<string, string>> files, Action<int, string> progressCallback)
    {
        if (files.Any())
        {
            var path = Path.GetDirectoryName(files[0].Item2);

            for (var x = 0; x < files.Count; x++)
            {
                var item = files.ElementAt(x);
                var from = item.Item1;
                var to = item.Item2;

                using (var inStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    string curPath = to;

                    if (File.Exists(curPath))
                    {
                        List<string> imageFiles = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).Where(file => new string[] { ".JPG", ".JPEG", ".RAF" }.Contains(Path.GetExtension(file))).ToList();
                        var imageNames = new List<string>();

                        foreach (var imageFile in imageFiles)
                        {
                            imageNames.Add(Path.GetFileNameWithoutExtension(imageFile));
                        }


                        curPath = Path.Combine(path, $"{imageNames.GetUniqueString(Path.GetFileNameWithoutExtension(curPath))}{Path.GetExtension(curPath)}");
                    }


                    await Task.Run(() => Copy(inStream, curPath));
                }

                var result = (decimal)(x + 1) / files.Count * 100;

                progressCallback((int)result, to);
            }
        }
    }

    public static void Copy(Stream inStream, string outputFilePath)
    {
        int bufferSize = 1024 * 1024;

        using (FileStream fileStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            fileStream.SetLength(inStream.Length);
            int bytesRead = -1;
            byte[] bytes = new byte[bufferSize];

            while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0)
            {
                fileStream.Write(bytes, 0, bytesRead);
            }
        }
    }

    /// <summary>
    /// Generate a uniquely numbered string to insert into this list
    /// Uses convention of appending the value with the duplication index number in brackets "~ (#)"
    /// </summary>
    /// <remarks>This will not actually add this list</remarks>
    /// <param name="input">The string to evaluate against this collection</param>
    /// <param name="comparer">[Optional] One of the enumeration values that specifies how the strings will be compared, will default to OrdinalIgnoreCase </param>
    /// <returns>A numbered variant of the input string that would be unique in the list of current values</returns>
    public static string GetUniqueString(this IList<string> currentValues, string input, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        // This matches the pattern we are using, i.e. "A String Value (#)"
        var regex = new System.Text.RegularExpressions.Regex(@"\(([0-9]+)\)$");

        // this is the comparison value that we want to increment
        string prefix = input.Trim();
        string result = input.Trim();

        // let it through if there is no current match
        if (currentValues.Any(x => x.Equals(input, comparison)))
        {
            // Identify if the input value has already been incremented (makes this more reusable)
            var inputMatch = regex.Match(input);
            if (inputMatch.Success)
            {
                // this is the matched value
                var number = inputMatch.Groups[1].Captures[0].Value;
                // remove the numbering from the alias to create the prefix
                prefix = input.Replace(string.Format("({0})", number), "").Trim();
            }

            // Now evaluate all the existing items that have the same prefix
            // NOTE: you can do this as one line in Linq, this is a bit easier to read
            // I'm trimming the list for consistency
            var potentialDuplicates = currentValues.Select(x => x.Trim()).Where(x => x.StartsWith(prefix, comparison));
            int count = 0;
            int maxIndex = 0;
            foreach (string item in potentialDuplicates)
            {
                // Get the index from the current item
                var indexMatch = regex.Match(item);
                if (indexMatch.Success)
                {
                    var index = int.Parse(indexMatch.Groups[1].Captures[0].Value);
                    var test = item.Replace(string.Format("({0})", index), "").Trim();
                    if (test.Equals(prefix, comparison))
                    {
                        count++;
                        maxIndex = Math.Max(maxIndex, index);
                    }
                }
            }
            int nextIndex = Math.Max(maxIndex, count) + 1;
            result = string.Format("{0} ({1})", prefix, nextIndex);
        }

        return result;
    }
}
