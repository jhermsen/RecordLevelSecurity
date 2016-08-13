using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLS.Extensions
{
    public static class SqlServerDbContextOptionsExtensionsRLS
    {
        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database.
        /// </summary>
        /// <param name="optionsBuilder"> The builder being used to configure the context. </param>
        /// <param name="connectionString"> The connection string of the database to connect to. </param>
        /// <param name="sqlServerOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseSqlServerRLS(
            this DbContextOptionsBuilder optionsBuilder,
            string connectionString,
            Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
        {
            var extension = GetOrCreateExtension(optionsBuilder);
            extension.ConnectionString = connectionString;
            ((IDbContextOptionsBuilderInfrastructure) optionsBuilder).AddOrUpdateExtension(extension);

            ConfigureWarnings(optionsBuilder);

            sqlServerOptionsAction?.Invoke(new SqlServerDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        
        private static SqlServerOptionsRLSExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
        {
            var existing = optionsBuilder.Options.FindExtension<SqlServerOptionsRLSExtension>();
            return existing != null
                ? new SqlServerOptionsRLSExtension(existing)
                : new SqlServerOptionsRLSExtension();
        }

        private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
        {
            // Set warnings defaults
            optionsBuilder.ConfigureWarnings(w =>
            {
                w.Configuration.TryAddExplicit(
                    RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw);
            });
        }
    }
}
