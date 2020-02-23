using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmcoy.Framework.AbstractBase
{
    #region 枚举类型

    #region EnumEntityStatus 通用数据状态枚举
    /// <summary>
    /// 通用数据状态枚举
    /// </summary>
    public enum EnumEntityStatus : byte
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = 0,
        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        Released = 1
    }
    #endregion

    #region EnumCategoryStatus 分类枚举
    /// <summary>
    /// 分类枚举
    /// </summary>
    public enum EnumCategoryStatus : byte
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = 0,
        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        Released = 1,
        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        Invalid = 2
    }
    #endregion

    #region EnumContentStatus 通用内容状态枚举
    /// <summary>
    /// 通用内容状态枚举
    /// </summary>
    public enum EnumContentStatus : byte
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Deleted = 0,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Released = 1,
        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        UnAudit = 2
    }
    #endregion

    #region EnumSex 性别枚举
    /// <summary>
    /// 性别枚举
    /// </summary>
    public enum EnumSex : byte
    {
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female = 0,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male = 1,
        /// <summary>
        /// 保密
        /// </summary>
        [Description("保密")]
        UnSetting = 2
    }
    #endregion

    #region EnumTerminal 终端枚举
    /// <summary>
    /// 终端枚举
    /// </summary>
    public enum EnumTerminal : byte
    {
        /// <summary>
        /// 网站
        /// </summary>
        [Description("网站")]
        Web = 0,
        /// <summary>
        /// iPhone客户端
        /// </summary>
        [Description("iPhone")]
        iPhone = 1,
        /// <summary>
        /// iPad
        /// </summary>
        [Description("iPad")]
        iPad = 2,
        /// <summary>
        /// Android手机
        /// </summary>
        [Description("Android手机")]
        AndroidMobile = 3,
        /// <summary>
        /// Android平板
        /// </summary>
        [Description("Android平板")]
        AndroidPad = 4,
        /// <summary>
        /// WindowsPhone
        /// </summary>
        [Description("WindowsPhone")]
        WindowsPhone = 5,
        /// <summary>
        /// WindowsPad
        /// </summary>
        [Description("WindowsPad")]
        WindowsPad = 6,
        /// <summary>
        /// PC_Client
        /// </summary>
        [Description("PC_Client")]
        PC_Client = 7,
    }
    #endregion

    #region 资源类型 EnumSourceType
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum EnumSourceType
    {
        /// <summary>
        /// 软件
        /// </summary>
        [Description("软件")]
        Soft = 0
    }
    #endregion

    #region EnumElementType 元素类型枚举
    /// <summary>
    /// 元素类型枚举
    /// </summary>
    public enum EnumElementType : byte
    {
        /// <summary>
        /// 未设置
        /// </summary>
        [Description("未设置")]
        None = 0,
        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签")]
        Tag = 1,
        /// <summary>
        /// 会员
        /// </summary>
        [Description("会员")]
        Member = 2,
        /// <summary>
        /// 课程
        /// </summary>
        [Description("课程")]
        Course = 3,
        /// <summary>
        /// 课件
        /// </summary>
        [Description("课件")]
        CourseWare = 4,
        /// <summary>
        /// 文章
        /// </summary>
        [Description("文章")]
        Article = 5,
        /// <summary>
        /// 论坛版块
        /// </summary>
        [Description("论坛版块")]
        TopicCategory = 6,
        /// <summary>
        /// 帖子
        /// </summary>
        [Description("帖子")]
        Topic = 7,
        /// <summary>
        /// 回帖
        /// </summary>
        [Description("回帖")]
        TopicReply = 8
    }
    #endregion

    #region EnumActionType 操作类型枚举
    /// <summary>
    /// 操作类型枚举 用于计算 加 减
    /// </summary>
    public enum EnumActionType : byte
    {
        /// <summary>
        /// 加
        /// </summary>
        [Description("加")]
        Add = 0,
        /// <summary>
        /// 减
        /// </summary>
        [Description("减")]
        Subtract = 1
    }
    #endregion

    #region EnumCourseWareType 课件类型
    /// <summary>
    /// 课件类型
    /// </summary>
    public enum EnumCourseWareType : byte
    {
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video = 0,
        /// <summary>
        /// 音频
        /// </summary>
        [Description("音频")]
        Audio = 1,
        /// <summary>
        /// 图文
        /// </summary>
        [Description("图文")]
        ImageText = 2,
        /// <summary>
        /// 幻灯片
        /// </summary>
        [Description("幻灯片")]
        Slide = 3
    }
    #endregion

    #region EnumCourseWareResolution 课件分辨率
    /// <summary>
    /// 课件分辨率
    /// </summary>
    public enum EnumCourseWareResolution : byte
    { 
        /// <summary>
        /// 800*600
        /// </summary>
        [Description("800*600")]
        For800
    }
    #endregion

    #region EnumParentType 上级类型枚举
    /// <summary>
    /// 上级类型枚举
    /// </summary>
    public enum EnumParentType : byte
    {
        /// <summary>
        /// 问答
        /// </summary>
        [Description("问答")]
        Topic = 0,
        /// <summary>
        /// 回复
        /// </summary>
        [Description("回复")]
        Reply = 1
    }
    #endregion

    #region EnumMessageType 消息枚举
    /// <summary>
    /// 消息枚举
    /// </summary>
    public enum EnumMessageType : byte
    {
        /// <summary>
        /// 系统消息
        /// </summary>
        [Description("系统消息")]
        System = 0,
        /// <summary>
        /// 企业消息
        /// </summary>
        [Description("企业消息")]
        Company = 1,
        /// <summary>
        /// 企业消息
        /// </summary>
        [Description("用户消息")]
        User = 2
    }
    #endregion

    #region EnumValidateModel 验证模式
    /// <summary>
    /// 验证模式
    /// </summary>
    public enum EnumValidateModel : byte
    {
        /// <summary>
        /// 邮箱验证
        /// </summary>
        [Description("邮箱验证")]
        EMail = 0,
        /// <summary>
        /// 手机验证
        /// </summary>
        [Description("手机验证")]
        Mobile = 1
    }
    #endregion

    #region EnumValidateType 验证类型
    /// <summary>
    /// 验证类型
    /// </summary>
    public enum EnumValidateType : byte
    {
        /// <summary>
        /// 会员注册
        /// </summary>
        [Description("会员注册")]
        Reg = 0,
        /// <summary>
        /// 找回密码
        /// </summary>
        [Description("找回密码")]
        FindPwd = 1,
        /// <summary>
        /// 重新绑定
        /// </summary>
        [Description("重新绑定")]
        ReBind = 2
    }
    #endregion

    #endregion

    #region MFAbstractEntity 实体抽象基类
    /// <summary>
    /// 实体抽象基类
    /// </summary>
    [Serializable]
    public abstract class MFAbstractEntity
    {
        #region PKValue 唯一主键
        /// <summary>
        /// 唯一主键
        /// </summary>
        [DisplayName("唯一主键")]
        public virtual int? PKValue { get; set; }
        #endregion
    }
    #endregion
}