using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Core.Domain.Entities;

namespace RegistroDeAtendimento.Infrastructure.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options){
    public DbSet<Paciente> Pacientes{ get; set; }
    public DbSet<Atendimento> Atendimentos{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Paciente>(entity => {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.DataNascimento)
                .HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v))
                .HasColumnType("date")
                .IsRequired();

            entity.Property(p => p.Sexo)
                .IsRequired();

            entity.Property(p => p.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(p => p.Status)
                .IsRequired();
        });

        modelBuilder.Entity<Paciente>().OwnsOne(p => p.Endereco, endereco => {
            endereco.Property(e => e.Cep).HasColumnName("Cep");
            endereco.Property(e => e.Cidade).HasColumnName("Cidade");
            endereco.Property(e => e.Bairro).HasColumnName("Bairro");
            endereco.Property(e => e.Logradouro).HasColumnName("Logradouro");
            endereco.Property(e => e.Complemento).HasColumnName("Complemento").IsRequired(false);
        });

        modelBuilder.Entity<Atendimento>(entity => {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.DataHora)
                .IsRequired();

            entity.Property(a => a.Descricao)
                .IsRequired();

            entity.HasOne(a => a.Paciente)
                .WithMany(p => p.Atendimentos)
                .HasForeignKey(a => a.PacienteId);
        });

        base.OnModelCreating(modelBuilder);
    }
}