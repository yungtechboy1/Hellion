using Hellion.Core.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellion.Core.Data.Resources
{
    public sealed class RgnFile
    {
        private byte[] fileData;

        public ICollection<RgnElement> Elements { get; private set; }


        public RgnFile(byte[] rgnData)
        {
            this.fileData = rgnData;
            this.Elements = new List<RgnElement>();
        }


        public void Read()
        {
            using (var memoryStream = new MemoryStream(this.fileData))
            using (var reader = new StreamReader(memoryStream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line) || line.StartsWith(Global.SingleLineComment))
                        continue;

                    if (line.StartsWith("respawn7"))
                    {
                        string[] respawnData = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                        if (respawnData.Length < 24)
                            continue;

                        this.Elements.Add(new RgnRespawn7(respawnData));
                    }
                    // add more...
                }
            }
        }
    }

    public class RgnElement
    {
        public RgnElement()
        {
        }
    }

    public sealed class RgnRespawn7 : RgnElement
    {
        public int Type { get; private set; }

        public int Model { get; private set; }

        public Vector3 Position { get; private set; }

        public int Count { get; private set; }

        public int Time { get; private set; }

        public int AgroNumber { get; private set; }

        public Vector3 StartPosition { get; private set; }

        public Vector3 EndPosition { get; private set; }

        public RgnRespawn7(string[] respawnData)
            : base()
        {
            this.Type = int.Parse(respawnData[1]);
            this.Model = int.Parse(respawnData[2]);
            this.Position = new Vector3(respawnData[3], respawnData[4], respawnData[5]);
            this.Count = int.Parse(respawnData[6]);
            this.Time = int.Parse(respawnData[7]);
            this.AgroNumber = int.Parse(respawnData[8]);
            this.StartPosition = new Vector3(respawnData[9], "", respawnData[10]);
            this.EndPosition = new Vector3(respawnData[11], "", respawnData[12]);
        }
    }
}
