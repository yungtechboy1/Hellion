using Hellion.Core.Data.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefinesToConst
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DefineToConst className defineFilePath");
                return;
            }

            var defineFile = new DefineFile(args[1]);

            Console.WriteLine("Parsing {0} ...", args[1]);
            defineFile.Parse();
            Console.WriteLine("Parsing done.");

            Console.WriteLine("Creating const class {0}...", args[0]);
            CreateConstClass(args[0], defineFile);
            Console.WriteLine("Const class done.");
        }

        private static void CreateConstClass(string className, DefineFile defineFile)
        {
            var sBuilder = new StringBuilder();

            sBuilder.AppendLine("public class " + className);
            sBuilder.AppendLine("{");

            foreach (var define in defineFile.Defines)
            {
                string defineValueType = GetDefineValueType(define.Value);
                string defineValue = GetDefineHexValue(define.Value);

                var format = string.Format("\tpublic const {0} {1} = 0x{2};", defineValueType, define.Key, defineValue);

                sBuilder.AppendLine(format);
            }

            sBuilder.AppendLine("}");

            File.WriteAllText(className + ".cs", sBuilder.ToString());
        }

        private static string GetDefineValueType(object defineValue)
        {
            switch (defineValue.GetType().Name)
            {
                case "Byte": return "byte";
                case "Int16": return "short";
                case "UInt16": return "ushort";
                case "Int32": return "int";
                case "UInt32": return "uint";
                case "Int64": return "long";
                default: return "int";
            }
        }

        private static string GetDefineHexValue(object defineValue)
        {
            switch (defineValue.GetType().Name)
            {
                case "Byte": return byte.Parse(defineValue.ToString()).ToString("X2");
                case "Int16": return short.Parse(defineValue.ToString()).ToString("X4");
                case "UInt16": return ushort.Parse(defineValue.ToString()).ToString("X4");
                case "Int32": return int.Parse(defineValue.ToString()).ToString("X8");
                case "UInt32": return uint.Parse(defineValue.ToString()).ToString("X8");
                case "Int64": return long.Parse(defineValue.ToString()).ToString("X8");
                default: return int.Parse(defineValue.ToString()).ToString("X8");
            }
        }
    }
}
