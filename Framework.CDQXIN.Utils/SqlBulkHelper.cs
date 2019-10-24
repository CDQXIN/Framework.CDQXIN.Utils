using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    public static class SqlBulkHelper
    {

        /// <summary>
        /// SqlBulkCopy批量插入数据
        /// 使用方法：SqlBulkHelper.SqlBulkCopyByDataTable(DBConnection.LianXue, "EX_SJCTB", dt);
        /// </summary>
        /// <param name="connectionStr">链接字符串</param>
        /// <param name="dataTableName">表名</param>
        /// <param name="sourceDataTable">数据源</param>
        /// <param name="timeOut">超时时间/秒</param>
        /// <param name="batchSize">一次事务插入的行数</param>
        public static void SqlBulkCopyByDataTable(string connectionStr, string dataTableName, DataTable sourceDataTable, int timeOut = 60, int batchSize = 100000)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionStr, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlBulkCopy.BulkCopyTimeout = timeOut;
                        sqlBulkCopy.DestinationTableName = dataTableName;
                        sqlBulkCopy.BatchSize = batchSize;
                        for (int i = 0; i < sourceDataTable.Columns.Count; i++)
                        {
                            sqlBulkCopy.ColumnMappings.Add(sourceDataTable.Columns[i].ColumnName, sourceDataTable.Columns[i].ColumnName);
                        }
                        sqlBulkCopy.WriteToServer(sourceDataTable);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// SqlBulkCopy批量插入数据
        /// 使用方法：SqlBulkHelper.SqlBulkCopyByList(DBConnection.LianXue, "EX_SJCTB", list);
        /// </summary>
        /// <param name="connectionStr">链接字符串</param>
        /// <param name="dataTableName">表名</param>
        /// <param name="list">数据源</param>
        /// <param name="timeOut">超时时间/秒</param>
        /// <param name="batchSize">一次事务插入的行数</param>
        public static void SqlBulkCopyByList<T>(string connectionStr, string dataTableName, List<T> list, int timeOut = 60, int batchSize = 100000)
        {
            DataTable dt = list.ToDataTable();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionStr, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlBulkCopy.BulkCopyTimeout = timeOut;
                        sqlBulkCopy.DestinationTableName = dataTableName;
                        sqlBulkCopy.BatchSize = batchSize;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlBulkCopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlBulkCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

    }
}
