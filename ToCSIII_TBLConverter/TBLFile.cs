//-----------------------------------------------------------------------
// <copyright file="TBLFile.cs" company="None">
//     No copyright
// </copyright>
// <author>Sébastien R. (a.k.a. Harlockin)</author>
//-----------------------------------------------------------------------
namespace ToCSIII_TBLConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.VisualBasic.FileIO;

    /// <summary>
    /// Provide the structure and some utilities for the '.TBL' files.
    /// </summary>
    public class TBLFile
    {
        private short objectCount;
        private short keyCount;
        private Dictionary<string, short> keys;
        private Dictionary<string, List<Item>> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="TBLFile"/> class.
        /// </summary>
        public TBLFile()
        {
            this.objectCount = 0;
            this.keyCount = 0;

            this.keys = new Dictionary<string, short>();

            this.items = new Dictionary<string, List<Item>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TBLFile"/> class.
        /// </summary>
        /// <param name="t_binaryReader">A System.IO.BinaryReader object.</param>
        public TBLFile(BinaryReader t_binaryReader)
        {
            this.objectCount = 0;
            this.keyCount = 0;

            this.keys = new Dictionary<string, short>();
            this.items = new Dictionary<string, List<Item>>();

            // read HEADER
            this.objectCount = t_binaryReader.ReadInt16();
            this.keyCount = t_binaryReader.ReadInt16();
            t_binaryReader.ReadInt16();

            // read KEY LIST
            for (var it = 0; it < this.keyCount; it++)
            {
                this.keys.Add(
                        Helper.ReadNullTerminatedString(t_binaryReader), // key
                        t_binaryReader.ReadInt16());                      // number of objects with this key

                t_binaryReader.ReadInt16();
            }

            // read OBJECTS
            for (var it = 0; it < this.objectCount; it++)
            {
                string key = Helper.ReadNullTerminatedString(t_binaryReader);
                short object_size = t_binaryReader.ReadInt16();

                if (this.items.ContainsKey(key))
                {
                    this.items[key].Add(new Item(t_binaryReader.ReadBytes(object_size)));
                }
                else
                {
                    this.items[key] = new List<Item> { new Item(t_binaryReader.ReadBytes(object_size)) };
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TBLFile"/> class.
        /// </summary>
        /// <param name="t_path">A System.IO.BinaryReader object.</param>
        public TBLFile(string t_path)
        {
            this.objectCount = 0;
            this.keyCount = 0;

            this.keys = new Dictionary<string, short>();
            this.items = new Dictionary<string, List<Item>>();

            using (TextFieldParser csvParser = new TextFieldParser(t_path, System.Text.Encoding.UTF8))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    this.objectCount++;

                    // Read current line fields, pointer moves to the next line.
                    try
                    {
                        string[] fields = csvParser.ReadFields();

                        if (this.items.ContainsKey(fields[0]))
                        {
                            this.keys[fields[0]] += 1;
                            this.items[fields[0]].Add(new Item(fields));
                        }
                        else
                        {
                            this.keyCount++;
                            this.keys.Add(fields[0], 1);
                            this.items[fields[0]] = new List<Item> { new Item(fields) };
                        }
                    }
                    catch (MalformedLineException ex)
                    {
                        Console.WriteLine("Line " + ex.Message + " is invalid. Skipping");
                    }
                }
            }
        }

        /// <summary>
        /// Export the TBL file to an easy to manipulate CSV.
        /// </summary>
        /// <param name="t_filename">The name of the file.</param>
        public void ExportCSV(string t_filename)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + t_filename + ".csv";

            using (var file = File.CreateText(path))
            {
                if (this.keys.ContainsKey("item"))
                {
                    file.WriteLine(Helper.GetItemHeader());

                    // we list each object
                    foreach (var key in this.items)
                    {
                        foreach (var item in key.Value)
                        {
                            file.WriteLine(key.Key + item.ToCSVString());
                        }

                        file.WriteLine(string.Empty);
                    }
                }

                file.Close();
            }

            return;
        }

        /// <summary>
        /// Export the CSV file to a TBL binary file.
        /// </summary>
        /// <param name="t_filename">The name of the file.</param>
        public void ExportTBL(string t_filename)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + t_filename + ".tbl";

            using (var writer = new BinaryWriter(File.Open(path, FileMode.Create)))
            {
                // Write header
                writer.Write(this.objectCount);
                writer.Write(this.keyCount);
                writer.Write(ushort.MinValue);

                // Write key list
                foreach (var key in this.keys)
                {
                    writer.Write(UTF8Encoding.UTF8.GetBytes(key.Key + "\0"));
                    writer.Write(key.Value);
                    writer.Write(ushort.MinValue);
                }

                // Write object list
                foreach (var key in this.items)
                {
                    foreach (var item in key.Value)
                    {
                        writer.Write(UTF8Encoding.UTF8.GetBytes(key.Key + "\0"));
                        writer.Write(item.ToByteArray());
                    }
                }

                writer.Close();
            }
        }
    }
}
