//-----------------------------------------------------------------------
// <copyright file="Item.cs" company="None">
//     No copyright
// </copyright>
// <author>Sébastien R. (a.k.a. Harlockin)</author>
//-----------------------------------------------------------------------
namespace ToCSIII_TBLConverter
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provide the structure and some utilities for the 'item' and 'item_q' objects.
    /// </summary>
    public class Item
    {
        private short id;
        private short characterRestriction;
        private string flags;
        private short category;
        private short unknowShort;
        private byte element;
        private byte weaponSwitchable;
        private byte weaponSlash;
        private byte weaponPierce;
        private byte weaponThrust;
        private byte weaponStrike;
        private byte targetType;
        private float targetRange;
        private byte targetSize;
        private byte[] unknow12bytes;
        private Effect[] effects;
        private short[] stats;
        private int price;
        private byte stackMax;
        private short rank;
        private string name;
        private string desc;
        private byte[] unknow8bytes;
        private short unknowQuartzShort1;
        private short unknowQuartzShort2;
        private short unknowQuartzShort3;

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="t_data">An array of byte containing an unprocessed Item.</param>
        public Item(byte[] t_data)
        {
            this.unknow12bytes = new byte[12];
            this.effects = new Effect[Helper.MaxItemEffects];
            this.stats = new short[10];
            this.unknow8bytes = new byte[8];

            using (BinaryReader br = new BinaryReader(new MemoryStream(t_data)))
            {
                this.id = br.ReadInt16();
                this.characterRestriction = br.ReadInt16();
                this.flags = Helper.ReadNullTerminatedString(br);
                this.category = br.ReadInt16();
                this.unknowShort = br.ReadInt16();
                this.element = br.ReadByte();
                this.weaponSwitchable = br.ReadByte();
                this.weaponSlash = br.ReadByte();
                this.weaponPierce = br.ReadByte();
                this.weaponThrust = br.ReadByte();
                this.weaponStrike = br.ReadByte();
                this.targetType = br.ReadByte();
                this.targetRange = br.ReadSingle();
                this.targetSize = br.ReadByte();
                this.unknow12bytes = br.ReadBytes(12);

                for (var i = 0; i < Helper.MaxItemEffects; i++)
                {
                    this.effects[i] = new Effect(br);
                }

                for (var i = 0; i < 10; i++)
                {
                    this.stats[i] = br.ReadInt16();
                }

                this.price = br.ReadInt32();
                this.stackMax = br.ReadByte();
                this.rank = br.ReadInt16();
                br.ReadInt16(); // -1
                this.name = Helper.ReadNullTerminatedString(br);
                this.desc = Helper.ReadNullTerminatedString(br);
                this.unknow8bytes = br.ReadBytes(8);

                if (br.BaseStream.Length > br.BaseStream.Position)
                {
                    this.unknowQuartzShort1 = br.ReadInt16();
                    this.unknowQuartzShort2 = br.ReadInt16();
                    this.unknowQuartzShort3 = br.ReadInt16();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        /// <param name="t_string">A string array containing an Item.</param>
        public Item(string[] t_string)
        {
            this.unknow12bytes = new byte[12];
            this.effects = new Effect[Helper.MaxItemEffects];
            this.stats = new short[10];
            this.unknow8bytes = new byte[8];

            this.id = short.Parse(t_string[1]);
            this.name = t_string[2].Replace("\\n", "\n");
            this.desc = t_string[3].Replace("\\n", "\n");
            this.category = short.Parse(t_string[4]);
            this.price = int.Parse(t_string[5]);
            this.stackMax = byte.Parse(t_string[6]);

            for (var i = 0; i < Helper.MaxItemEffects; i++)
            {
                this.effects[i] = new Effect(short.Parse(t_string[7 + (4 * i)]), int.Parse(t_string[8 + (4 * i)]), int.Parse(t_string[9 + (4 * i)]), int.Parse(t_string[10 + (4 * i)]));
            }

            // 26
            for (var i = 0; i < 10; i++)
            {
                this.stats[i] = short.Parse(t_string[27 + i]);
            }

            // 36
            this.rank = short.Parse(t_string[37]);
            this.element = byte.Parse(t_string[38]);
            this.weaponSwitchable = byte.Parse(t_string[39]);
            this.weaponSlash = byte.Parse(t_string[40]);
            this.weaponPierce = byte.Parse(t_string[41]);
            this.weaponThrust = byte.Parse(t_string[42]);
            this.weaponStrike = byte.Parse(t_string[43]);
            this.targetType = byte.Parse(t_string[44]);
            this.targetRange = float.Parse(t_string[45]);
            this.targetSize = byte.Parse(t_string[46]);
            this.characterRestriction = short.Parse(t_string[47]);
            this.flags = t_string[48];

            this.unknowShort = short.Parse(t_string[49]);
            this.unknow12bytes = Helper.StringToByteArray(t_string[50].Replace(" ", string.Empty));
            this.unknow8bytes = Helper.StringToByteArray(t_string[51].Replace(" ", string.Empty));

            if (t_string[0] == "item_q")
            {
                this.unknowQuartzShort1 = short.Parse(t_string[52]);
                this.unknowQuartzShort2 = short.Parse(t_string[53]);
                this.unknowQuartzShort3 = short.Parse(t_string[54]);
            }
        }

        /// <summary>
        /// Write the current Item to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public string ToCSVString()
        {
            string line = string.Empty;

            line += "," + this.id.ToString();
            line += "," + '"' + this.name.Replace("\n", "\\n") + '"';
            line += "," + '"' + this.desc.Replace("\n", "\\n") + '"';
            line += "," + this.category.ToString();
            line += "," + this.price.ToString();
            line += "," + this.stackMax.ToString();

            for (var i = 0; i < Helper.MaxItemEffects; i++)
            {
                line += "," + this.effects[i].ToCSVString();
            }

            for (var i = 0; i < 10; i++)
            {
                line += "," + this.stats[i].ToString();
            }

            line += "," + this.rank.ToString();
            line += "," + this.element.ToString();
            line += "," + this.weaponSwitchable.ToString();
            line += "," + this.weaponSlash.ToString();
            line += "," + this.weaponPierce.ToString();
            line += "," + this.weaponThrust.ToString();
            line += "," + this.weaponStrike.ToString();
            line += "," + this.targetType.ToString();
            line += "," + this.targetRange.ToString();
            line += "," + this.targetSize.ToString();
            line += "," + this.characterRestriction.ToString();
            line += "," + '"' + this.flags + '"';

            // Unknows
            line += "," + this.unknowShort.ToString();
            line += "," + Helper.ByteArrayToString(this.unknow12bytes);
            line += "," + Helper.ByteArrayToString(this.unknow8bytes);

            if (this.unknow8bytes.Any(singleByte => singleByte != 0))
            {
                line += "," + this.unknowQuartzShort1.ToString();
                line += "," + this.unknowQuartzShort2.ToString();
                line += "," + this.unknowQuartzShort3.ToString();
            }

            return line;
        }

        /// <summary>
        /// Write the current Item to an array of bytes.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public byte[] ToByteArray()
        {
            byte[] array = new byte[23947];

            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(short.MinValue);
                    writer.Write(this.id);
                    writer.Write(this.characterRestriction);
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.flags + "\0"));
                    writer.Write(this.category);
                    writer.Write(this.unknowShort);
                    writer.Write(this.element);
                    writer.Write(this.weaponSwitchable);
                    writer.Write(this.weaponSlash);
                    writer.Write(this.weaponPierce);
                    writer.Write(this.weaponThrust);
                    writer.Write(this.weaponStrike);
                    writer.Write(this.targetType);
                    writer.Write(this.targetRange);
                    writer.Write(this.targetSize);
                    writer.Write(this.unknow12bytes);

                    for (var i = 0; i < Helper.MaxItemEffects; i++)
                    {
                        this.effects[i].ToByteArray(writer);
                    }

                    for (var i = 0; i < 10; i++)
                    {
                        writer.Write(this.stats[i]);
                    }

                    writer.Write(this.price);
                    writer.Write(this.stackMax);
                    writer.Write(this.rank);
                    writer.Write(ushort.MaxValue);
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.name + "\0"));
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.desc + "\0"));
                    writer.Write(this.unknow8bytes);

                    if (this.unknow8bytes.Any(singleByte => singleByte != 0))
                    {
                        writer.Write(this.unknowQuartzShort1);
                        writer.Write(this.unknowQuartzShort2);
                        writer.Write(this.unknowQuartzShort3);
                        writer.Write(ushort.MaxValue);
                        writer.Write(ushort.MaxValue);
                        writer.Write(ushort.MaxValue);
                    }

                    writer.BaseStream.Position = 0;
                    writer.Write((short)(writer.BaseStream.Length - 2));
                }

                Array.Resize(ref array, ms.ToArray().Length);
                array = ms.ToArray();
            }

            return array;
        }
    }
}
