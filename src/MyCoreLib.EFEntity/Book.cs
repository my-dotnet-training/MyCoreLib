using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entity
{
    /// <summary>
    /// Book实体
    /// </summary>
    public class Book : BaseEntity
    {
        /// <summary>
        /// 书名
        /// </summary>
        [DisplayName("书名"), Required]
        public string BookName { get; set; }

        /// <summary>
        /// 书的作者
        /// </summary>
        [DisplayName("书的作者"), Required]
        public string BookAuthor { get; set; }

        /// <summary>
        /// 书的价格
        /// </summary>
        [DisplayName("书的价格")]
        public decimal BookPrice { get; set; }

        /// <summary>
        /// 出版商编号
        /// </summary>
        [DisplayName("出版商编号")]
        public Guid PublisherId { get; set; }

        /// <summary>
        /// 导航属性---出版商
        /// </summary>
        public virtual Publisher Publisher { get; set; }
    }
}
