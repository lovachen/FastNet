using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class DataTablesParameters
    {
        /// <summary>
        /// 列
        /// </summary>
        public Dictionary<string, string> Columns = new Dictionary<string, string>();

        /// <summary>
        /// 排序
        /// </summary>
        public Dictionary<string, string> Order = new Dictionary<string, string>();

        /// <summary>
        /// 查询关键字
        /// </summary>
        public Dictionary<string, string> Search = new Dictionary<string, string>();

        /// <summary>
        /// 开始条数
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 请求的页码
        /// </summary>
        public int PageIndex => Start / Length + 1;

        /// <summary>
        /// 排序的字段名称
        /// </summary>
        public string OrderName
        {
            get
            {
                if (Order.Any() && Columns.Any())
                {
                    if (Order.ContainsKey("order[0][column]"))
                    {
                        string column = Order["order[0][column]"];
                        string key = $"columns[{column}][data]";
                        if (Columns.ContainsKey(key))
                        {
                            return Columns[key] ?? String.Empty;
                        }
                    }
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// 排序方式，asc：正序，desc：倒序
        /// </summary>
        public string OrderDir
        {
            get
            {
                if (Order.Any())
                {
                    if (Order.ContainsKey("order[0][dir]"))
                    {
                        return Order["order[0][dir]"] ?? "asc";
                    }
                }
                return "asc";
            }
        }

    }


}
