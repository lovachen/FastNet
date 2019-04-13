using System;
using System.Collections.Generic;
using System.Text;

namespace AiBao.Framework.Skia
{
    /// <summary>
    /// 拆解参数
    /// </summary>
    public class ResolveArg
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cut"></param>
        /// <param name="resize"></param>
        public ResolveArg(string cut, string resize)
        {
            CroppingMode mode = CroppingMode.NONE;
            int width = 0, height = 0;
            if (!String.IsNullOrEmpty(resize))
            {
                string w, h;
                if (resize.IndexOf('x') > -1)
                {
                    var arr = resize.Split("x");
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].StartsWith("w_"))
                        {
                            w = arr[i].Replace("w_", "");
                            int.TryParse(w, out width);
                        }
                        else
                        {
                            h = arr[i].Replace("h_", "");
                            int.TryParse(h, out height);
                        }
                    }
                }
                else if (resize.StartsWith("h_"))
                {
                    h = resize.Replace("h_", "");
                    int.TryParse(h, out height);
                }
                else if (resize.StartsWith("w_"))
                {
                    w = resize.Replace("w_", "");
                    int.TryParse(w, out width);
                }
                switch (cut)
                {
                    case "m_fill":
                        if (width > 0 && height > 0)
                            mode = CroppingMode.CUT;
                        break;
                    case "m_h":
                        if (height > 0)
                            mode = CroppingMode.RATIO_HEIGHT;
                        break;
                    case "m_w":
                        if (width > 0)
                            mode = CroppingMode.RATIO_WIDTH;
                        break;
                }
            }
            this.Width = width;
            this.Height = height;
            this.Mode = mode;
        }

        /// <summary>
        /// 
        /// </summary>
        public CroppingMode Mode { get; }

        /// <summary>
        /// 长
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 宽
        /// </summary>
        public int Height { get; }
    }
}
