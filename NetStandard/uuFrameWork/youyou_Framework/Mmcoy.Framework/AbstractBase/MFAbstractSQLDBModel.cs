using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mmcoy.Framework.Helper;

namespace Mmcoy.Framework.AbstractBase
{
    #region MFAbstractSQLDBModel SQLDBModel抽象基类
    /// <summary>
    /// SQLDBModel抽象基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MFAbstractSQLDBModel<T>
        where T : MFAbstractEntity, new()
    {
        #region 抽象属性或方法

        #region ConnectionString 数据库连接字符串
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected abstract string ConnectionString { get; }
        #endregion

        #region TableName 表名
        /// <summary>
        /// 表名
        /// </summary>
        protected abstract string TableName { get; }
        #endregion

        #region ValueParas 将实体属性转换为参数
        /// <summary>
        /// 将实体属性转换为参数
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract SqlParameter[] ValueParas(T entity);
        #endregion

        #region GetEntitySelfProperty 封装实体
        /// <summary>
        /// 封装实体
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="table"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract T GetEntitySelfProperty(IDataReader reader, DataTable table);
        #endregion

        #region ColumnList 列集合
        /// <summary>
        /// 列集合
        /// </summary>
        protected abstract IList<string> ColumnList { get; }
        #endregion

        #endregion

        #region 增 改 删

        #region Create 新增对象
        /// <summary>
        /// 新增对象
        /// </summary>
        /// <returns></returns>
        public MFReturnValue<object> Create(T entity)
        {
            return Create(null, entity);
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="trans">事物</param>
        /// <returns></returns>
        public MFReturnValue<object> Create(SqlTransaction trans, T entity)
        {
            SqlParameter[] paramArray = ValueParas(entity);
            paramArray[0].Direction = ParameterDirection.Output;

            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;
            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;

            if (trans.IsNullOrEmpty())
            {
                MFSqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "{0}_Create".FormatWith(this.TableName), paramArray);
            }
            else
            {
                MFSqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "{0}_Create".FormatWith(this.TableName), paramArray);
            }

            int nReturnCode = paramArray[paramArray.Length - 1].Value.ToInt();

            MFReturnValue<object> val = new MFReturnValue<object>();

            if (nReturnCode < 0)
            {
                val.HasError = true;
            }
            else
            {
                val.HasError = false;
                val.OutputValues["Id"] = paramArray[0].Value.ToInt();
            }
            val.Message = paramArray[paramArray.Length - 2].Value.ObjectToString();
            val.ReturnCode = nReturnCode;
            return val;
        }
        #endregion

        #region Update 更新对象
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <returns></returns>
        public MFReturnValue<object> Update(T entity)
        {
            return Update(null, entity);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="trans">事物</param>
        /// <returns></returns>
        public MFReturnValue<object> Update(SqlTransaction trans, T entity)
        {
            var paramArray = ValueParas(entity);

            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;
            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;

            if (trans.IsNullOrEmpty())
            {
                MFSqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "{0}_Update".FormatWith(this.TableName), paramArray);
            }
            else
            {
                MFSqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "{0}_Update".FormatWith(this.TableName), paramArray);
            }

            int nReturnCode = paramArray[paramArray.Length - 1].Value.ToInt();

            MFReturnValue<object> val = new MFReturnValue<object>();

            if (nReturnCode < 0)
            {
                val.HasError = true;
            }
            else
            {
                val.HasError = false;
            }
            val.Message = paramArray[paramArray.Length - 2].Value.ObjectToString();
            val.ReturnCode = nReturnCode;
            return val;
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="setStr"></param>
        /// <param name="conditionStr"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Update(string setStr, string conditionStr, IDictionary<string, object> parameters)
        {
            return Update(null, setStr, conditionStr, parameters);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="trans">事务</param>
        /// <param name="setStr"></param>
        /// <param name="conditionStr"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual MFReturnValue<object> Update(SqlTransaction trans, string setStr, string conditionStr, IDictionary<string, object> parameters)
        {
            MFReturnValue<object> retValue = new MFReturnValue<object>();
            try
            {
                SqlParameter[] paramArray = new SqlParameter[parameters.Count];

                int i = 0;
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    paramArray[i] = new SqlParameter(parameter.Key, parameter.Value);
                    i++;
                }

                string sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", this.TableName, setStr, conditionStr);

                if (trans != null)
                {
                    MFSqlHelper.ExecuteNonQuery(trans, CommandType.Text, sql, paramArray);
                }
                else
                {
                    MFSqlHelper.ExecuteNonQuery(this.ConnectionString, CommandType.Text, sql, paramArray);
                }
            }
            catch (Exception ex)
            {
                retValue.HasError = true;
                retValue.Message = ex.Message;
            }
            return retValue;
        }
        #endregion

