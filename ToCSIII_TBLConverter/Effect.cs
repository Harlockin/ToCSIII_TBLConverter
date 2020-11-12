//-----------------------------------------------------------------------
// <copyright file="Effect.cs" company="None">
//     No copyright
// </copyright>
// <author>Sébastien R. (a.k.a. Harlockin)</author>
//-----------------------------------------------------------------------
namespace ToCSIII_TBLConverter
{
    using System.IO;

    /// <summary>
    /// Represent a structure containing the ID of an effect and its 3 values.
    /// </summary>
    public class Effect
    {
        private short effectId;
        private int[] effectParams;

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class.
        /// </summary>
        /// <param name="t_binaryReader">A System.IO.BinaryReader object.</param>
        public Effect(BinaryReader t_binaryReader)
        {
            this.effectId = t_binaryReader.ReadInt16();
            this.effectParams = new int[Helper.MaxItemEffectValues];

            for (var i = 0; i < Helper.MaxItemEffectValues; i++)
            {
                this.effectParams[i] = t_binaryReader.ReadInt32();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Effect"/> class.
        /// </summary>
        /// <param name="t_effectID">The ID of the effect.</param>
        /// <param name="t_param1">The first parameter.</param>
        /// <param name="t_param2">The second parameter.</param>
        /// <param name="t_param3">The third parameter.</param>
        public Effect(short t_effectID, int t_param1, int t_param2, int t_param3)
        {
            this.effectId = t_effectID;
            this.effectParams = new int[Helper.MaxItemEffectValues];

            this.effectParams[0] = t_param1;
            this.effectParams[1] = t_param2;
            this.effectParams[2] = t_param3;
        }

        /// <summary>
        /// Write the current Effect to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public string ToCSVString()
        {
            string line = string.Empty;

            line += this.effectId.ToString();

            for (var i = 0; i < Helper.MaxItemEffectValues; i++)
            {
                line += "," + this.effectParams[i].ToString();
            }

            return line;
        }

        /// <summary>
        /// Write the current Effect to a byte array.
        /// </summary>
        /// <param name="t_writer">a BinaryWriter object.</param>
        public void ToByteArray(BinaryWriter t_writer)
        {
            t_writer.Write(this.effectId);

            for (var i = 0; i < Helper.MaxItemEffectValues; i++)
            {
                t_writer.Write(this.effectParams[i]);
            }
        }
    }
}
