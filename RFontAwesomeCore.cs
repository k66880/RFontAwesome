using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RFontAwesome
{
    public class RFontAwesomeCore
    {
        private static PrivateFontCollection pfc = new PrivateFontCollection();
        
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(byte[] fontdata)
        {
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontdata.Length);
            Marshal.Copy(fontdata, 0, fontPtr, fontdata.Length);
            pfc.AddMemoryFont(fontPtr, fontdata.Length);
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        public static Font GetFont(float emSize)
        {
            if (pfc.Families.Length <= 0)
                return new Font("FontAwesome", emSize);
            return new Font(pfc.Families[0], emSize);
        }

        /// <summary>
        /// 获取指定图标枚举对应的Unicode字符
        /// </summary>
        public static string GetString(RFontAwesomeEnum key)
        {
            var s = GetDescription(key);
            if (s.StartsWith("0x"))
            {
                int n = Convert.ToInt32(s, 16);
                byte[] intBuff = BitConverter.GetBytes(n);
                var c = Encoding.Unicode.GetChars(intBuff);
                return new string(c);
            }
            return "";
        }

        /// <summary>
        /// 获取指定图标枚举对应描述属性
        /// </summary>
        public static string GetDescription(RFontAwesomeEnum enumName)
        {
            var description = "";
            var fieldInfo = enumName.GetType().GetField(enumName.ToString());
            foreach (var attr in fieldInfo.CustomAttributes)
            {
                if (attr.AttributeType == typeof(DescriptionAttribute))
                {
                    var arg = attr.ConstructorArguments[0];
                    return arg.Value.ToString();
                }
            }
            return description;
        }
    }
}