        #region Delete 删除对象
        /// <summary>
        /// 删除指定编号的对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public MFReturnValue<object> Delete(int? id)
        {
            return Delete(id, null);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">,隔开的编号</param>
        /// <returns></returns>
        public MFReturnValue<object> Delete(string ids)
        {
            MFReturnValue<object> val = new MFReturnValue<object>();

            using (SqlConnection conn = new SqlConnection(this.ConnectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    IList<string> lst = ids.ToList();
                    lst.ForEach(id =>
                    {
                        Delete(id.ToInt(), trans);
                    });
                    trans.Commit();
                    val.HasError = false;
                    val.Message = "批量删除成功";
                }
                catch
                {
                    trans.Rollback();
                    val.HasError = false;
                    val.Message = "批量删除失败";
                }
            }
            return val;
        }

        /// <summary>
        /// 删除指定编号的对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="trans">事物</param>
        /// <returns></returns>
        public MFReturnValue<object> Delete(int? id, SqlTransaction trans)
        {
            MFReturnValue<object> val = new MFReturnValue<object>();

            int nId;
            if (id.HasValue)
            {
                nId = id.Value;
            }
            else
            {
                val.HasError = true;
                val.Message = "无效主键";
                return val;
            }

            SqlParameter[] paramArray = new SqlParameter[] { 
                new SqlParameter("@Id", nId),
                new SqlParameter("@RetMsg", SqlDbType.NVarChar, 255),
                new SqlParameter("@ReturnValue", SqlDbType.Int)
            };

            paramArray[paramArray.Length - 2].Direction = ParameterDirection.Output;
            paramArray[paramArray.Length - 1].Direction = ParameterDirection.ReturnValue;

            if (trans.IsNullOrEmpty())
            {
                MFSqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "{0}_Delete".FormatWith(this.TableName), paramArray);
            }
            else
            {
                MFSqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "{0}_Delete".FormatWith(this.TableName), paramArray);
            }

            int nReturnCode = paramArray[paramArray.Length - 1].Value.ToInt();

