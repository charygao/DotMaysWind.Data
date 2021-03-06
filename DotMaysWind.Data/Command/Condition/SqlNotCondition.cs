﻿using System;
using System.Text;

namespace DotMaysWind.Data.Command.Condition
{
    /// <summary>
    /// Sql Not条件语句类
    /// </summary>
    public sealed class SqlNotCondition : AbstractSqlCondition
    {
        #region 字段
        private ISqlCondition _baseCondition;
        #endregion

        #region 属性
        /// <summary>
        /// 获取语句类型
        /// </summary>
        public override SqlConditionType ConditionType
        {
            get { return SqlConditionType.NotCondition; }
        }

        /// <summary>
        /// 获取或设置基础Sql条件语句
        /// </summary>
        public ISqlCondition BaseCondition
        {
            get { return this._baseCondition; }
            set { this._baseCondition = value; }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化Sql Not条件语句类
        /// </summary>
        /// <param name="baseCommand">源Sql语句</param>
        /// <param name="baseCondition">基础Sql条件语句</param>
        private SqlNotCondition(AbstractSqlCommandWithWhere baseCommand, ISqlCondition baseCondition)
            : base(baseCommand)
        {
            this._baseCondition = baseCondition;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取条件语句包含的参数集合
        /// </summary>
        /// <returns>条件语句参数集合</returns>
        public override DataParameter[] GetAllParameters()
        {
            return this._baseCondition.GetAllParameters();
        }

        /// <summary>
        /// 输出条件语句
        /// </summary>
        /// <returns>条件语句</returns>
        public override String GetClauseText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(NOT(");
            sb.Append(this._baseCondition.GetClauseText());
            sb.Append("))");

            return sb.ToString();
        }
        #endregion

        #region 重载方法和运算符
        /// <summary>
        /// 获取当前参数的哈希值
        /// </summary>
        /// <returns>当前参数的哈希值</returns>
        public override Int32 GetHashCode()
        {
            return -this._baseCondition.GetHashCode();
        }

        /// <summary>
        /// 判断两个Sql条件语句是否相同
        /// </summary>
        /// <param name="obj">待比较的Sql条件语句</param>
        /// <returns>两个Sql条件语句是否相同</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            SqlNotCondition condition = obj as SqlNotCondition;

            if (condition == null)
            {
                return false;
            }

            if (!this._baseCondition.Equals(condition._baseCondition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断两个Sql条件语句是否相同
        /// </summary>
        /// <param name="obj">待比较的Sql条件语句</param>
        /// <param name="obj2">待比较的第二个Sql条件语句</param>
        /// <returns>两个Sql条件语句是否相同</returns>
        public static Boolean operator ==(SqlNotCondition obj, SqlNotCondition obj2)
        {
            return Object.Equals(obj, obj2);
        }

        /// <summary>
        /// 判断两个Sql条件语句是否不同
        /// </summary>
        /// <param name="obj">待比较的Sql条件语句</param>
        /// <param name="obj2">待比较的第二个Sql条件语句</param>
        /// <returns>两个Sql条件语句是否不同</returns>
        public static Boolean operator !=(SqlNotCondition obj, SqlNotCondition obj2)
        {
            return !Object.Equals(obj, obj2);
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 创建新的Sql Not条件语句
        /// </summary>
        /// <param name="cmd">Sql语句</param>
        /// <param name="condition">条件语句</param>
        /// <exception cref="ArgumentNullException">Sql条件语句不能为空</exception>
        /// <returns>Sql Not条件语句</returns>
        internal static SqlNotCondition Create(AbstractSqlCommandWithWhere cmd, ISqlCondition condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }

            return new SqlNotCondition(cmd, condition);
        }
        #endregion
    }
}