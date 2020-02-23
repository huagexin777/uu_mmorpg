using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Mmcoy.Framework
{
    /// <summary>
    /// RSA 非对称加密
    /// </summary>
    public sealed class MFRSAUtil
    {
        #region GenerateKeys 生成密钥对
        /// <summary>
        /// 生成密钥对 key:PublicKey PrivateKey
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GenerateKeys()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            RSACryptoServiceProvider objdsa = new RSACryptoServiceProvider();

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
            try
            {
                RSACryptoServiceProvider oRSA3 = new RSACryptoServiceProvider();
                oRSA3.FromXmlString(privateKey);
                byte[] messagebytes = Encoding.UTF8.GetBytes(content);
                byte[] AOutput = oRSA3.SignData(messagebytes, "SHA1");
                return Convert.ToBase64String(AOutput);
            }
            catch {
                return string.Empty;
            }
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
            try
            {
                string[] strSplit = signature.Split('-');
                byte[] SignedHash = Convert.FromBase64String(signature);

                RSACryptoServiceProvider oRSA4 = new RSACryptoServiceProvider();
                oRSA4.FromXmlString(publicKey);
                return oRSA4.VerifyData(Encoding.UTF8.GetBytes(content), "SHA1", SignedHash);
            }
            catch {
                return false;
            }
        }
        #endregion
    }
}