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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoProvaAssociado>()
            .HasOne(tpa => tpa.TipoProva)
            .WithMany(tp => tp.TipoProvaAssociados)
            .HasForeignKey(tpa => tpa.CodigoTipo);
        }

        public DbSet<AcaoUsuario> AcaoUsuario { get; set; }
        public DbSet<AnexoResposta> AnexoResposta { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<AnexosQuestoes> AnexosQuestoes { get; set; }
        public DbSet<Codigos_Table> Codigos_Table { get; set; }
        public DbSet<Prova> Prova { get; set; }
        public DbSet<Questoes> Questoes { get; set; }
        public DbSet<RespostasQuestoes> RespostasQuestoes { get; set; }
        public DbSet<PessoasForDev> PessoasForDev { get; set; }
        public DbSet<EmpresaForDev> EmpresaForDev { get; set; }
        public DbSet<CartaoCreditoDevTools> CartaoCreditoDevTools { get; set; }
        public DbSet<VeiculosForDev> VeiculosForDev { get; set; }
        public DbSet<RespostasUsuarios> RespostasUsuarios { get; set; }
        public DbSet<CrudFormsInstalador> CrudFormsInstalador { get; set; }
        public DbSet<Email> Email { get; set; }
        public DbSet<ResultadosTabuadaDivertida> ResultadosTabuadaDivertida { get; set; }
        public DbSet<RecuperaSenha> RecuperaSenha { get; set; }
        public DbSet<Logger> Logger { get; set; }
        public DbSet<Simulados> Simulados { get; set; }
        public DbSet<UsuariosCrudForms> UsuariosCrudForms { get; set; }
        public DbSet<LicencasSunSalePro> LicencasSunSalePro { get; set; }
        public DbSet<ComentariosQuestoes> ComentariosQuestoes { get; set; }
        public DbSet<TipoProva> TipoProva { get; set; }
        public DbSet<TipoProvaAssociado> TipoProvaAssociado { get; set; }
        public DbSet<VerificacaoUsuario> VerificacaoUsuario { get; set; }
        public DbSet<Alimentos> Alimentos { get; set; }
        public DbSet<CategoriaAlimentos> CategoriaAlimentos { get; set; }
        public DbSet<Metas> Metas { get; set; }
        public DbSet<NotasCorteSisu> NotasCorteSisu { get; set; }
        public DbSet<Pesos> Pesos { get; set; }
        public DbSet<TipoPerfil> TipoPerfil { get; set; }
        public DbSet<Avaliacao> Avaliacao { get; set; }
        public DbSet<QuestoesAvaliacao> QuestoesAvaliacao { get; set; }
        public DbSet<RespostasAvaliacoes> RespostasAvaliacoes { get; set; }
        public DbSet<WhosThatPokemonResult> WhosThatPokemonResult { get; set; }
        public DbSet<Postagem> Postagem { get; set; }
        public DbSet<SavedResultsWpp> SavedResultsWpp { get; set; }
    }
}
