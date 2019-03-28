using cts.web.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao
{
    [Serializable]
    public class DatatableModel<T>
    {

        public DatatableModel(IPagedList<T> pageList)
        {
            recordsFiltered = pageList.TotalCount;
            recordsTotal = pageList.TotalCount;
            data = pageList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public DatatableModel(IList<T> list)
        {
            recordsTotal = list.Count;
            recordsFiltered = recordsTotal;
            data = list;
        }

        /// <summary>
        /// 上面提到了，Datatables发送的draw是多少那么服务器就返回多少。 这里注意，作者出于安全的考虑，强烈要求把这个转换为整形，即数字后再返回，而不是纯粹的接受然后返回，这是 为了防止跨站脚本（XSS）攻击。
        /// </summary>
        public int draw { get; set; }

        /// <summary>
        /// 即没有过滤的记录数（数据库里总共记录数）
        /// </summary>
        public int recordsTotal { get; set; }

        /// <summary>
        /// 过滤后的记录数（如果有接收到前台的过滤条件，则返回的是过滤后的记录数）
        /// </summary>
        public int recordsFiltered { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }


    }
}
