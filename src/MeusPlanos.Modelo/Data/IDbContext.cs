using System;
using System.Threading;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MeusPlanos.Modelo.Data;

public interface IDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    EntityEntry Attach(object entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    ChangeTracker ChangeTracker { get; }

    public DatabaseFacade Database { get; }

    public DbSet<Papel> Papeis { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}