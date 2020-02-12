
//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2020-01-29 08:22:06
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Job数据管理
/// </summary>
public partial class JobDBModel : AbstractDBModel<JobDBModel, JobEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "Job.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override JobEntity MakeEntity(GameDataTableParser parse)
    {
        JobEntity entity = new JobEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.Name = parse.GetFieldValue("Name");
        entity.Headpic = parse.GetFieldValue("Headpic");
        entity.PrefabName = parse.GetFieldValue("PrefabName");
        entity.Des = parse.GetFieldValue("Des");
        return entity;
    }
}
