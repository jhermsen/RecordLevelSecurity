using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading;
using Microsoft.AspNetCore.Http;
using RLS.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace RLS.Extensions
{
    public class SqlServerConnectionRLS : SqlServerConnection
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<ApplicationUser> _userManager;
        public SqlServerConnectionRLS(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IDbContextOptions options, ILogger<SqlServerConnection> logger) : base(options, logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public override void Open()
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(user);

            base.Open();

            if (userId != null)
            {
                DbCommand cmd = this.DbConnection.CreateCommand();
                cmd.CommandText = "EXEC sp_set_session_context @key=N'UserId', @value=@UserId";
                DbParameter param = cmd.CreateParameter();
                param.ParameterName = "@UserId";
                param.Value = userId;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
        }

        public override async Task OpenAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var user = _httpContextAccessor.HttpContext.User;
            await base.OpenAsync(cancellationToken);
            var userId = _userManager.GetUserId(user);

            if (userId != null)
            {
                DbCommand cmd = this.DbConnection.CreateCommand();
                cmd.CommandText = "EXEC sp_set_session_context @key=N'UserId', @value=@UserId";
                DbParameter param = cmd.CreateParameter();
                param.ParameterName = "@UserId";
                param.Value = userId;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
