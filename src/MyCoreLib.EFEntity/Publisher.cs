using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Entity
{
    public class Publisher : BaseEntity
    {
        /// <summary>
        /// 出版商的名字
        /// </summary>
        [DisplayName("出版商的名字"), Required]
        public string PublisherName { get; set; }

        /// <summary>
        /// 导航属性
        /// </summary>
        public virtual ICollection<Book> Books { get; set; }
    }
}
