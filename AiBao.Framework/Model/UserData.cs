using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Model
{
    /// <summary>
    /// 保存到cookie的数据
    /// </summary>
    [Serializable]
    public class UserData
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin { get; set; }

    }
}
