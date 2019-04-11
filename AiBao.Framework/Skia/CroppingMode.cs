using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Skia
{
    /// <summary>
    /// 缩略图模式
    /// </summary>
    public enum CroppingMode
    {
        /// <summary>
        /// 宽指定，高等比缩放
        /// </summary>
        RATIO_WIDTH = 0,

        /// <summary>
        /// 高指定，宽等比缩放
        /// </summary>
        RATIO_HEIGHT = 1,

        /// <summary>
        /// 剪裁
        /// </summary>
        CUT = 2,

        /// <summary>
        /// 不剪裁
        /// </summary>
        NONE = 3
    }
}
