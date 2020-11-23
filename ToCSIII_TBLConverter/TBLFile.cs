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
        private Dictionary<string, List<ToCSIII_TBLConverter.Object>> objects;
        private Dictionary<string, List<UnhandledObject>> unhandledObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="TBLFile"/> class.
        /// </summary>
        public TBLFile()
        {
            this.objectCount = 0;
            this.keyCount = 0;

            this.keys = new Dictionary<string, short>();
            this.objects = new Dictionary<string, List<Object>>();
            this.unhandledObjects = new Dictionary<string, List<UnhandledObject>>();
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
            this.objects = new Dictionary<string, List<Object>>();
            this.unhandledObjects = new Dictionary<string, List<UnhandledObject>>();

            // read HEADER
            this.objectCount = t_binaryReader.ReadInt16();
            this.keyCount = t_binaryReader.ReadInt16();
            t_binaryReader.ReadInt16();

            // read KEY LIST
            for (var it = 0; it < this.keyCount; it++)
            {
                this.keys.Add(
                        Helper.ReadNullTerminatedString(t_binaryReader), // key
                        t_binaryReader.ReadInt16());                     // number of objects with this key

                t_binaryReader.ReadInt16();
            }

            // read OBJECTS
            for (var it = 0; it < this.objectCount; it++)
            {
                string key = Helper.ReadNullTerminatedString(t_binaryReader);
                short object_size = t_binaryReader.ReadInt16();

                if (key == "item")
                {
                    if (this.objects.ContainsKey(key))
                    {
                        this.objects[key].Add(new Item(t_binaryReader.ReadBytes(object_size)));
                    }
                    else
                    {
                        this.objects[key] = new List<Object> { new Item(t_binaryReader.ReadBytes(object_size)) };
                    }
                }
                else if (key == "item_q")
                {
                    if (this.objects.ContainsKey(key))
                    {
                        this.objects[key].Add(new ItemQuartz(t_binaryReader.ReadBytes(object_size)));
                    }
                    else
                    {
                        this.objects[key] = new List<Object> { new ItemQuartz(t_binaryReader.ReadBytes(object_size)) };
                    }
                }
                else if (key == "magic")
                {
                    if (this.objects.ContainsKey(key))
                    {
                        this.objects[key].Add(new Magic(t_binaryReader.ReadBytes(object_size)));
                    }
                    else
                    {
                        this.objects[key] = new List<Object> { new Magic(t_binaryReader.ReadBytes(object_size)) };
                    }
                }
                else if (key == "ItemHelpData")
                {
                    if (this.objects.ContainsKey(key))
                    {
                        this.objects[key].Add(new ItemHelpData(t_binaryReader.ReadBytes(object_size)));
                    }
                    else
                    {
                        this.objects[key] = new List<Object> { new ItemHelpData(t_binaryReader.ReadBytes(object_size)) };
                    }
                }
                else if (key == "CompHelpData")
                {
                    if (this.objects.ContainsKey(key))
                    {
                        this.objects[key].Add(new CompHelpData(t_binaryReader.ReadBytes(object_size)));
                    }
                    else
                    {
                        this.objects[key] = new List<Object> { new CompHelpData(t_binaryReader.ReadBytes(object_size)) };
                    }
                }
                else
                {
                    if (this.unhandledObjects.ContainsKey(key))
                    {
                        this.unhandledObjects[key].Add(new UnhandledObject(t_binaryReader.ReadBytes(object_size)));
                    }
                    else
                    {
                        this.unhandledObjects[key] = new List<UnhandledObject> { new UnhandledObject(t_binaryReader.ReadBytes(object_size)) };
                    }
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
            this.objects = new Dictionary<string, List<Object>>();
            this.unhandledObjects = new Dictionary<string, List<UnhandledObject>>();

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

                        if (fields[0] == "item")
                        {
                            if (this.objects.ContainsKey(fields[0]))
                            {
                                this.keys[fields[0]] += 1;
                                this.objects[fields[0]].Add(new Item(fields));
                            }
                            else
                            {
                                this.keyCount++;
                                this.keys.Add(fields[0], 1);
                                this.objects[fields[0]] = new List<Object> { new Item(fields) };
                            }
                        }
                        else if (fields[0] == "item_q")
                        {
                            if (this.objects.ContainsKey(fields[0]))
                            {
                                this.keys[fields[0]] += 1;
                                this.objects[fields[0]].Add(new ItemQuartz(fields));
                            }
                            else
                            {
                                this.keyCount++;
                                this.keys.Add(fields[0], 1);
                                this.objects[fields[0]] = new List<Object> { new ItemQuartz(fields) };
                            }
                        }
                        else if (fields[0] == "magic")
                        {
                            if (this.objects.ContainsKey(fields[0]))
                            {
                                this.keys[fields[0]] += 1;
                                this.objects[fields[0]].Add(new Magic(fields));
                            }
                            else
                            {
                                this.keyCount++;
                                this.keys.Add(fields[0], 1);
                                this.objects[fields[0]] = new List<Object> { new Magic(fields) };
                            }
                        }
                        else if (fields[0] == "ItemHelpData")
                        {
                            if (this.objects.ContainsKey(fields[0]))
                            {
                                this.keys[fields[0]] += 1;
                                this.objects[fields[0]].Add(new ItemHelpData(fields));
                            }
                            else
                            {
                                this.keyCount++;
                                this.keys.Add(fields[0], 1);
                                this.objects[fields[0]] = new List<Object> { new ItemHelpData(fields) };
                            }
                        }
                        else if (fields[0] == "CompHelpData")
                        {
                            if (this.objects.ContainsKey(fields[0]))
                            {
                                this.keys[fields[0]] += 1;
                                this.objects[fields[0]].Add(new CompHelpData(fields));
                            }
                            else
                            {
                                this.keyCount++;
                                this.keys.Add(fields[0], 1);
                                this.objects[fields[0]] = new List<Object> { new CompHelpData(fields) };
                            }
                        }
                        else
                        {
                            if (this.unhandledObjects.ContainsKey(fields[0]))
                            {
                                this.keys[fields[0]] += 1;
                                this.unhandledObjects[fields[0]].Add(new UnhandledObject(fields));
                            }
                            else
                            {
                                this.keyCount++;
                                this.keys.Add(fields[0], 1);
                                this.unhandledObjects[fields[0]] = new List<UnhandledObject> { new UnhandledObject(fields) };
                            }
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
                if (this.objects.ContainsKey("item"))
                {
                    file.WriteLine(Helper.GetItemHeader());

                    // we list each object
                    foreach (var key in this.objects)
                    {
                        foreach (var @object in key.Value)
                        {
                            if (key.Key == "item")
                            {
                                var item = @object as Item;
                                file.WriteLine(key.Key + item.ToCSVString());
                            }
                            else if (key.Key == "item_q")
                            {
                                var itemQuartz = @object as ItemQuartz;
                                file.WriteLine(key.Key + itemQuartz.ToCSVString());
                            }
                        }
                    }
                }

                if (this.objects.ContainsKey("magic"))
                {
                    file.WriteLine(Helper.GetMagicHeader());

                    // we list each object
                    foreach (var key in this.objects)
                    {
                        foreach (var @object in key.Value)
                        {
                            var magic = @object as Magic;
                            file.WriteLine(key.Key + magic.ToCSVString());
                        }
                    }
                }

                if (this.objects.ContainsKey("ItemHelpData"))
                {
                    file.WriteLine(Helper.GetHelpDataHeader());

                    // we list each object
                    foreach (var key in this.objects)
                    {
                        foreach (var @object in key.Value)
                        {
                            if (key.Key == "ItemHelpData")
                            {
                                var itemHelpData = @object as ItemHelpData;
                                file.WriteLine(key.Key + itemHelpData.ToCSVString());
                            }
                            else if (key.Key == "CompHelpData")
                            {
                                var compHelpData = @object as CompHelpData;
                                file.WriteLine(key.Key + compHelpData.ToCSVString());
                            }
                        }
                    }
                }

                if (this.unhandledObjects.Count > 0)
                {
                    // we list each object
                    foreach (var key in this.unhandledObjects)
                    {
                        foreach (var unhandledObject in key.Value)
                        {
                            file.WriteLine(key.Key + unhandledObject.ToCSVString());
                        }
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
                foreach (var key in this.objects)
                {
                    foreach (var @object in key.Value)
                    {
                        writer.Write(UTF8Encoding.UTF8.GetBytes(key.Key + "\0"));

                        if (key.Key == "item")
                        {
                            var item = @object as Item;
                            writer.Write(item.ToByteArray());
                        }
                        else if (key.Key == "item_q")
                        {
                            var itemQuartz = @object as ItemQuartz;
                            writer.Write(itemQuartz.ToByteArray());
                        }
                        else if (key.Key == "magic")
                        {
                            var magic = @object as Magic;
                            writer.Write(magic.ToByteArray());
                        }
                        else if (key.Key == "ItemHelpData")
                        {
                            var itemHelpData = @object as ItemHelpData;
                            writer.Write(itemHelpData.ToByteArray());
                        }
                        else if (key.Key == "CompHelpData")
                        {
                            var compHelpData = @object as CompHelpData;
                            writer.Write(compHelpData.ToByteArray());
                        }
                    }
                }
                
                foreach (var key in this.unhandledObjects)
                {
                    foreach (var @object in key.Value)
                    {
                        writer.Write(UTF8Encoding.UTF8.GetBytes(key.Key + "\0"));
                        writer.Write((short)@object.ToByteArray().Length);
                        writer.Write(@object.ToByteArray());
                    }
                }

                writer.Close();
            }
        }
    }
}
