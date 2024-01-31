using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MachineTst.Model
{
    public partial class TestMachineContext : DbContext
    {
        internal readonly object EmployeeTest;

        public TestMachineContext()
        {
        }

        public TestMachineContext(DbContextOptions<TestMachineContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmployeeTest> EmployeeTests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=SHREE-PC\\SQLEXPRESS;Initial Catalog=TestMachine;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeTest>(entity =>
            {
                entity.ToTable("EmployeeTest");

                entity.Property(e => e.EmployeeSalary).HasColumnName("Employee_Salary");

                entity.Property(e => e.MangerId).HasColumnName("Manger_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
