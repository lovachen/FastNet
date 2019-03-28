using cts.web.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao
{
    public static class IPageListExtensions
    {

        /// <summary>
        /// 转成 jquery.datatable数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageList"></param>
        /// <returns></returns>
        public static DatatableModel<T> ToAjax<T>(this IPagedList<T> pageList)
        {
            DatatableModel<T> model = new DatatableModel<T>(pageList);
            return model;
        }

    }
}
