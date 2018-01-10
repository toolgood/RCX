 

RCX算法基于RC4算法改进，性能相差不太。

The RCX algorithm is improved based on the RC4 algorithm, and performance is almost the same.
 
#### RCX算法与RC4算法比较 （Comparison between RCX algorithm and RC4 algorithm ）
密钥(secret key)：ToolGood  输出数据类型(output data type)：Base64_Url
```` csharp
RC4('ABCDDDDDDDDDDDDDDDDDDDDDD') => O8AF0I3sAzyQaTO78S9irZwDfemUR4eGsw

RC4('ACBDDDDDDDDDDDDDDDDDDDDDD') => O8EE0I3sAzyQaTO78S9irZwDfemUR4eGsw

RC4('CBADDDDDDDDDDDDDDDDDDDDDD') => OcAH0I3sAzyQaTO78S9irZwDfemUR4eGsw

RC4('1234567891234567891234567') => S7B1oPyecEDtHEXMgV4Q3uB-CJ_jN_b0wA

RC4('1234567800034567891234567') => S7B1oPyecEDkHUfMgV4Q3uB-CJ_jN_b0wA

 

RCX('ABCDDDDDDDDDDDDDDDDDDDDDD') => O3priO83Pd4e-7IeTBJmrIax7kmO5yzr2Q

RCX('ACBDDDDDDDDDDDDDDDDDDDDDD') => O3s81pEyp9daRW9yHYC4ynIOalk8FYSI9g

RCX('CBADDDDDDDDDDDDDDDDDDDDDD') => OXpp1Sm4eyyhg5MQGWrjGa6w2MZhoK09Kw

RCX('1234567891234567891234567') => SwoWZFa8uiJnqv_arFs0WVHOfYRvMGTsAw

RCX('1234567800034567891234567') => SwoWZFa8uiJuQXjGVkKaspQseRHK9qtVZA
`````

从上面的代码，可以明显看出RC4算法加密的缺点，而数据经过RCX算法加密后变得无序。

From the above code, you can clearly see the shortcomings of RC4 algorithm encryption,But the data is encrypted by the RCX algorithm and becomes disordered. 

#### 核心代码（Core code ）
RCX算法采用[明文]与[密文]对密码盘进行调换，并且对 j 进行修改: j=j+[明文]+[密文] 。

The RCX algorithm uses [plain text] and [ciphertext] to swap the cipher disk, and modify j: j = j + [plain text] + [ciphertext].
```` csharp
    public class RCX
    {
         private const int keyLen = 256;
 
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
`````