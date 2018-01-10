using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ToolGood.RcxTest
{
    /// <summary>
    /// RCX algorithm
    /// Author : Lin ZhiJun
    /// NickName : ToolGood
    /// Email : toolgood@qq.com
    /// 
    /// See https://github.com/toolgood/RCX
    /// </summary>
    public class RCX
    {
        private byte[] keybox;
        private const int keyLen = 256;

        public RCX(string pass)
        {
            var ps = Encoding.Unicode.GetBytes(pass);
            keybox = GetKey(ps, keyLen);
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            byte[] mBox = new byte[keyLen];
            Array.Copy(keybox, mBox, keyLen);
            byte[] output = new byte[data.Length];
            int i = 0, j = 0;
            for (int offset = 0; offset < data.Length; offset++) {
                i = (++i) & 0xFF;
                j = (j + mBox[i]) & 0xFF;

                byte a = data[offset];
                byte c = (byte)(a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
                output[offset] = c;

                byte temp2 = mBox[c];
                mBox[c] = mBox[a];
                mBox[a] = temp2;
                j = (j + a + c);
            }
            return output;
        }



        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, byte[] pass)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            byte[] mBox = GetKey(pass, keyLen);
            byte[] output = new byte[data.Length];
            int i = 0, j = 0;
            for (int offset = 0; offset < data.Length; offset++) {
                i = (++i) & 0xFF;
                j = (j + mBox[i]) & 0xFF;

                byte a = data[offset];
                byte c = (byte)(a ^ mBox[(mBox[i] + mBox[j]) & 0xFF]);
                output[offset] = c;

                byte temp2 = mBox[c];
                mBox[c] = mBox[a];
                mBox[a] = temp2;
                j = (j + a + c);
            }
            return output;
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string pass)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            return Encrypt(data, Encoding.Unicode.GetBytes(pass));
        }

        private static byte[] GetKey(byte[] pass, int kLen)
        {
            byte[] mBox = new byte[kLen];
            for (Int64 i = 0; i < kLen; i++) {
                mBox[i] = (byte)i;
            }
            Int64 j = 0;
            for (Int64 i = 0; i < kLen; i++) {
                j = (j + mBox[i] + pass[i % pass.Length]) % kLen;
                byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;
            }
            return mBox;
        }
    }
}
