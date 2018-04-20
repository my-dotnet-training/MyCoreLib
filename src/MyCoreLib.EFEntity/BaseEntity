using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EF.Entity
{
    public abstract class BaseEntity
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DisplayName("数据ID")]
        public Guid ID { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("添加时间"), Required]
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        /// 
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("更新时间"), Required]
        public DateTime ModifiedDate { get; set; }
    }
}
