using GoCar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoCar.Infrastructure.Configurations
{
    public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.ToTable("Pagamentos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Tipo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.FormaPagamento)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.Valor)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.DataPagamento)
                .IsRequired(false);

            builder.Property(p => p.Observacoes)
                .HasMaxLength(500);

            builder.Property(p => p.IsAtivo)
                .IsRequired();

            builder.Property(p => p.DataCriacao)
                .IsRequired();

            // Locação → Pagamentos
            builder.HasOne(p => p.Locacao)
                .WithMany(l => l.Pagamentos)
                .HasForeignKey(p => p.LocacaoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}