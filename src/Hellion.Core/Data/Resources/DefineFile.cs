using System;
using System.Collections.Generic;
using System.IO;

namespace Hellion.Core.Data.Resources
{
    /// <summary>
    /// Represents a C/C++ define file.
    /// </summary>
    public sealed class DefineFile
    {
        private const string DefineDirective = "#define";

        private string filePath;

        /// <summary>
        /// Gets the defines dictionary.
        /// </summary>
        public Dictionary<string, int> Defines { get; private set; }

        /// <summary>
        /// Creates a new DefineFile instance.
        /// </summary>
        /// <param name="filePath">Define file path</param>
        public DefineFile(string filePath)
        {
            this.filePath = filePath;
            this.Defines = new Dictionary<string, int>();
        }

        /// <summary>
        /// Parse the define file.
        /// </summary>
        public void Parse()
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (line.StartsWith(Global.SingleLineComment))
                        continue;
                    if (line.StartsWith(Global.MultiLineCommentStart))
                    {
                        while (!line.Contains(Global.MultiLineCommentEnd))
                            line = reader.ReadLine();
                        continue;
                    }
                    if (line.Contains(Global.SingleLineComment))
                        line = line.Remove(line.IndexOf('/'));

                    if (line.StartsWith(DefineDirective))
                    {
                        string[] splitLine = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (splitLine.Length >= 3)
                        {
                            string defineKey = splitLine[1];
                            int defineValue = -1;

                            int.TryParse(splitLine[2], out defineValue);

                            if (!this.Defines.ContainsKey(defineKey))
                                this.Defines.Add(defineKey, defineValue);
                        }
                    }
                }
            }
        }
    }
}
