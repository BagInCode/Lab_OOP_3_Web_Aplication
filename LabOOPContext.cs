using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LabIStTP
{
    public partial class LabOOPContext : DbContext
    {
        public LabOOPContext()
        {
        }

        public LabOOPContext(DbContextOptions<LabOOPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Вузы> Вузы { get; set; }
        public virtual DbSet<Группы> Группы { get; set; }
        public virtual DbSet<Задачи> Задачи { get; set; }
        public virtual DbSet<Пользователь> Пользователь { get; set; }
        public virtual DbSet<Преподаватели> Преподаватели { get; set; }
        public virtual DbSet<СтудентЗадача> СтудентЗадача { get; set; }
        public virtual DbSet<Студенты> Студенты { get; set; }
        public virtual DbSet<Сценарии> Сценарии { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-E6AFL5P\\SQLEXPRESS; Database=LabOOP; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Вузы>(entity =>
            {
                entity.ToTable("ВУЗы");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.НазваниеВуза)
                    .IsRequired()
                    .HasColumnName("Название ВУЗа")
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Группы>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ВузId).HasColumnName("ВУЗ_ID");

                entity.Property(e => e.Название)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(d => d.Вуз)
                    .WithMany(p => p.Группы)
                    .HasForeignKey(d => d.ВузId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Группы_ВУЗы");
            });

            modelBuilder.Entity<Задачи>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Дата).HasColumnType("datetime");

                entity.Property(e => e.Место)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Описание).HasMaxLength(250);

                entity.Property(e => e.ПользовательId).HasColumnName("Пользователь_ID");

                entity.Property(e => e.СценарийId).HasColumnName("Сценарий_ID");

                entity.HasOne(d => d.Пользователь)
                    .WithMany(p => p.Задачи)
                    .HasForeignKey(d => d.ПользовательId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Задачи_Пользователь");

                entity.HasOne(d => d.Сценарий)
                    .WithMany(p => p.Задачи)
                    .HasForeignKey(d => d.СценарийId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Задачи_Сценарии");
            });

            modelBuilder.Entity<Пользователь>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Фио)
                    .IsRequired()
                    .HasColumnName("ФИО")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Преподаватели>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ВузId).HasColumnName("ВУЗ_ID");

                entity.Property(e => e.Фио)
                    .IsRequired()
                    .HasColumnName("ФИО")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Вуз)
                    .WithMany(p => p.Преподаватели)
                    .HasForeignKey(d => d.ВузId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Преподаватели_ВУЗы");
            });

            modelBuilder.Entity<СтудентЗадача>(entity =>
            {
                entity.ToTable("Студент-Задача");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ЗадачаId).HasColumnName("Задача_ID");

                entity.Property(e => e.СтудентId).HasColumnName("Студент_ID");

                entity.HasOne(d => d.Задача)
                    .WithMany(p => p.СтудентЗадача)
                    .HasForeignKey(d => d.ЗадачаId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Студент-Задача_Задачи");

                entity.HasOne(d => d.Студент)
                    .WithMany(p => p.СтудентЗадача)
                    .HasForeignKey(d => d.СтудентId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Студент-Задача_Студенты");
            });

            modelBuilder.Entity<Студенты>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ГруппаId).HasColumnName("Группа_ID");

                entity.Property(e => e.Фио)
                    .IsRequired()
                    .HasColumnName("ФИО")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Группа)
                    .WithMany(p => p.Студенты)
                    .HasForeignKey(d => d.ГруппаId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Студенты_Группы");
            });

            modelBuilder.Entity<Сценарии>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.КВоАктёров).HasColumnName("К-во актёров");

                entity.Property(e => e.Описание)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
