namespace TimeLink.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MyDataModel : DbContext
    {
        public MyDataModel()
            : base("name=MyDataModel")
        {
        }

        public virtual DbSet<T_ACCOUNT> T_ACCOUNT { get; set; }
        public virtual DbSet<T_GOAL> T_GOAL { get; set; }
        public virtual DbSet<T_TASK> T_TASK { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
