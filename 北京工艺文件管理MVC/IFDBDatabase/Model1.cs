namespace 北京工艺文件管理MVC.IFDBDatabase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=IFDbModel")
        {
        }

        public virtual DbSet<jdxa_file_info> jdxa_file_info { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //}
    }
}
