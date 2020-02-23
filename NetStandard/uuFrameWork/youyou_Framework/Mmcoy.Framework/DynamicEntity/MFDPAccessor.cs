using Mmcoy.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework
{
    public class MFDPAccessor
    {
        #region GetAccessor
        private static object m_mutex = new object();
        private static Dictionary<Type, Dictionary<string, MFDynamicProperty>> m_cache =
            new Dictionary<Type, Dictionary<string, MFDynamicProperty>>();

        private static MFDynamicProperty GetAccessor(Type type, string propertyName)
        {
            MFDynamicProperty accessor = null;
            Dictionary<string, MFDynamicProperty> typeCache;

            propertyName = propertyName.ToLower();
            if (m_cache.TryGetValue(type, out typeCache))
            {
                if (typeCache.TryGetValue(propertyName, out accessor))
                {
                    return accessor;
                }
            }

            lock (m_mutex)
            {
                if (!m_cache.ContainsKey(type))
                {
                    m_cache[type] = new Dictionary<string, MFDynamicProperty>(StringComparer.OrdinalIgnoreCase);
                }

                var proInfo = type.GetProperties().Where(p => p.Name.ToLower() == propertyName).FirstOrDefault();
                if (proInfo != null)
                {
                    accessor = new MFDynamicProperty(proInfo);
                    m_cache[type][propertyName] = accessor;
                }

                return accessor;
            }
        }
        #endregion

        #region GetEntity 从字典中加载数据到实体对象
        /// <summary>
        /// 从字典中加载数据到实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueDict"></param>
        /// <param name="entity"></param>
        /// <param name="extName"></param>
        /// <returns></returns>
        public static T GetEntity<T>(IDictionary<string, object> valueDict, T entity, string extName = "")
        {
            var type = entity.GetType();
            var column = string.Empty;
            var value = new object();
            var colName = string.Empty;
            for (var i = 0; i < valueDict.Count; i++)
            {
                column = valueDict.Keys.ElementAt(i).ToLower();
                colName = column;
                if (!string.IsNullOrEmpty(extName))
                    colName = column.Replace(extName, "");
                var property = GetAccessor(type, colName.Split('_')[0]);
                if (property != null && property.Property != null)
                {
                    var pType = property.Property.PropertyType;
                    if (pType.IsClass && !pType.IsValueType && !pType.IsArray && !pType.IsEnum && pType.Name != "String")
                    {
                        //extName = string.Format("{0}{1}_", extName, property.Property.Name);
                        value = GetEntity(valueDict, GenerateEntity(pType), string.Format("{0}{1}_", extName, property.Property.Name).ToLower());
                    }
                    else
                    {
                        value = valueDict[column];
                    }
                    if (value != null && !(value is DBNull))
                        property.SetValue(entity, value);
                    if (valueDict.Count - i > 0)
                    {
                        valueDict.Remove(column);
                        i--;
                    }
                }
            }
            return entity;
        }
        #endregion

        #region GetEntity 从DataReader中加载数据到实体对象
        /// <summary>
        /// 从DataReader中加载数据到实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        /// <param name="extName"></param>
        /// <param name="keyList"></param>
        /// <returns></returns>
        public static T GetEntity<T>(IDataReader reader, T entity, string extName = "", IList<string> keyList = null)
        {
            var type = entity.GetType();
            var column = string.Empty;
            var value = new object();
            var colName = string.Empty;
            if (keyList == null)
            {
                keyList = new List<string>();
                foreach (DataRow row in reader.GetSchemaTable().Rows)
                    keyList.Add(row.Field<string>(0).ToLower());
            }
            for (var i = 0; i < keyList.Count; i++)
            {
                column = keyList[i];
                colName = column;
                if (!string.IsNullOrEmpty(extName))
                    colName = column.Replace(extName, "");
                var property = GetAccessor(type, colName.Split('_')[0]);
                if (property != null && property.Property != null)
                {
                    var pType = property.Property.PropertyType;
                    if (pType.IsClass && !pType.IsValueType && !pType.IsArray && !pType.IsEnum && pType.Name != "String")
                    {
                        //extName = string.Format("{0}{1}_", extName, property.Property.Name);
                        value = GetEntity(reader, GenerateEntity(pType), string.Format("{0}{1}_", extName, property.Property.Name).ToLower(), keyList);
                    }
                    else
                    {
                        value = reader[column];
                    }
                    if (value != null && !(value is DBNull))
                        property.SetValue(entity, value);

                    if (keyList.Count - i > 0)
                    {
                        keyList.Remove(column);
                        i--;
                    }
                }
            }
            return entity;
        }
        #endregion

        #region EntityToSqlParameters
        public static SqlParameter[] EntityToSqlParameters<T>(T entity, IList<SqlParameter> paramList = null, IList<string> paramNames = null, string extName = "")
        {
            var etype = entity.GetType();
            var key = string.Empty;
            var colName = string.Empty;
            if (paramList == null)
                paramList = new List<SqlParameter>();
            if (paramNames != null)
            {
                for (int i = 0; i < paramNames.Count; i++)
                {
                    key = paramNames[i].ToLower();
                    colName = key;
                    if (!string.IsNullOrEmpty(extName))
                        colName = key.Replace(extName, "");
                    var property = GetAccessor(etype, colName.Split('_')[0]);
                    if (property != null && property.Property != null)
                    {
                        var pType = property.Property.PropertyType;
                        if (pType.IsClass && !pType.IsValueType && !pType.IsArray && !pType.IsEnum && pType.Name != "String")
                        {
                            //extName = string.Format("{0}{1}_", extName, property.Property.Name);
                            EntityToSqlParameters(property.GetValue(entity), paramList, paramNames, string.Format("{0}{1}_", extName, property.Property.Name).ToLower());
                        }
                        else
                        {
                            var value = property.GetValue(entity);
                            if (value != null && !(value is DBNull))
                                paramList.Add(new SqlParameter("@" + key, value));
                        }
                        if (paramNames.Count - i > 0)
                        {
                            paramNames.Remove(key);
                            i--;
                        }
                    }
                }
            }
            else
            {
                Dictionary<string, MFDynamicProperty> propertys = null;
                if (!m_cache.ContainsKey(etype))
                {
                    var propertyList = etype.GetProperties();
                    foreach (var p in propertyList)
                        GetAccessor(etype, p.Name.ToLower());
                }

                if (!m_cache.ContainsKey(etype))
                    throw new Exception("将对象转换成数据库参数时出现异常!");

                propertys = m_cache[etype];
                for (int i = 0; i < propertys.Count; i++)
                {
                    key = propertys.Keys.ElementAt(i).ToLower();
                    colName = key;
                    if (!string.IsNullOrEmpty(extName))
                        colName = string.Format("{0}{1}", extName, key);
                    var property = propertys[key];
                    if (property != null && property.Property != null)
                    {
                        var pType = property.Property.PropertyType;
                        if (pType.IsClass && !pType.IsValueType && !pType.IsArray && !pType.IsEnum && pType.Name != "String")
                        {
                            //extName = string.Format("{0}{1}_", extName, property.Property.Name);
                            EntityToSqlParameters(property.GetValue(entity), paramList, paramNames, string.Format("{0}{1}_", extName, property.Property.Name).ToLower());
                        }
                        else
                        {
                            var value = property.GetValue(entity);
                            if (value != null && !(value is DBNull))
                                paramList.Add(new SqlParameter("@" + colName, value));
                        }
                    }
                }
            }
            return paramList.ToArray();
        }
        #endregion

        #region 
        private static IDictionary<Type, Func<Type, object>> _generateEntity = new Dictionary<Type, Func<Type, object>>();
        /// <summary>
        /// 生成实体对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object GenerateEntity(Type type)
        {
            lock (type)
            {
                if (!_generateEntity.ContainsKey(type))
                {
                    var paramType = Expression.Parameter(typeof(Type));

                    var objLinq = Expression.New(type);

                    _generateEntity[type] = Expression.Lambda<Func<Type, object>>(objLinq, paramType).Compile();
                }
            }

            return _generateEntity[type](type);
        }

        #region GetPageList 返回集合
        /// <summary>
        /// 返回分页集合
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="isDesc">是否倒序</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <param name="isEfficient">是否使用高效存储过程（高效存储过程要求排序字段只有一个 值类型 数据唯一）</param>
        /// <returns></returns>
        public static MFReturnValue<List<T>> GetPageList<T>(string tableName, string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, int? pageSize = 20, int? pageIndex = 1, bool isAutoStatus = true, bool isEfficient = false, SqlTransaction trans = null, string connectionString="") where T : new()
        {
            if (isAutoStatus && condition.IndexOf("Status") == -1)
            {
                var statuString = " Status = 1 ";
                condition = statuString + (condition.IsNullOrEmpty() ? string.Empty : " And ") + condition;
            }

            //非高效存储过程 并且 不包含排序关键字的时候 自动加排序关键字
            if (orderby.IndexOf("asc") == -1 && orderby.IndexOf("desc") == -1 && orderby.IndexOf(",") == -1 && isEfficient == false)
            {
                orderby += isDesc ? " desc" : " asc";
            }

            //表名
            var tableParam = new SqlParameter("@TableName", tableName);
            //查询列
            var colParam = new SqlParameter("@Fields", columns);
            //查询条件
            var whereParam = new SqlParameter("@OrderField", orderby);
            //排序条件
            var orderbyParam = new SqlParameter("@sqlWhere", condition);
            //分页条数
            var pageSizeParam = new SqlParameter("@PageSize", pageSize);
            //当前页码
            var pageIndexParam = new SqlParameter("@PageIndex", pageIndex);
            //总记录数
            var pagesParam = new SqlParameter("@TotalCount", SqlDbType.Int);
            pagesParam.Direction = ParameterDirection.Output;

            //排序方式
            var orderTypeParam = new SqlParameter("@OrderType", isDesc);

            SqlParameter[] paramArray = null;

            //高效存储过程多一个参数
            if (isEfficient)
            {
                paramArray = new SqlParameter[] { tableParam, colParam, whereParam, orderbyParam, pageSizeParam, pageIndexParam, orderTypeParam, pagesParam };
            }
            else
            {
                paramArray = new SqlParameter[] { tableParam, colParam, whereParam, orderbyParam, pageSizeParam, pageIndexParam, pagesParam };
            }

            IDataReader reader;
            if (trans == null)
            {
                reader = MFSqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, isEfficient ? "GetPageList_High" : "GetPageList", paramArray);
            }
            else
            {
                reader = MFSqlHelper.ExecuteReader(trans, CommandType.StoredProcedure, isEfficient ? "GetPageList_High" : "GetPageList", paramArray);
            }
            var backVal = new MFReturnValue<List<T>>();
            backVal.Value = ReaderToEntityList<T>(reader);
            backVal.OutputValues["TotalCount"] = pagesParam.Value.ToInt();
            return backVal;
        }

        /// <summary>
        /// 将DataReader转换成实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ReaderToEntityList<T>(IDataReader reader) where T : new()
        {
            List<T> entityList = new List<T>();
            using (reader)
            {
                while (reader != null && reader.Read())
                {
                    entityList.Add(GetEntity(reader, new T()));
                }
            }
            return entityList;
        }
        #endregion
        #endregion
    }
}