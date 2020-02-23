/// <summary>
/// 类名 : MFReturnValue
/// 作者 : 朱明星
/// 说明 : 返回值类
/// 创建日期 : 2013-01-01
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    /// <summary>
    /// 返回值类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class MFReturnValue<T>
    {
        #region MFBackValue 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MFReturnValue()
        { 
        
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="value">泛型值</param>
        public MFReturnValue(T value)
            : this()
        {
            this.Value = value;
        }
        #endregion

        #region 属性

        #region Value 获取或设置返回值
        /// <summary>
        /// 获取或设置返回值
        /// </summary>
        public T Value
        {
            get;
            set;
        }
        #endregion

        #region Message 获取或设置提示信息
        /// <summary>
        /// 获取或设置提示信息
        /// </summary>
        public string Message
        {
            get;
            set;
        }
        #endregion

        #region HasError 获取或设置是否有错
        /// <summary>
        /// 获取或设置是否有错
        /// </summary>
        public bool HasError
        {
            get;
            set;
        }
        #endregion

        #region Error 获取或设置异常对象
        /// <summary>
        /// 获取或设置异常对象
        /// </summary>
        public Exception Error
        {
            get;
            set;
        }
        #endregion

        #region ReturnCode 返回代码
        /// <summary>
        /// 返回代码（一般是返回存储过程中的Return值）
        /// </summary>
        public int ReturnCode
        {
            get;
            set;
        }
        #endregion

        #region OutputValues 获取附加参数
        protected IDictionary<string, object> _outputValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 获取附加参数
        /// </summary>
        public IDictionary<string, object> OutputValues
        {
            get
            {
                return _outputValues;
            }
        }
        #endregion

        #endregion

        #region 方法

        #region GetOutputValue 获取返回值的附加参数
        /// <summary>
        /// 获取返回值的附加参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TM GetOutputValue<TM>(string key)
        {

            if (!OutputValues.IsNullOrEmpty() && OutputValues.ContainsKey(key))
            {
                return (TM)OutputValues[key];
            }
            return default(TM);
        }
        #endregion

        #region SetOutputValue 设置返回值的附加参数
        /// <summary>
        /// 设置返回值的附加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetOutputValue<TM>(string key, TM value)
        {
            OutputValues[key] = value;
        }
        #endregion

        #endregion
    }
}