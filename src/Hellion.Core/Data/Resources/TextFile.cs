using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hellion.Core.Data.Resources
{
    /// <summary>
    /// FlyFF Text file.
    /// </summary>
    public class TextFile
    {
        private string filePath;

        public Dictionary<string, string> Texts { get; private set; }

        public TextFile(string filePath)
        {
            this.filePath = filePath;
            this.Texts = new Dictionary<string, string>();
        }

        public void Parse()
        {
            using (var fileStream = new FileStream(this.filePath, FileMode.Open, FileAccess.Read))
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

                    string[] texts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (texts.Length > 2)
                    {
                        string key = texts.First();
                        string value = line.Replace(key, string.Empty).Trim();

                        if (!this.Texts.ContainsKey(key))
                            this.Texts.Add(key, value);
                    }
                }
            }
        }
    }
}
