﻿using System;
using System.Collections.Generic;

namespace DotMaysWind.Data.Command.Condition
{
    /// <summary>
    /// Sql简单条件语句类
    /// </summary>
    public sealed class SqlBasicCommandCondition : AbstractSqlCondition
    {
        #region 字段
        private String _columnName;
        private SqlOperator _operator;
        private SelectCommand _command;
        #endregion

        #region 属性
        /// <summary>
        /// 获取语句类型
        /// </summary>
        public override SqlConditionType ConditionType
        {
            get { return SqlConditionType.BasicCommandCondition; }
        }

        /// <summary>
        /// 获取字段名称
        /// </summary>
        public String ColumnName
        {
            get { return this._columnName; }
        }
        
        /// <summary>
        /// 获取Sql查询类型
        /// </summary>
        public SqlOperator Operator
        {
            get { return this._operator; }
        }
        #endregion

        #region 构造方法
        /// <summary>
        /// 初始化Sql查询语句类
        /// </summary>
        /// <param name="baseCommand">源Sql语句</param>
        /// <param name="columnName">字段名称</param>
        /// <param name="op">条件运算符</param>
        /// <param name="tableName">查询的表名</param>
        /// <param name="action">设置选择语句的方法</param>
        private SqlBasicCommandCondition(AbstractSqlCommandWithWhere baseCommand, String columnName, SqlOperator op, String tableName, Action<SelectCommand> action)
            : base(baseCommand)
        {
            this._columnName = columnName;
            this._operator = op;

            SelectCommand command = baseCommand.Database.InternalCreateSelectCommand((baseCommand.RootSource == null ? baseCommand : baseCommand.RootSource), tableName);
            action(command);
            this._command = command;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取条件语句包含的参数集合
        /// </summary>
        /// <returns>条件语句参数集合</returns>
        public override DataParameter[] GetAllParameters()
        {
            if (this._command == null)
            {
                return null;
            }

            return this._command.GetAllParameters();
        }

        /// <summary>
        /// 输出条件语句
        /// </summary>
        /// <returns>条件语句</returns>
        public override String GetClauseText()
        {
            if (this._command == null)
            {
                return String.Empty;
            }

            String format = String.Format("({0})", SqlOperators.InternalGetOperatorFormat(this._operator));

            return String.Format(format, this._columnName, '(' + this._command.GetCommandText() + ')');
        }
        #endregion

        #region 重载方法和运算符
        /// <summary>
        /// 获取当前参数的哈希值
        /// </summary>
        /// <returns>当前参数的哈希值</returns>
        public override Int32 GetHashCode()
        {
            return this._columnName.GetHashCode();
        }

        /// <summary>
        /// 判断两个Sql简单条件语句是否相同
        /// </summary>
        /// <param name="obj">待比较的Sql简单条件语句</param>
        /// <returns>两个Sql简单条件语句是否相同</returns>
        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            SqlBasicCommandCondition condition = obj as SqlBasicCommandCondition;

            if (condition == null)
            {
                return false;
            }

            if (this._operator != condition._operator)
            {
                return false;
            }

            if (!String.Equals(this._columnName, condition._columnName))
            {
                return false;
            }

            if ((this._command != null && condition._command == null) || (this._command == null && condition._command != null))
            {
                return false;
            }

            if (this._command != null && condition._command != null && !this._command.Equals(condition._command))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断两个Sql简单条件语句是否相同
        /// </summary>
        /// <param name="obj">待比较的Sql简单条件语句</param>
        /// <param name="obj2">待比较的第二个Sql简单条件语句</param>
        /// <returns>两个Sql简单条件语句是否相同</returns>
        public static Boolean operator ==(SqlBasicCommandCondition obj, SqlBasicCommandCondition obj2)
        {
            return Object.Equals(obj, obj2);
        }

        /// <summary>
        /// 判断两个Sql简单条件语句是否不同
        /// </summary>
        /// <param name="obj">待比较的Sql简单条件语句</param>
        /// <param name="obj2">待比较的第二个Sql简单条件语句</param>
        /// <returns>两个Sql简单条件语句是否不同</returns>
        public static Boolean operator !=(SqlBasicCommandCondition obj, SqlBasicCommandCondition obj2)
        {
            return !Object.Equals(obj, obj2);
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 创建新的Sql条件语句
        /// </summary>
        /// <param name="cmd">Sql语句</param>
        /// <param name="columnName">字段名称</param>
        /// <param name="op">条件运算符</param>
        /// <param name="tableName">查询的表名</param>
        /// <param name="action">设置选择语句的方法</param>
        /// <exception cref="ArgumentNullException">设置语句的方法不能为空</exception>
        /// <returns>Sql条件语句</returns>
        internal static SqlBasicCommandCondition InternalCreate(AbstractSqlCommandWithWhere cmd, String columnName, SqlOperator op, String tableName, Action<SelectCommand> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            return new SqlBasicCommandCondition(cmd, columnName, op, tableName, action);
        }

        /// <summary>
        /// 创建新的Sql条件语句
        /// </summary>
        /// <param name="cmd">Sql语句</param>
        /// <param name="columnName">字段名称</param>
        /// <param name="op">条件运算符</param>
        /// <param name="action">设置选择语句的方法</param>
        /// <exception cref="ArgumentNullException">设置语句的方法不能为空</exception>
        /// <returns>Sql条件语句</returns>
        internal static SqlBasicCommandCondition InternalCreate(AbstractSqlCommandWithWhere cmd, String columnName, SqlOperator op, Action<SelectCommand> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            return new SqlBasicCommandCondition(cmd, columnName, op, cmd.TableName, action);
        }
        #endregion
    }
}