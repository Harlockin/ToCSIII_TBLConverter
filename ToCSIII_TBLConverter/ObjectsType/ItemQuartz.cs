//-----------------------------------------------------------------------
// <copyright file="ItemQuartz.cs" company="None">
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
    /// Provide the structure and some utilities for the 'item_q' objects.
    /// </summary>
    public class ItemQuartz : Item
    {
        private short unknowQuartzShort1;
        private short unknowQuartzShort2;
        private short unknowQuartzShort3;
        private short unknowQuartzShort4;
        private short unknowQuartzShort5;
        private short unknowQuartzShort6;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemQuartz"/> class.
        /// </summary>
        public ItemQuartz() : base()
        {
            this.unknowQuartzShort1 = 0;
            this.unknowQuartzShort2 = 0;
            this.unknowQuartzShort3 = 0;
            this.unknowQuartzShort4 = 0;
            this.unknowQuartzShort5 = 0;
            this.unknowQuartzShort6 = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemQuartz"/> class.
        /// </summary>
        /// <param name="t_data">An array of byte containing an unprocessed ItemQuartz.</param>
        public ItemQuartz(byte[] t_data)
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
                
                this.unknowQuartzShort1 = br.ReadInt16();
                this.unknowQuartzShort2 = br.ReadInt16();
                this.unknowQuartzShort3 = br.ReadInt16();
                this.unknowQuartzShort4 = br.ReadInt16();
                this.unknowQuartzShort5 = br.ReadInt16();
                this.unknowQuartzShort6 = br.ReadInt16();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemQuartz"/> class.
        /// </summary>
        /// <param name="t_string">A string array containing an Item.</param>
        public ItemQuartz(string[] t_string) : base(t_string)
        {
            this.unknowQuartzShort1 = short.Parse(t_string[52]);
            this.unknowQuartzShort2 = short.Parse(t_string[53]);
            this.unknowQuartzShort3 = short.Parse(t_string[54]);
            this.unknowQuartzShort4 = short.Parse(t_string[55]);
            this.unknowQuartzShort5 = short.Parse(t_string[56]);
            this.unknowQuartzShort6 = short.Parse(t_string[57]);
        }

        /// <summary>
        /// Write the current Item to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public override string ToCSVString()
        {
            string line = base.ToCSVString();
            
            line += "," + this.unknowQuartzShort1.ToString();
            line += "," + this.unknowQuartzShort2.ToString();
            line += "," + this.unknowQuartzShort3.ToString();
            line += "," + this.unknowQuartzShort4.ToString();
            line += "," + this.unknowQuartzShort5.ToString();
            line += "," + this.unknowQuartzShort6.ToString();

            return line;
        }

        /// <summary>
        /// Write the current Item to an array of bytes.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public override byte[] ToByteArray()
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
                        writer.Write(this.unknowQuartzShort4);
                        writer.Write(this.unknowQuartzShort5);
                        writer.Write(this.unknowQuartzShort6);
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
