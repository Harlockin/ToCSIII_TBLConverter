//-----------------------------------------------------------------------
// <copyright file="GenericObject.cs" company="None">
//     No copyright
// </copyright>
// <author>Sébastien R. (a.k.a. Harlockin)</author>
//-----------------------------------------------------------------------
namespace ToCSIII_TBLConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Provide the common structure and some utilities for the diverse objects in '.TBL' files.
    /// </summary>
    public class GenericObject
    {
        private byte[] theData;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObject"/> class.
        /// </summary>
        /// <param name="t_data">An array of byte containing an unprocessed Item.</param>
        public GenericObject(byte[] t_data)
        {
            this.theData = t_data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericObject"/> class.
        /// </summary>
        /// <param name="t_data">A string array containing an Item.</param>
        public GenericObject(string[] t_data)
        {
            this.theData = Helper.StringToByteArray(t_data[1].Replace(" ", string.Empty));
        }

        /// <summary>
        /// Write the current GenericObject to a CSV formatted string.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public string ToCSVString()
        {
            string line = string.Empty;
            line += "," + Helper.ByteArrayToString(this.theData);
            return line;
        }

        /// <summary>
        /// Write the current GenericObject to an array of bytes.
        /// </summary>
        /// <returns>The CSV formatted string.</returns>
        public byte[] ToByteArray()
        {
            return this.theData;
        }
    }
}
