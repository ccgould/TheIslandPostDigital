using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheIslandPostManager.Helpers;

public static class Copier
{
    public static async Task CopyFiles(List<Tuple<string, string>> files, Action<int, string> progressCallback)
    {
        for (var x = 0; x < files.Count; x++)
        {
            var item = files.ElementAt(x);
            var from = item.Item1;
            var to = item.Item2;

            using (var inStream = new FileStream(from, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await Task.Run(() => Copy(inStream, to));
            }

            var result = ((decimal)(x + 1) / files.Count) * 100;

            progressCallback((int)result, to);
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
}