            if (nReturnCode < 0)
            {
                val.HasError = true;
            }
            else
            {
                val.HasError = false;
            }
            val.Message = paramArray[paramArray.Length - 2].Value.ObjectToString();
            val.ReturnCode = nReturnCode;
            return val;
        }
        #endregion

        #region ExecuteNonQuery 执行命令
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmdText">要执行的命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType cmdType = CommandType.StoredProcedure, SqlParameter[] param = null)
        {
            return MFSqlHelper.ExecuteNonQuery(this.ConnectionString, cmdType, cmdText, param);
        }
        #endregion

        #region ExecuteScalar 执行单值查询
        /// <summary>
        /// 执行单值查询
        /// </summary>
        /// <param name="cmdText">要执行的命令</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        protected object ExecuteScalar(string cmdText, CommandType cmdType = CommandType.StoredProcedure, SqlParameter[] param = null)
        {
            return MFSqlHelper.ExecuteScalar(this.ConnectionString, cmdType, cmdText, param);
        }

        protected object ExecuteScalar(SqlTransaction trans, string cmdText, CommandType cmdType, SqlParameter[] param)
        {
            if (trans == null)
            {
                return MFSqlHelper.ExecuteScalar(this.ConnectionString, cmdType, cmdText, param);
            }
            else
            {
                return MFSqlHelper.ExecuteScalar(trans, cmdType, cmdText, param);
            }
        }
        #endregion

        #endregion

        #region 查询(实体 列表)

        #region GetEntity 根据编号查询实体
        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity(int? id)
        {
            SqlParameter[] paramArray = new SqlParameter[] { 
                new SqlParameter("@Id", id)
            };

            return GetEntity("{0}_GetEntity".FormatWith(this.TableName), paramArray);
        }
        #endregion

        #region GetEntity 根据查询条件查询实体
        /// <summary>
        /// 根据查询条件查询实体
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <returns></returns>
        public T GetEntity(string condition, bool isAutoStatus = true)
        {
            if (isAutoStatus && ColumnList.Contains("Status") && condition.IndexOf("Status") == -1)
            {
                var statuString = " Status = 1 ";
                condition = (statuString + (condition.IsNullOrEmpty() ? string.Empty : " And ") + condition).ToDBStr();
            }

            return GetEntity("select * from [{0}] where {1}".FormatWith(this.TableName, condition), null, CommandType.Text);
        }
        #endregion

        #region GetEntity 自定义sql或存储过程查询实体
        /// <summary>
        /// 自定义sql或存储过程查询实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramArray"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public T GetEntity(string sql, SqlParameter[] paramArray, CommandType cmdType = CommandType.StoredProcedure)
        {
            T entity = default(T);
            using (var reader = MFSqlHelper.ExecuteReader(this.ConnectionString, cmdType, sql.ToDBStr(), paramArray))
            {
                if (reader != null && reader.Read())
                {
                    DataTable columnData = reader.GetSchemaTable();
                    entity = GetEntitySelfProperty(reader, columnData);
                }
            }
            return entity;
        }
        #endregion

        #region GetEntityList 通过DataReader获取实体对像列表
        /// <summary>
        /// 通过DataReader获取实体对像列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected List<T> GetEntityList(IDataReader reader)
        {
            List<T> list = new List<T>();
            using (reader)
            {
                while (reader != null && reader.Read())
                {
                    DataTable columnData = reader.GetSchemaTable();
                    list.Add(GetEntitySelfProperty(reader, columnData));
                }
            }
            return list;
        }
        #endregion

        #region GetPageList 返回集合
        public MFReturnValue<List<T>> GetPageList(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, int? pageSize = 20, int? pageIndex = 1, bool isAutoStatus = true, bool isEfficient = false)
        {
            return GetPageListWithTran(tableName: tableName, columns: columns, condition: condition, orderby: orderby, isDesc: isDesc, pageSize: pageSize, pageIndex: pageIndex, isAutoStatus: isAutoStatus, isEfficient: isEfficient, trans: null);
        }

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
        public MFReturnValue<List<T>> GetPageListWithTran(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, int? pageSize = 20, int? pageIndex = 1, bool isAutoStatus = true, bool isEfficient = false, SqlTransaction trans = null)
        {
            if (isAutoStatus && ColumnList.Contains("Status") && condition.IndexOf("Status") == -1)
            {
                var statuString = " Status = 1 ";
                condition = statuString + (condition.IsNullOrEmpty() ? string.Empty : " And ") + condition;
            }

            //非高效存储过程 并且 不包含排序关键字的时候 自动加排序关键字
            if (orderby.IndexOf("asc", StringComparison.CurrentCultureIgnoreCase) == -1 && orderby.IndexOf("desc", StringComparison.CurrentCultureIgnoreCase) == -1 && orderby.IndexOf(",") == -1 && isEfficient == false)
            {
                orderby += isDesc ? " desc" : " asc";
            }

            if (tableName.IsNullOrEmpty())
            {
                tableName = (this.TableName.IndexOf(" join ", StringComparison.CurrentCultureIgnoreCase) == -1) ? string.Format("[{0}]", this.TableName) : this.TableName;
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
                reader = MFSqlHelper.ExecuteReader(this.ConnectionString, CommandType.StoredProcedure, isEfficient ? "GetPageList_High" : "GetPageList", paramArray);
            }
            else
            {
                reader = MFSqlHelper.ExecuteReader(trans, CommandType.StoredProcedure, isEfficient ? "GetPageList_High" : "GetPageList", paramArray);
            }
            var backVal = new MFReturnValue<List<T>>();
            backVal.Value = GetEntityList(reader);
            backVal.OutputValues["TotalCount"] = pagesParam.Value.ToInt();
            return backVal;
        }
        #endregion

        #region GetList 返回集合
        public List<T> GetList(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, bool isAutoStatus = true, bool isEfficient = false)
        {
            return GetListWithTran(tableName: tableName, columns: columns, condition: condition, orderby: orderby, isDesc: isDesc, isAutoStatus: isAutoStatus, isEfficient: isEfficient, trans: null);
        }

        /// <summary>
        /// 返回集合
        /// </summary>
        /// <param name="columns">列名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="isDesc">是否倒序</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <param name="isEfficient">是否使用高效存储过程（高效存储过程要求排序字段只有一个 值类型 数据唯一）</param>
        /// <returns></returns>
        public List<T> GetListWithTran(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, bool isAutoStatus = true, bool isEfficient = false, SqlTransaction trans = null)
        {
            return GetPageListWithTran(tableName, columns, condition, orderby, isDesc, 1000, 1, isAutoStatus, isEfficient, trans: trans).Value;
        }
        #endregion

        #region GetCount 返回总数
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int GetCount(string condition = "")
        {
            condition = condition.IsNullOrEmpty() ? string.Empty : "where {0}".FormatWith(condition);
            string strSql = "select count(0) from [{0}] {1}".FormatWith(this.TableName, condition);
            return MFStringUtil.ToInt(MFSqlHelper.ExecuteScalar(this.ConnectionString, CommandType.Text, strSql));
        }

        #endregion

        #endregion
    }
    #endregion
}