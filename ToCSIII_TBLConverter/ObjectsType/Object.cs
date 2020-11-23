//-----------------------------------------------------------------------
// <copyright file="Object.cs" company="None">
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
    /// Provide the structure and some utilities for the 'HelpData' objects.
    /// </summary>
    public class Object
    {
        protected short id;
        protected string desc;

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/> class.
        /// </summary>
        public Object()
        {
            this.id = 0;
            this.desc = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/> class.
        /// </summary>
        /// <param name="t_data">An array of byte containing an unprocessed HelpData object.</param>
        public Object(byte[] t_data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(t_data)))
            {
                this.id = br.ReadInt16();
                this.desc = Helper.ReadNullTerminatedString(br);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Object"/> class.
        /// </summary>
        /// <param name="t_string">A string array containing an HelpData object.</param>
        public Object(string[] t_string)
        {
            this.id = short.Parse(t_string[1]);
            this.desc = t_string[2].Replace("\\n", "\n");
        }

        /// <summary>
        /// Write the current Item to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public virtual string ToCSVString()
        {
            string line = string.Empty;

            line += "," + this.id.ToString();
            line += "," + '"' + this.desc.Replace("\n", "\\n") + '"';

            return line;
        }

        /// <summary>
        /// Write the current Item to an array of bytes.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public virtual byte[] ToByteArray()
        {
            byte[] array = new byte[23947];

            using (var ms = new MemoryStream())
            {
                using (var writer = new BinaryWriter(ms))
                {
                    writer.Write(short.MinValue);
                    writer.Write(this.id);
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
