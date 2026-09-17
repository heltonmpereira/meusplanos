using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MeusPlanos.Definicao.Entidade;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MeusPlanos.Modelo.Data
{
    public class MeusPlanosContext(DbContextOptions options) : DbContext(options), IDbContext
    {
        private void ValidarEntidades()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added ||
                                 e.State == EntityState.Modified
                           select e.Entity;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(
                    entity,
                    validationContext,
                    true);
            }
        }
        private IEnumerable<EntityEntry> EntradasComPropriedade(
            IEnumerable<string> nomes, EntityState estado)
        {
            var entries = ChangeTracker.Entries()
                .Where(w => w.State == estado && w.Entity.GetType()
                    .GetProperties()
                    .Any(a => nomes.Contains(a.Name)))
                .ToList();

            return entries;
        }
        private static PropertyEntry ObterPropriedade(EntityEntry obj,
            IEnumerable<string> name)
        {
            foreach (var nome in name)
            {
                try
                {
                    return obj.Property(nome);
                }
                catch
                {
                    //nada por aqui...
                }
            }

            return null;
        }
        private void PreparaCampos()
        {
            var camposCadastro = new[] { "DataCadastro", "Data_Cadastro", "DataCriacao" };

            var entries = EntradasComPropriedade(camposCadastro, EntityState.Added);
            foreach (var entry in entries)
            {
                var campo = ObterPropriedade(entry, camposCadastro);
                if (campo == null)
                    break;

                campo.CurrentValue = DateTime.Now;
            }

            var camposAtualizacao = new[] { "DataAtualizacao", "Data_Atualizacao", "DataAlteracao" };
            var entriesAlteradas = EntradasComPropriedade(camposAtualizacao, EntityState.Modified);
            foreach (var entry in entriesAlteradas)
            {
                var campo = ObterPropriedade(entry, camposAtualizacao);
                if (campo == null)
                    break;

                campo.CurrentValue = DateTime.Now;
            }

            var entriesCadastros = EntradasComPropriedade(camposCadastro, EntityState.Modified);
            foreach (var entry in entriesCadastros)
            {
                var campo = ObterPropriedade(entry, camposCadastro);
                if (campo == null)
                    break;

                campo.CurrentValue = campo.OriginalValue;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(message => Debug.WriteLine(message))
                .EnableDetailedErrors();
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif


            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
#if DEBUG
            foreach (var entry in ChangeTracker.Entries())
                Debug.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
#endif

            ValidarEntidades();
            PreparaCampos();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Papel> Papeis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}