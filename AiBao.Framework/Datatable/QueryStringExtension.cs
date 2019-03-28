using AiBao.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// 查询参数扩展，转换成 DataTablesParameters
    /// </summary>
    public static class QueryStringExtension
    {
        /// <summary>
        /// GET请求转换jquery.datatable的参数
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static DataTablesParameters ToTableParms(this QueryString queryString)
        {
            var param = new DataTablesParameters();
            if (queryString != null && !String.IsNullOrEmpty(queryString.Value))
            {
                var queryValue = System.Web.HttpUtility.UrlDecode(queryString.Value).Trim().Trim('?');
                if (queryValue.IndexOf('&') > -1)
                {
                    var p_arr = queryValue.Split('&');
                    foreach (var p in p_arr)
                    {
                        int index = p.IndexOf('=');
                        if (index > -1)
                        {
                            var name = p.Substring(0, index);
                            if (name.Equals("handler", StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }
                            var value = p.Substring(index + 1);
                            if (name.StartsWith("columns", StringComparison.InvariantCultureIgnoreCase))
                            {
                                param.Columns.Add(name, value);
                            }
                            else if (name.StartsWith("order", StringComparison.InvariantCultureIgnoreCase))
                            {
                                param.Order.Add(name, value);
                            }else if (name.StartsWith("start", StringComparison.InvariantCultureIgnoreCase))
                            {
                                int.TryParse(value, out int _start);
                                param.Start = _start;
                            }
                            else if (name.StartsWith("length", StringComparison.InvariantCultureIgnoreCase))
                            {
                                int.TryParse(value, out int _length);
                                param.Length = _length;
                            }
                            else if (name.StartsWith("search", StringComparison.InvariantCultureIgnoreCase))
                            {
                                param.Search.Add(name, value);
                            }
                        }
                    }
                }
            }
            return param;
        }
    }
}
