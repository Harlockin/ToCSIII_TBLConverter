//-----------------------------------------------------------------------
// <copyright file="Magic.cs" company="None">
//     No copyright
// </copyright>
// <author>Sébastien R. (a.k.a. Harlockin)</author>
//-----------------------------------------------------------------------
namespace ToCSIII_TBLConverter
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Provide the structure and some utilities for the 'magic' objects.
    /// </summary>
    public class Magic : ToCSIII_TBLConverter.Object
    {
        private short characterRestriction;
        private string flags;
        private byte category;
        private byte type;
        private byte element;
        private byte switchable;
        private byte targetType;
        private float targetRange;
        private byte targetSize;
        private short unknowShort1;
        private short unknowShort2;
        private float unknowFloat1;
        private float unknowFloat2;
        private Effect[] effects;
        private byte castTime;
        private byte delay;
        private short cost;
        private byte unbalance;
        private short breakValue;
        private byte level;
        private byte order;
        private byte unknowByte1;
        private string animation;
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Magic"/> class.
        /// </summary>
        public Magic() : base()
        {
            this.characterRestriction = 0;
            this.flags = string.Empty;
            this.category = 0;
            this.type = 0;
            this.element = 0;
            this.switchable = 0;
            this.targetType = 0;
            this.targetRange = 0.0f;
            this.targetSize = 0;
            this.unknowShort1 = 0;
            this.unknowShort2 = 0;
            this.unknowFloat1 = 0.0f;
            this.unknowFloat2 = 0.0f;
            this.effects = new Effect[Helper.MaxItemEffects];
            this.castTime = 0;
            this.delay = 0;
            this.cost = 0;
            this.unbalance = 0;
            this.breakValue = 0;
            this.level = 0;
            this.order = 0;
            this.unknowByte1 = 0;
            this.animation = string.Empty;
            this.name = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Magic"/> class.
        /// </summary>
        /// <param name="t_data">An array of byte containing an unprocessed Magic.</param>
        public Magic(byte[] t_data)
        {
            this.effects = new Effect[Helper.MaxItemEffects];

            using (BinaryReader br = new BinaryReader(new MemoryStream(t_data)))
            {
                this.id = br.ReadInt16();
                this.characterRestriction = br.ReadInt16();
                this.flags = Helper.ReadNullTerminatedString(br);
                this.category = br.ReadByte();
                this.type = br.ReadByte();
                this.element = br.ReadByte();
                this.switchable = br.ReadByte();
                this.targetType = br.ReadByte();
                this.targetRange = br.ReadSingle();
                this.targetSize = br.ReadByte();
                this.unknowShort1 = br.ReadInt16();
                this.unknowShort2 = br.ReadInt16();
                this.unknowFloat1 = br.ReadSingle();
                this.unknowFloat2 = br.ReadSingle();

                for (var i = 0; i < Helper.MaxItemEffects; i++)
                {
                    this.effects[i] = new Effect(br);
                }

                this.castTime = br.ReadByte();
                this.delay = br.ReadByte();
                this.cost = br.ReadInt16();
                this.unbalance = br.ReadByte();
                this.breakValue = br.ReadInt16();
                this.level = br.ReadByte();
                this.order = br.ReadByte();
                this.unknowByte1 = br.ReadByte();

                this.animation = Helper.ReadNullTerminatedString(br);
                this.name = Helper.ReadNullTerminatedString(br);
                this.desc = Helper.ReadNullTerminatedString(br);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Magic"/> class.
        /// </summary>
        /// <param name="t_string">A string array containing an Magic.</param>
        public Magic(string[] t_string)
        {
            this.effects = new Effect[Helper.MaxItemEffects];

            this.id = short.Parse(t_string[1]);
            this.characterRestriction = short.Parse(t_string[2]);
            this.flags = t_string[3];
            this.category = byte.Parse(t_string[4]);
            this.type = byte.Parse(t_string[5]);
            this.element = byte.Parse(t_string[6]);
            this.switchable = byte.Parse(t_string[7]);
            this.targetType = byte.Parse(t_string[8]);
            this.targetRange = float.Parse(t_string[9]);
            this.targetSize = byte.Parse(t_string[10]);
            this.unknowShort1 = short.Parse(t_string[11]);
            this.unknowShort2 = short.Parse(t_string[12]);
            this.unknowFloat1 = float.Parse(t_string[13]);
            this.unknowFloat2 = float.Parse(t_string[14]);

            for (var i = 0; i < Helper.MaxItemEffects; i++)
            {
                this.effects[i] = new Effect(short.Parse(t_string[15 + (4 * i)]), int.Parse(t_string[16 + (4 * i)]), int.Parse(t_string[17 + (4 * i)]), int.Parse(t_string[18 + (4 * i)]));
            }

            // 35
            this.castTime = byte.Parse(t_string[35]);
            this.delay = byte.Parse(t_string[36]);
            this.cost = short.Parse(t_string[37]);
            this.unbalance = byte.Parse(t_string[38]);
            this.breakValue = short.Parse(t_string[39]);
            this.level = byte.Parse(t_string[40]);
            this.order = byte.Parse(t_string[41]);
            this.unknowByte1 = byte.Parse(t_string[42]);

            this.animation = t_string[43].Replace("\\n", "\n");
            this.name = t_string[44].Replace("\\n", "\n");
            this.desc = t_string[45].Replace("\\n", "\n");
        }

        /// <summary>
        /// Write the current Magic to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public override string ToCSVString()
        {
            string line = string.Empty;

            line += "," + this.id.ToString();
            line += "," + this.characterRestriction.ToString();
            line += "," + '"' + this.flags + '"';
            line += "," + this.category.ToString();
            line += "," + this.type.ToString();
            line += "," + this.element.ToString();
            line += "," + this.switchable.ToString();
            line += "," + this.targetType.ToString();
            line += "," + this.targetRange.ToString();
            line += "," + this.targetSize.ToString();
            line += "," + this.unknowShort1.ToString();
            line += "," + this.unknowShort2.ToString();
            line += "," + this.unknowFloat1.ToString();
            line += "," + this.unknowFloat2.ToString();

            for (var i = 0; i < Helper.MaxItemEffects; i++)
            {
                line += "," + this.effects[i].ToCSVString();
            }

            line += "," + this.castTime.ToString();
            line += "," + this.delay.ToString();
            line += "," + this.cost.ToString();
            line += "," + this.unbalance.ToString();
            line += "," + this.breakValue.ToString();
            line += "," + this.level.ToString();
            line += "," + this.order.ToString();
            line += "," + this.unknowByte1.ToString();

            line += "," + '"' + this.animation + '"';
            line += "," + '"' + this.name + '"';
            line += "," + '"' + this.desc.Replace("\n", "\\n") + '"';

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
                    writer.Write(this.type);
                    writer.Write(this.element);
                    writer.Write(this.switchable);
                    writer.Write(this.targetType);
                    writer.Write(this.targetRange);
                    writer.Write(this.targetSize);
                    writer.Write(this.unknowShort1);
                    writer.Write(this.unknowShort2);
                    writer.Write(this.unknowFloat1);
                    writer.Write(this.unknowFloat2);

                    for (var i = 0; i < Helper.MaxItemEffects; i++)
                    {
                        this.effects[i].ToByteArray(writer);
                    }

                    writer.Write(this.castTime);
                    writer.Write(this.delay);
                    writer.Write(this.cost);
                    writer.Write(this.unbalance);
                    writer.Write(this.breakValue);
                    writer.Write(this.level);
                    writer.Write(this.order);
                    writer.Write(this.unknowByte1);
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.animation + "\0"));
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.name + "\0"));
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.desc + "\0"));

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
