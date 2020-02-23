using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Mmcoy.Framework
{
    public class MFSerializationUtil
    {
        /// <summary>
        /// 序列化 对象到字符串
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return Convert.ToBase64String(buffer);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 反序列化 字符串到对象
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <param name="str">要转换为对象的字符串</param>
        /// <returns>反序列化出来的对象</returns>
        public static T Deserialize<T>(string str)
        {
            T obj = default(T);
            if (string.IsNullOrEmpty(str))
                return default(T);

            try
            {
                IFormatter formatter = new BinaryFormatter();
                byte[] buffer = Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(buffer);
                obj = (T)formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
            }
            catch
            {
                obj = default(T);
            }
            return obj;
        }

        /// <summary>
        /// 将对象序列化为JSON
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>json串</returns>
        public static string SerializeToJson(object obj)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            json.MaxJsonLength = Int32.MaxValue;
            return json.Serialize(obj);
        }

        /// <summary>
        /// 将JSON反序列化为对象
        /// </summary>
        /// <typeparam name="T">反序列后的数据类型</typeparam>
        /// <param name="input">JSON串</param>
        /// <returns>对象</returns>
        public static T DeserializeJson<T>(string input)
        {
            try
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                return json.Deserialize<T>(input);
            }
            catch { return default(T); }
        }


        /// <summary>
        /// 将XML反序列化为对象
        /// </summary>
        /// <typeparam name="T">反序列后的数据类型</typeparam>
        /// <param name="xmlString">XML文本</param>
        /// <returns></returns>
        public static T DeserializeXml<T>(string xmlString)
        {
            return DeserializeXml<T>(xmlString, Encoding.Default);
        }

        /// <summary>
        /// 将XML反序列化为对象
        /// </summary>
        /// <typeparam name="T">反序列后的数据类型</typeparam>
        /// <param name="xmlString">XML文本</param>
        /// <param name="encoding">字符集编码</param>
        /// <returns></returns>
        public static T DeserializeXml<T>(string xmlString, Encoding encoding)
        {
            MemoryStream ms = new MemoryStream(encoding.GetBytes(xmlString));
            return (T)new XmlSerializer(typeof(T)).Deserialize(ms);
        }
    }
}