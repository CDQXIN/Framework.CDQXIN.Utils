﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.CDQXIN.Utils
{
    /// <summary>
    /// CSV文件工具类
    /// </summary>
    public class CsvHelper
    {
        #region 导出报表为Csv
        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">物理路径</param>
        /// <param name="tableheader">表头</param>
        /// <param name="columname">字段标题,逗号分隔</param>
        public static bool DtToCsv(DataTable dt, string strFilePath, string tableheader, string columname)
        {
            try
            {
                var strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);

                strmWriterObj.WriteLine(tableheader);
                strmWriterObj.WriteLine(columname);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var strBufferLine = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dt.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }

                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 将Csv读入DataTable
        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        /// <param name="dt"></param>
        public static DataTable CsvToDt(string filePath, int n, DataTable dt)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8, false);
            var m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    if (str != null)
                    {
                        string[] split = str.Split(',');

                        DataRow dr = dt.NewRow();
                        int i;
                        for (i = 0; i < split.Length; i++)
                        {
                            dr[i] = split[i];
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
        #endregion
    }
}
