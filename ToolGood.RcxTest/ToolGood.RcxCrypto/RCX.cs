using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ToolGood.RcxCrypto
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
        private Encoding encoding;


        public RCX(string pass)
        {
            var ps = Encoding.UTF8.GetBytes(pass);
            encoding = Encoding.UTF8;
            keybox = GetKey(ps, keyLen);
        }

        public RCX(string pass, Encoding encoding)
        {
            var ps = encoding.GetBytes(pass);
            this.encoding = encoding;
            keybox = GetKey(ps, keyLen);
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Encrypt(string data)
        {
            return Encrypt(encoding.GetBytes(data));
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] Encrypt(string data, Encoding encoding)
        {
            return Encrypt(encoding.GetBytes(data));
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
            //Buffer.BlockCopy(keybox, 0, mBox, 0, keyLen);
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
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, string pass, Encoding encoding)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            return Encrypt(encoding.GetBytes(data), encoding.GetBytes(pass));
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, string pass)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            return Encrypt(Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(pass));
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, string pass, Encoding encoding)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            return Encrypt(data, encoding.GetBytes(pass));
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

            return Encrypt(data, Encoding.UTF8.GetBytes(pass));
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, byte[] pass)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            return Encrypt(Encoding.UTF8.GetBytes(data), pass);
        }
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pass"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string data, byte[] pass, Encoding encoding)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (pass == null) throw new ArgumentNullException("pass");

            return Encrypt(encoding.GetBytes(data), pass);
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
