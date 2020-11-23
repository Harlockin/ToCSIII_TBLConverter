//-----------------------------------------------------------------------
// <copyright file="Helper.cs" company="None">
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

    /// <summary>
    /// Provide helping function and variables.
    /// </summary>
    public static class Helper
    {
        public const int MaxItemEffects = 5;

        public const int MaxItemEffectValues = 3;

        /// <summary>
        /// Read a null terminated string from a binary stream.
        /// </summary>
        /// <param name="t_binary_reader">A BinaryReader object.</param>
        /// <returns>The string.</returns>
        public static string ReadNullTerminatedString(BinaryReader t_binary_reader)
        {
            List<byte> list = new List<byte>();

            byte ch = t_binary_reader.ReadByte();
            while (ch != 0x00)
            {
                list.Add(ch);
                ch = t_binary_reader.ReadByte();
            }

            string output = Encoding.UTF8.GetString(list.ToArray());
            return output;
        }

        /// <summary>
        /// Transform an array of byte to a hexadecimal string representation.
        /// </summary>
        /// <param name="t_byte_array">An array of bytes containing some data.</param>
        /// <returns>The string.</returns>
        public static string ByteArrayToString(byte[] t_byte_array)
        {
            return BitConverter.ToString(t_byte_array).Replace("-", " ");
        }

        /// <summary>
        /// Transform an hexadecimal string representation to an array of byte.
        /// </summary>
        /// <param name="t_hex_string">An hexadecimal string representation.</param>
        /// <returns>The string.</returns>
        public static byte[] StringToByteArray(string t_hex_string)
        {
            int numberChars = t_hex_string.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(t_hex_string.Substring(i, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// Generate the header for unprocessed objects in a CSV formatted string.
        /// </summary>
        /// <returns>The header.</returns>
        public static string GetObjectHeader()
        {
            string header = "Type,Unknows";

            return header;
        }

        /// <summary>
        /// Generate the item and item_q header in a CSV formatted string.
        /// </summary>
        /// <returns>The header.</returns>
        public static string GetItemHeader()
        {
            string header = "Type,ID,Name,Desc,Category,Price,MaxQty" +
                            ",Effect1,E1 Param1,E1 Param2,E1 Param3" +
                            ",Effect2,E2 Param1,E2 Param2,E2 Param3" +
                            ",Effect3,E3 Param1,E3 Param2,E3 Param3" +
                            ",Effect4,E4 Param1,E4 Param2,E4 Param3" +
                            ",Effect5,E5 Param1,E5 Param2,E5 Param3" +
                            ",STR,DEF,ATS,ADF,ACC,EVA,SPD,MOV,HP,EP" +
                            ",Rank,Element,WeaponSwitch,WeaponSlash" +
                            ",WeaponPierce,WeaponThrust,WeaponStrike" +
                            ",T.Type,T.Range,T.Size,Char,Flags" +
                            ",Unknows";

            return header;
        }

        /// <summary>
        /// Generate the header for Item and Comp HelpData in a CSV formatted string.
        /// </summary>
        /// <returns>The header.</returns>
        public static string GetHelpDataHeader()
        {
            string header = "Type,ID,Desc,Unknows";

            return header;
        }

        /// <summary>
        /// Generate the magic and magic header in a CSV formatted string.
        /// </summary>
        /// <returns>The header.</returns>
        public static string GetMagicHeader()
        {
            string header = "Type,ID,Char,Flags,Category,Type,Element" +
                            ",WeaponSwitch,T.Type,T.Range,T.Size,Unknow,,Unknow,Unknow," +
                            ",Effect1,E1 Param1,E1 Param2,E1 Param3" +
                            ",Effect2,E2 Param1,E2 Param2,E2 Param3" +
                            ",Effect3,E3 Param1,E3 Param2,E3 Param3" +
                            ",Effect4,E4 Param1,E4 Param2,E4 Param3" +
                            ",Effect5,E5 Param1,E5 Param2,E5 Param3" +
                            ",Cast,Delay,Cost,Unbalance,Break,Level," +
                            ",Order,Unknow,Animation,Name,Desc";

            return header;
        }
    }
}
