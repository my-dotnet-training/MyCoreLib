using EF.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EF.Data
{
    public class BookMap : EntityTypeConfiguration<Book>
    {
        public BookMap()
        {
            //配置主键
            this.HasKey(s => s.ID);
            //配置字段
            this.Property(s => s.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(s => s.BookName).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            // this.Property(s => s.BookAuthor).HasColumnType("nvarchar(50)").IsRequired();//注意这个和BookName字段配置的区别之处:这样写EF生成不了数据库
            this.Property(s => s.BookAuthor).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            this.Property(s => s.BookPrice).IsRequired();
            this.Property(s => s.AddedDate).IsRequired();
            this.Property(s => s.ModifiedDate).IsRequired();
            this.Property(s => s.PublisherId).IsRequired();

            //配置关系[一个出版商可以出版很多书籍]【外键单独配置，不是必须在Property中配置，当然也可以在Property中配置】
            this.HasRequired(s => s.Publisher).WithMany(s => s.Books).HasForeignKey(s => s.PublisherId).WillCascadeOnDelete(true);

            //配置表名字
            this.ToTable("Books");


        }
    }
}
