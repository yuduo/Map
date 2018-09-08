using Newtonsoft.Json.Linq;
using System;
using System.Data;

namespace HongLi.MapControl.Util
{
    /// <summary>
    /// 拓展方法集
    /// </summary>
    public static class ExtensionUtil
    {
        public static int ToInt32(this object str)
        {
            return str.ToString().ToInt32();
        }

        public static int ToInt32(this string str)
        {
            if (str.Trim() == "")
            {
                return 0;
            }
            return Convert.ToInt32(str);
        }

        public static long ToInt64(this object str)
        {
            return str.ToString().ToInt64();
        }

        public static long ToInt64(this string str)
        {
            if (str.Trim() == "")
            {
                return 0;
            }
            return Convert.ToInt64(str);
        }

        public static double ToDouble(this string str)
        {
            if (str.Trim() == "")
            {
                return 0;
            }
            return Convert.ToDouble(str);
        }

        public static double ToDouble(this object obj)
        {
            return obj.ToString().ToDouble();
        }

        /// <summary> 
        /// 字符串转16进制字节数组 
        /// </summary> 
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        public static byte[] ToHexByte(this string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            var returnBytes = new byte[hexString.Length / 2];
            for (var i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string ToHexStr(this byte[] bytes)
        {
            var returnStr = "";
            if (bytes == null) return returnStr;
            for (var i = 0; i < bytes.Length; i++)
            {
                returnStr += i.ToString("X2");
            }
            return returnStr;
        }

        /// <summary> 
        /// 从汉字转换到16进制 
        /// </summary> 
        /// <param name="s"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <param name="split">是否每字符用逗号分隔</param> 
        /// <returns></returns> 
        public static string ToHex(this string s, string charset, bool split)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格 
                         //throw new ArgumentException("s is not valid chinese string!"); 
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                str += $"{bytes[i]:X}";
                if (split && (i != bytes.Length - 1))
                {
                    str += ",";
                }
            }
            return str.ToLower();
        }

        ///<summary> 
        /// 从16进制转换成汉字 
        /// </summary> 
        /// <param name="hex"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <returns></returns> 
        public static string UnHex(this string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException(nameof(hex));
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格 
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", nameof(hex));
                }
            }
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }

        public static JArray ToJArray(this DataTable dt)
        {
            var arr = new JArray();
            if (dt == null || dt.Rows.Count < 1)
            {
                return arr;
            }
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var item = new JObject();
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    item.Add(new JProperty(dt.Columns[j].ColumnName, dt.Rows[i][j].ToString()));
                }
                arr.Add(item);
            }
            return arr;
        }

        public static JObject ToJObject(this DataRow dr)
        {
            var item = new JObject();
            if (dr == null || dr.Table.Columns.Count < 1)
            {
                return item;
            }
            for (var i = 0; i < dr.Table.Columns.Count; i++)
            {
                item.Add(new JProperty(dr.Table.Columns[i].ColumnName, dr.Table.Rows[0][i].ToString()));
            }
            return item;
        }
    }
}
