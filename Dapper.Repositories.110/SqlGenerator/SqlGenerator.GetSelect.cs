using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common;

namespace Dapper.Repositories.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        private SqlQuery GetSelect(Expression<Func<TEntity, bool>> predicate, bool firstOnly, int pageNo, int pageSize, bool includeLogicalDeleted, object from = null, object to = null, string columnName = "", bool betweenQuery = false, params Expression<Func<TEntity, object>>[] includes)
        {
            var sqlQuery = InitBuilderSelect(firstOnly);

            var joinsBuilder = AppendJoinToSelect(sqlQuery, includes);
            sqlQuery.SqlBuilder
                .Append(" FROM ")
                .Append(TableName)
                .Append(" ");

            if (includes.Any())
                sqlQuery.SqlBuilder.Append(joinsBuilder);

            AppendWherePredicateQuery(sqlQuery, predicate, QueryType.Select, includeLogicalDeleted);

            if (betweenQuery)
                sqlQuery.SqlBuilder
                    .Append(predicate == null && !LogicalDelete ? "WHERE" : "AND")
                    .Append(" ")
                    .Append(TableName)
                    .Append(".")
                    .Append(columnName)
                    .Append(" BETWEEN '")
                    .Append(from)
                    .Append("' AND '")
                    .Append(to)
                    .Append("'");

            if (firstOnly && (Config.SqlProvider == SqlProvider.MySQL || Config.SqlProvider == SqlProvider.PostgreSQL))
                sqlQuery.SqlBuilder.Append(" LIMIT 1");

            if (!firstOnly && pageNo > 0 && pageSize > 0)
            {
                int offset = (pageNo - 1) * pageSize;
                int fetch = pageSize;

                string keyProperties = string.Join(',', KeySqlProperties.Select(x => x.PropertyName).ToList());
                sqlQuery.SqlBuilder.Append($" ORDER BY {keyProperties}");

                if (Config.SqlProvider == SqlProvider.MSSQL)
                    sqlQuery.SqlBuilder.Append($" OFFSET {offset} ROWS FETCH NEXT {fetch} ROWS ONLY;");
                else if (Config.SqlProvider == SqlProvider.MySQL || Config.SqlProvider == SqlProvider.PostgreSQL)
                    sqlQuery.SqlBuilder.Append($" LIMIT {offset}, {fetch};");

                //Count Pages
                var countPagesQuery = new SqlQuery();
                countPagesQuery.SqlBuilder.Append($" SELECT CAST((CEILING(CAST(COUNT(*) AS DECIMAL) / CAST({fetch} AS DECIMAL))) AS INT) ");
                countPagesQuery.SqlBuilder
                    .Append(" FROM ")
                    .Append(TableName)
                    .Append(" ");

                if (includes.Any())
                    countPagesQuery.SqlBuilder.Append(joinsBuilder);

                AppendWherePredicateQuery(countPagesQuery, predicate, QueryType.Select, includeLogicalDeleted);
                if (betweenQuery)
                    countPagesQuery.SqlBuilder
                        .Append(predicate == null && !LogicalDelete ? "WHERE" : "AND")
                        .Append(" ")
                        .Append(TableName)
                        .Append(".")
                        .Append(columnName)
                        .Append(" BETWEEN '")
                        .Append(from)
                        .Append("' AND '")
                        .Append(to)
                        .Append("'");

                sqlQuery.SqlBuilder.Append(countPagesQuery.GetSql());
            }

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectFirst(Expression<Func<TEntity, bool>> predicate, bool includeLogicalDeleted, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, true, 0, 0, includeLogicalDeleted, includes);
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectAll(Expression<Func<TEntity, bool>> predicate, int pageNo, int pageSize, bool includeLogicalDeleted, object from = null, object to = null, string columnName = "", bool betweenQuery = false, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetSelect(predicate, false, pageNo, pageSize, includeLogicalDeleted, from, to, columnName, betweenQuery, includes);
        }

        /// <inheritdoc />
        public SqlQuery GetSelectById(object id, bool includeLogicalDeleted, params Expression<Func<TEntity, object>>[] includes)
        {
            if (KeySqlProperties.Length != 1)
                throw new NotSupportedException("GetSelectById support only 1 key");

            var keyProperty = KeySqlProperties[0];

            var sqlQuery = InitBuilderSelect(true);

            if (includes.Any())
            {
                var joinsBuilder = AppendJoinToSelect(sqlQuery, includes);
                sqlQuery.SqlBuilder
                    .Append(" FROM ")
                    .Append(TableName)
                    .Append(" ");

                sqlQuery.SqlBuilder.Append(joinsBuilder);
            }
            else
            {
                sqlQuery.SqlBuilder
                    .Append(" FROM ")
                    .Append(TableName)
                    .Append(" ");
            }

            IDictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { keyProperty.PropertyName, id }
            };

            sqlQuery.SqlBuilder
                .Append("WHERE ")
                .Append(TableName)
                .Append(".")
                .Append(keyProperty.ColumnName)
                .Append(" = @")
                .Append(keyProperty.PropertyName)
                .Append(" ");

            if (LogicalDelete && !includeLogicalDeleted)
                sqlQuery.SqlBuilder
                    .Append("AND ")
                    .Append(TableName)
                    .Append(".")
                    .Append(StatusPropertyName)
                    .Append(" != ")
                    .Append(LogicalDeleteValue)
                    .Append(" ");

            if (Config.SqlProvider == SqlProvider.MySQL || Config.SqlProvider == SqlProvider.PostgreSQL)
                sqlQuery.SqlBuilder.Append("LIMIT 1");

            sqlQuery.SetParam(dictionary);

            LogSqlQuery(sqlQuery);
            return sqlQuery;
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectBetween(object from, object to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted)
        {
            return GetSelectBetween(from, to, btwField, pageNo, pageSize, includeLogicalDeleted, null);
        }

        /// <inheritdoc />
        public virtual SqlQuery GetSelectBetween(object from, object to, Expression<Func<TEntity, object>> btwField, int pageNo, int pageSize, bool includeLogicalDeleted, Expression<Func<TEntity, bool>> predicate)
        {
            var fieldName = ExpressionHelper.GetPropertyName(btwField);
            var columnName = SqlProperties.First(x => x.PropertyName == fieldName).ColumnName;
            var query = GetSelectAll(predicate, pageNo, pageSize, includeLogicalDeleted, from, to, columnName, true);

            LogSqlQuery(query);
            return query;
        }

        private SqlQuery InitBuilderSelect(bool firstOnly)
        {
            var query = new SqlQuery();
            query.SqlBuilder.Append("SELECT ");
            if (firstOnly && Config.SqlProvider == SqlProvider.MSSQL)
                query.SqlBuilder.Append("TOP 1 ");

            query.SqlBuilder.Append(GetFieldsSelect(TableName, SqlProperties));

            return query;
        }

        private static string GetFieldsSelect(string tableName, SqlPropertyMetadata[] properties)
        {
            //Projection function
            string ProjectionFunction(SqlPropertyMetadata p)
            {
                return !string.IsNullOrEmpty(p.Alias)
                    ? string.Format("{0}.{1} AS {2}", tableName, p.ColumnName, p.PropertyName)
                    : string.Format("{0}.{1}", tableName, p.ColumnName);
            }

            return string.Join(", ", properties.Select(ProjectionFunction));
        }
    }
}
