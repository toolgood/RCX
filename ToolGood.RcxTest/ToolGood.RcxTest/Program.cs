using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolGood.RcxCrypto;

namespace ToolGood.RcxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding encoding = Encoding.ASCII;
            Console.WriteLine("配置说明：");
            Console.WriteLine("    编码类型(Encoding)：ASCII");
            Console.WriteLine("    输出数据类型(output data type)：Base64_Url");
            Console.WriteLine("    密钥(secret key)：ToolGood ");
            Console.WriteLine("");

            Console.WriteLine("1、测试RCX算法的正确性(Testing the correctness of the RCX algorithm)");
            Console.WriteLine("明文(original text)：hello RCX!");
            var bytes = RCX.Encrypt("hello RCX!", "ToolGood",encoding);
            Console.WriteLine("加密后数据(ecrypted data)：" + Base64.ToBase64ForUrlString(bytes));
            Console.WriteLine("解密后文本(decrypted text)：" + Encoding.UTF8.GetString(RCX.Encrypt(bytes, "ToolGood",encoding)));

            Console.WriteLine("");
            Console.WriteLine("2、测试RCX算法的变化能力(Testing the ability to change the RCX algorithm)");
            Console.WriteLine("RC4('ABCDDDDDDDDDDDDDDDDDDDDDD') => " + Base64.ToBase64ForUrlString(RC4.Encrypt("ABCDDDDDDDDDDDDDDDDDDDDDD", "ToolGood", encoding)));
            Console.WriteLine("RC4('ACBDDDDDDDDDDDDDDDDDDDDDD') => " + Base64.ToBase64ForUrlString(RC4.Encrypt("ACBDDDDDDDDDDDDDDDDDDDDDD", "ToolGood", encoding)));
            Console.WriteLine("RC4('CBADDDDDDDDDDDDDDDDDDDDDD') => " + Base64.ToBase64ForUrlString(RC4.Encrypt("CBADDDDDDDDDDDDDDDDDDDDDD", "ToolGood", encoding)));
            Console.WriteLine("RC4('1234567891234567891234567') => " + Base64.ToBase64ForUrlString(RC4.Encrypt("1234567891234567891234567", "ToolGood", encoding)));
            Console.WriteLine("RC4('1234567800034567891234567') => " + Base64.ToBase64ForUrlString(RC4.Encrypt("1234567800034567891234567", "ToolGood", encoding)));

            Console.WriteLine("");
            Console.WriteLine("RCX('ABCDDDDDDDDDDDDDDDDDDDDDD') => " + Base64.ToBase64ForUrlString(RCX.Encrypt("ABCDDDDDDDDDDDDDDDDDDDDDD", "ToolGood", encoding)));
            Console.WriteLine("RCX('ACBDDDDDDDDDDDDDDDDDDDDDD') => " + Base64.ToBase64ForUrlString(RCX.Encrypt("ACBDDDDDDDDDDDDDDDDDDDDDD", "ToolGood", encoding)));
            Console.WriteLine("RCX('CBADDDDDDDDDDDDDDDDDDDDDD') => " + Base64.ToBase64ForUrlString(RCX.Encrypt("CBADDDDDDDDDDDDDDDDDDDDDD", "ToolGood", encoding)));
            Console.WriteLine("RCX('1234567891234567891234567') => " + Base64.ToBase64ForUrlString(RCX.Encrypt("1234567891234567891234567", "ToolGood", encoding)));
            Console.WriteLine("RCX('1234567800034567891234567') => " + Base64.ToBase64ForUrlString(RCX.Encrypt("1234567800034567891234567", "ToolGood", encoding)));


            Console.WriteLine("");
            Console.WriteLine("3、测试RCX算法的性能(Testing the performance of the RCX algorithm)");
            Console.WriteLine("数据长度(data length)：10000 ");
            Console.WriteLine("加密次数(encryption count)：1000 ");
            var str = new string('a', 10000);
            var count = 1000;
            DoRC4(str, "ToolGood", count);
            DoRCX(str, "ToolGood", count);

            Console.ReadKey();
        }
        public static void DoRC4(string txt, string pass, int count)
        {
            var rc4 = new RC4(pass);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++) {
                rc4.Encrypt(txt);
            }
            stopwatch.Stop();
            Console.WriteLine("RC4  => " + stopwatch.ElapsedMilliseconds.ToString() + "ms");
        }
        public static void DoRCX(string txt, string pass, int count)
        {
            var rc4x = new RCX(pass);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < count; i++) {
                rc4x.Encrypt(txt);
            }
            stopwatch.Stop();
            Console.WriteLine("RCX => " + stopwatch.ElapsedMilliseconds.ToString() + "ms");
        }


    }
}
