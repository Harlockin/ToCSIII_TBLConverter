//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="None">
//     No copyright
// </copyright>
// <author>Sébastien R. (a.k.a. Harlockin)</author>
//-----------------------------------------------------------------------
namespace ToCSIII_TBLConverter
{
    using System;
    using System.IO;

    /// <summary>
    /// The entry point of the program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the program.
        /// </summary>
        /// <param name="args">the arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            string path = string.Empty;

            if (args.Length > 0)
            {
                path = @args[0];
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("{0} is not a valid file or directory.", path);
                return;
            }

            switch (Path.GetExtension(path))
            {
                case ".csv":
                    {
                        var file = new TBLFile(path);
                        file.ExportTBL(Path.GetFileNameWithoutExtension(path));
                    }

                    break;
                case ".tbl":
                    {
                        using (BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open)))
                        {
                            var file = new TBLFile(br);
                            file.ExportCSV(Path.GetFileNameWithoutExtension(path));
                        }
                    }

                    break;
                default:
                    Console.WriteLine("This file type is not currently supported. {0}", Path.GetExtension(path));
                    break;
            }

            Console.WriteLine("Done.");
        }
    }
}
