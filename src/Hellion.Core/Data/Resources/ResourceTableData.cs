using System.Collections.Generic;

namespace Hellion.Core.Data.Resources
{
    public class ResourceTableData
    {
        private Dictionary<string, string> tableData;

        /// <summary>
        /// Gets or sets the resource table data.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get { return this.tableData.ContainsKey(key) ? this.tableData[key] : string.Empty; }
            set
            {
                if (this.tableData.ContainsKey(key))
                    this.tableData[key] = value;
                else
                    this.tableData.Add(key, value);
            }
        }
        
        /// <summary>
        /// Creates a new ResourceTableData instance.
        /// </summary>
        public ResourceTableData()
        {
            this.tableData = new Dictionary<string, string>();
        }
    }
}
