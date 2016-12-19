using Hellion.Core.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Hellion.Core.Data.Resources
{
    /// <summary>
    /// This class is used to parse and exploit the FlyFF resources like propItem.txt, propSkills.txt, etc...
    /// </summary>
    public class ResourceTable
    {
        private byte[] fileData;
        private List<string> headers;
        private Dictionary<string, int> defines;
        private Dictionary<string, string> texts;
        private ICollection<ResourceTableData> tableData;

        /// <summary>
        /// Gets the reading index.
        /// </summary>
        public int ReadingIndex { get; private set; }

        /// <summary>
        /// Gets the number of data inside this resource table.
        /// </summary>
        public int Count
        {
            get { return this.tableData.Count; }
        }

        /// <summary>
        /// Creates a new ResourceTable instance using a file path.
        /// </summary>
        /// <param name="filePath"></param>
        public ResourceTable(string filePath)
            : this(File.ReadAllBytes(filePath))
        {
        }

        /// <summary>
        /// Creates a new ResourceTable instance using a byte array representing the file data.
        /// </summary>
        /// <param name="fileData"></param>
        public ResourceTable(byte[] fileData)
        {
            this.fileData = fileData;
            this.ReadingIndex = -1;
            this.headers = new List<string>();
            this.defines = new Dictionary<string, int>();
            this.texts = new Dictionary<string, string>();
            this.tableData = new List<ResourceTableData>();
        }

        /// <summary>
        /// Set the table headers.
        /// </summary>
        /// <param name="headers"></param>
        public void SetTableHeaders(params string[] headers)
        {
            this.headers.AddRange(headers);
        }

        /// <summary>
        /// Add a dictionary of defines to this resource table.
        /// </summary>
        /// <param name="definesToAdd"></param>
        public void AddDefines(Dictionary<string, int> definesToAdd)
        {
            foreach (var definesKeyValue in definesToAdd)
                this.defines.Add(definesKeyValue.Key, definesKeyValue.Value);
        }

        /// <summary>
        /// Add a dictionary of texts to this resource table.
        /// </summary>
        /// <param name="textsToAdd"></param>
        public void AddTexts(Dictionary<string, string> textsToAdd)
        {
            foreach (var textsKeyValue in textsToAdd)
                this.texts.Add(textsKeyValue.Key, textsKeyValue.Value);
        }

        /// <summary>
        /// Read the resource table.
        /// </summary>
        /// <returns></returns>
        public bool Read()
        {
            ++this.ReadingIndex;

            if (this.ReadingIndex > this.tableData.Count - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Parse the resource table raw data.
        /// </summary>
        public void Parse()
        {
            using (var memoryStream = new MemoryStream(this.fileData))
            using (var reader = new StreamReader(memoryStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    // Remove comments from line
                    if (line.StartsWith(Global.SingleLineComment))
                        continue;
                    if (line.StartsWith(Global.MultiLineCommentStart))
                    {
                        while (line.Contains(Global.MultiLineCommentEnd) == false)
                            line = reader.ReadLine();
                        continue;
                    }
                    if (line.Contains(Global.SingleLineComment))
                        line = line.Remove(line.IndexOf("/"));

                    line = line.Replace(",,", ",=,").Replace(",", "\t");
                    string[] lineData = line.Split(new char[] { '\t', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (lineData.Length == this.headers.Count)
                    {
                        var data = new ResourceTableData();

                        for (int i = 0; i < lineData.Length; ++i)
                        {
                            string dataValue = lineData[i].Trim();

                            if (this.defines.ContainsKey(dataValue))
                                dataValue = this.defines[dataValue].ToString();
                            else if (this.texts.ContainsKey(dataValue))
                                dataValue = this.texts[dataValue];

                            dataValue = dataValue.Replace("=", "0").Replace(",", ".").Replace("\"", "");
                            data[this.headers[i]] = dataValue;
                        }

                        this.tableData.Add(data);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the value of the current line using a header key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var value = this.tableData.ElementAt(this.ReadingIndex)?[key];

            try
            {
                if (string.IsNullOrEmpty(value))
                    return default(T);

                if (value.StartsWith("0x"))
                {
                    value = value.Remove(0, 2);
                    value = uint.Parse(value, NumberStyles.HexNumber).ToString();
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception e)
            {
                Log.Error("Unable to get the value of key: {0}.", key);
                Log.Debug("StackTrace: {0}", e.StackTrace);
            }

            return default(T);
        }
    }
}
