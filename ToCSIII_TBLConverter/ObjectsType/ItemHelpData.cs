//-----------------------------------------------------------------------
// <copyright file="ItemHelpData.cs" company="None">
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
    /// Provide the structure and some utilities for the 'ItemHelpData' objects.
    /// </summary>
    public class ItemHelpData : ToCSIII_TBLConverter.Object
    {
        private byte[] unknows;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHelpData"/> class.
        /// </summary>
        public ItemHelpData() : base()
        {
            this.unknows = new byte[9];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHelpData"/> class.
        /// </summary>
        /// <param name="t_data">An array of byte containing an unprocessed HelpData object.</param>
        public ItemHelpData(byte[] t_data)
        {
            this.unknows = new byte[9];

            using (BinaryReader br = new BinaryReader(new MemoryStream(t_data)))
            {
                this.id = br.ReadInt16();
                this.desc = Helper.ReadNullTerminatedString(br);
                this.unknows = br.ReadBytes(9);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHelpData"/> class.
        /// </summary>
        /// <param name="t_string">A string array containing an HelpData object.</param>
        public ItemHelpData(string[] t_string) : base(t_string)
        {
            this.unknows = new byte[9];

            this.unknows = Helper.StringToByteArray(t_string[3].Replace(" ", string.Empty));
        }

        /// <summary>
        /// Write the current Item to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public override string ToCSVString()
        {
            string line = string.Empty;

            line += "," + this.id.ToString();
            line += "," + '"' + this.desc.Replace("\n", "\\n") + '"';
            line += "," + Helper.ByteArrayToString(this.unknows);

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
                    writer.Write(UTF8Encoding.UTF8.GetBytes(this.desc + "\0"));
                    writer.Write(this.unknows);

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
