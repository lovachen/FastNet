using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AiBao.Mapping
{
    [Serializable]
    public class Sys_UserJwtMapping
    {
        [StringLength(64)]
        public string Jti { get; set; }

        [Required]
        [StringLength(64)]
        public string RefreshToken { get; set; }

        public Guid UserId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Expiration { get; set; }

        public int Platform { get; set; }

        #region

        /// <summary>
        /// 
        /// </summary>
        public string PlatformName => Platform == 0 ? "PC端" : Platform == 1 ? "App" : "其它";

        /// <summary>
        /// 
        /// </summary>
        public string ExpirationForamt => Expiration.ToString("F");

        #endregion

    }
}
