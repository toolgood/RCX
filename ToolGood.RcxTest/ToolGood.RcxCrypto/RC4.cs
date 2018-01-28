using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.RcxCrypto
{
    public class RC4
    {
        public byte[] keybox;
        private const int keyLen = 256;
        private Encoding encoding;

        public RC4(string pass)
        {
            var ps = Encoding.UTF8.GetBytes(pass);
            encoding = Encoding.UTF8;
            keybox = GetKey(ps, keyLen);
        }

        public RC4(string pass, Encoding encoding)
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
            //Buffer.BlockCopy(keybox, 0, mBox, 0, keyLen);

            Array.Copy(keybox, mBox, keyLen);
            byte[] output = new byte[data.Length];
            int i = 0, j = 0;
            for (Int64 offset = 0; offset < data.Length; offset++) {
                i = (++i) & 0xFF;
                j = (j + mBox[i]) & 0xFF;
                byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;

                byte a = data[offset];
                byte b = mBox[(mBox[i] + mBox[j]) & 0xFF];
                output[offset] = (byte)((int)a ^ (int)b);
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

            return Encrypt(Encoding.UTF8.GetBytes(data),pass);
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
            for (Int64 offset = 0; offset < data.Length; offset++) {
                i = (++i) & 0xFF;
                j = (j + mBox[i]) & 0xFF;
                byte temp = mBox[i];
                mBox[i] = mBox[j];
                mBox[j] = temp;

                byte a = data[offset];
                byte b = mBox[(mBox[i] + mBox[j]) & 0xFF];
                output[offset] = (byte)((int)a ^ (int)b);
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
