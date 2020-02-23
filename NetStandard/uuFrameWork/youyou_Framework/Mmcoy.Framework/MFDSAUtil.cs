using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    /// <summary>
    /// 非对称加密
    /// </summary>
    public sealed class MFDSAUtil
    {
        #region GenerateKeys 生成密钥对
        /// <summary>
        /// 生成密钥对 key:PublicKey PrivateKey
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GenerateKeys()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            DSACryptoServiceProvider objdsa = new DSACryptoServiceProvider();

            dic["PublicKey"] = objdsa.ToXmlString(false);//公钥
            dic["PrivateKey"] = objdsa.ToXmlString(true);//私钥
            return dic;
        }
        #endregion

        #region GenerateSignature 生成签名
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="content">原文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名</returns>
        public static string GenerateSignature(string content, string privateKey)
        {
            DSACryptoServiceProvider objdsa = new DSACryptoServiceProvider();
            objdsa.FromXmlString(privateKey);
            byte[] source = System.Text.UTF8Encoding.UTF8.GetBytes(content);
            //数字签名 
            return BitConverter.ToString(objdsa.SignData(source));
        }
        #endregion

        #region VerifySignature 验证签名
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="signature">签名</param>
        /// <returns>是否正确</returns>
        public static bool VerifySignature(string content, string publicKey, string signature)
        {
            DSACryptoServiceProvider objdsa = new DSACryptoServiceProvider();
            byte[] fileHashValue = new SHA1CryptoServiceProvider().ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(content));
            string[] strSplit = signature.Split('-');
            byte[] SignedHash = new byte[strSplit.Length];
            for (int i = 0; i < strSplit.Length; i++)
                SignedHash[i] = byte.Parse(strSplit[i], System.Globalization.NumberStyles.AllowHexSpecifier);
            objdsa.FromXmlString(publicKey);
            return objdsa.VerifySignature(fileHashValue, SignedHash);
        }
        #endregion

        #region GetTimestamp 获取时间戳
        /// <summary>
        /// 获取时间戳 定义为从格林尼治时间1970年01月01日00时00分00秒起至现在的总秒数
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
        #endregion
    }
}