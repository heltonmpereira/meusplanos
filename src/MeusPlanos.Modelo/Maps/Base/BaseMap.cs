using MeusPlanos.Definicao.Interface.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeusPlanos.Modelo.Maps.Base;

public class BaseMap<T, TPK> : IEntityTypeConfiguration<T>
    where T : class, IEntidade<TPK>
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.ToTable(typeof(T).Name);
        builder.HasKey(k => k.Id);
    }
}