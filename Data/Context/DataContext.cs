using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("connection"));
        }

        public DbSet<AcaoUsuario> AcaoUsuario { get; set; }
        public DbSet<AnexoResposta> AnexoResposta { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
