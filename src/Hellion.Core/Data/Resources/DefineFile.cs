using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Hellion.Core.Data.Resources
{
    /// <summary>
    /// Represents a C/C++ define file.
    /// </summary>
    public sealed class DefineFile
    {
        private const string DefineDirective = "#define";
        private const string DwordCast = "(DWORD)";
        private const string WordCast = "(WORD)";
        private const string ByteCast = "(BYTE)";

        private string filePath;

        /// <summary>
        /// Gets the defines dictionary.
        /// </summary>
        public Dictionary<string, object> Defines { get; private set; }

        /// <summary>
        /// Creates a new DefineFile instance.
        /// </summary>
        /// <param name="filePath">Define file path</param>
        public DefineFile(string filePath)
        {
            this.filePath = filePath;
            this.Defines = new Dictionary<string, object>();
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

                            if (!this.Defines.ContainsKey(defineKey))
                                this.Defines.Add(defineKey, this.ParseDefineValue(splitLine[2]));
                        }
                    }
                }
            }
        }

        private object ParseDefineValue(string defineValue)
        {
            object newDefineValue = null;

            try
            {
                if (defineValue.StartsWith(DwordCast))
                {
                    defineValue = defineValue.Replace(DwordCast, string.Empty);
                    newDefineValue = Convert.ToUInt32(defineValue, defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.StartsWith(WordCast))
                {
                    defineValue = defineValue.Replace(WordCast, string.Empty);
                    newDefineValue = Convert.ToUInt16(defineValue, defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.StartsWith(ByteCast))
                {
                    defineValue = defineValue.Replace(ByteCast, string.Empty);
                    newDefineValue = Convert.ToByte(defineValue, defineValue.StartsWith("0x") ? 16 : 10);
                }
                else if (defineValue.EndsWith("L"))
                {
                    defineValue = defineValue.Replace("L", string.Empty);
                    newDefineValue = Convert.ToInt64(defineValue, defineValue.StartsWith("0x") ? 16 : 10);
                }
                else
                {
                    newDefineValue = Convert.ToInt32(defineValue, defineValue.StartsWith("0x") ? 16 : 10);
                }
            }
            catch
            {
                newDefineValue = 0;
            }

            return newDefineValue;
        }
    }
}
