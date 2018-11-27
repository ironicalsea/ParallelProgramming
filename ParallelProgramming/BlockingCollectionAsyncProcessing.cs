using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParallelProgramming
{
    public class BlockingCollectionAsyncProcessing
    {
        public void ProcessFile(string inputPath, string outputPath)
        {
            var inputLines = new List<string>();
            var processedLines = new List<string>();

            // read #1
            foreach (var line in File.ReadLines(inputPath))
            {
                inputLines.Add(line);
            }

            // process #2
            foreach (var line in inputLines
                .Select(line => Regex.Replace(line, @"\s+", ", ")))
            {
                processedLines.Add(line);
            }

            // write #3
            File.WriteAllLines(outputPath, processedLines);
        }
    }
}