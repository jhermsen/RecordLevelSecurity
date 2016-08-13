using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace RLS.Extensions
{
    public class SqlServerOptionsRLSExtension : SqlServerOptionsExtension
    {
        public SqlServerOptionsRLSExtension():base()
        { }
        public SqlServerOptionsRLSExtension(SqlServerOptionsExtension copyFrom) : base(copyFrom)
        {
        }

        public override void ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServerRLS();
        }
    }
}
