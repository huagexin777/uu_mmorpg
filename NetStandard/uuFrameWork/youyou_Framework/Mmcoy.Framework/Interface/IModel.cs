using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mmcoy.Framework.AbstractBase;

namespace Mmcoy.Framework.Interface
{
    public interface IModel<T> where T : MFAbstractEntity, new()
    {
        #region Create 新增对象
        /// <summary>
        /// 新增对象
        /// </summary>
        /// <returns></returns>
        MFReturnValue<object> Create(T entity);
        #endregion

        #region Update 更新对象
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <returns></returns>
        MFReturnValue<object> Update(T entity);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="setStr"></param>
        /// <param name="conditionStr"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        MFReturnValue<object> Update(string setStr, string conditionStr, IDictionary<string, object> parameters);
        #endregion

        #region Delete 删除对象
        /// <summary>
        /// 删除指定编号的对象
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        MFReturnValue<object> Delete(int? id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">,隔开的编号</param>
        /// <returns></returns>
        MFReturnValue<object> Delete(string ids);
        #endregion

        #region GetEntity 根据编号查询实体
        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntity(int? id);
        #endregion

        #region GetEntity 根据查询条件查询实体
        /// <summary>
        /// 根据查询条件查询实体
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <returns></returns>
        T GetEntity(string condition, bool isAutoStatus = true);
        #endregion

        #region GetPageList 返回集合
        /// <summary>
        /// 返回分页集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="isDesc">是否倒序</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <param name="isEfficient">是否使用高效存储过程（高效存储过程要求排序字段只有一个 值类型 数据唯一）</param>
        /// <returns></returns>
        MFReturnValue<List<T>> GetPageList(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, int? pageSize = 20, int? pageIndex = 1, bool isAutoStatus = true, bool isEfficient = false);
        #endregion

        #region GetList 返回集合
        /// <summary>
        /// 返回集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="isDesc">是否倒序</param>
        /// <param name="isAutoStatus">是否自动状态</param>
        /// <param name="isEfficient">是否使用高效存储过程（高效存储过程要求排序字段只有一个 值类型 数据唯一）</param>
        /// <returns></returns>
        List<T> GetList(string tableName = "", string columns = "*", string condition = "", string orderby = "Id", bool isDesc = true, bool isAutoStatus = true, bool isEfficient = false);
        #endregion

        #region GetCount 返回总数
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int GetCount(string condition = "");
        #endregion
         
    }
}