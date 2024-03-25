using Application.Implementation.Repositories;
using Application.Implementation.Services;
using Application.Interface.Repositories;
using Application.Interface.Services;
using AutoMapper;
using Domain.Entities;
using Domain.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace APISunSale.Startup
{
    public class Start
    {
        private WebApplicationBuilder _app;

        public Start(WebApplicationBuilder app) 
        { 
            this._app = app;
        }

        public Start Builder()
        {
            // Services
            _app.Services.AddScoped<IAcaoUsuarioService, AcaoUsuarioService>();
            _app.Services.AddScoped<IAnexoRespostaService, AnexoRespostaService>();
            _app.Services.AddScoped<IUsuariosService, UsuariosService>();
            _app.Services.AddScoped<IAnexosQuestoesService, AnexosQuestoesService>();
            _app.Services.AddScoped<IProvaService, ProvaService>();
            _app.Services.AddScoped<IQuestoesService, QuestoesService>();
            _app.Services.AddScoped<IRespostasQuestoesService, RespostasQuestoesService>();
            _app.Services.AddScoped<IRespostasQuestoesService, RespostasQuestoesService>();
            _app.Services.AddScoped<IPessoasForDevService, PessoasForDevService>();
            _app.Services.AddScoped<IEmpresaForDevService, EmpresaForDevService>();
            _app.Services.AddScoped<ICartaoCreditoDevToolsService, CartaoCreditoDevToolsService>();
            _app.Services.AddScoped<IVeiculosForDevService, VeiculosForDevService>();
            _app.Services.AddScoped<IRespostasUsuariosService, RespostasUsuariosService>();
            _app.Services.AddScoped<ICrudFormsInstaladorService, CrudFormsInstaladorService>();
            _app.Services.AddScoped<IEmailService, EmailService>();
            _app.Services.AddScoped<IResultadosTabuadaDivertidaService, ResultadosTabuadaDivertidaService>();
            _app.Services.AddScoped<IRecuperaSenhaService, RecuperaSenhaService>();
            _app.Services.AddScoped<ILoggerService, LoggerService>();
            _app.Services.AddScoped<IImageMagicService, ImageMagicService>();
            _app.Services.AddScoped<IEstagioService, EstagioService>();
            _app.Services.AddScoped<ISimuladoService, SimuladosService>();
            _app.Services.AddScoped<IUsuariosCrudFormsService, UsuariosCrudFormsService>();
            _app.Services.AddScoped<ILicencasSunSaleProService, LicencasSunSaleProService>();
            _app.Services.AddScoped<IComentariosQuestoesService, ComentariosQuestoesService>();
            _app.Services.AddScoped<ITipoProvaService, TipoProvaService>();
            _app.Services.AddScoped<ITipoProvaAssociadoService, TipoProvaAssociadoService>();
            _app.Services.AddScoped<IVerificacaoUsuarioService, VerificacaoUsuarioService>();
            _app.Services.AddScoped<IAlimentosService, AlimentosService>();
            _app.Services.AddScoped<ICategoriaAlimentosService, CategoriaAlimentosService>();
            _app.Services.AddScoped<IMetasService, MetasService>();
            _app.Services.AddScoped<INotasCorteSisuService, NotasCorteSisuService>();
            _app.Services.AddScoped<IPesosService, PesosService>();
            _app.Services.AddScoped<IAdminService, AdminService>();
            _app.Services.AddScoped<ITipoPerfilService, TipoPerfilService>();
            _app.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
            _app.Services.AddScoped<IQuestoesAvaliacaoService, QuestoesAvaliacaoService>();
            _app.Services.AddScoped<IRespostasAvaliacoesService, RespostasAvaliacoesService>();

            // Repositories
            _app.Services.AddScoped<IAcaoUsuarioRepository, AcaoUsuarioRepository>();
            _app.Services.AddScoped<IAnexoRespostaRepository, AnexoRespostaRepository>();
            _app.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
            _app.Services.AddScoped<IAnexosQuestoesRepository, AnexosQuestoesRepository>();
            _app.Services.AddScoped<ICodigosTableRepository, CodigosTableRepository>();
            _app.Services.AddScoped<IProvaRepository, ProvaRepository>();
            _app.Services.AddScoped<IQuestoesRepository, QuestoesRepository>();
            _app.Services.AddScoped<IRespostasQuestoesRepository, RespostasQuestoesRepository>();
            _app.Services.AddScoped<IPessoasForDevRepository, PessoasForDevRepository>();
            _app.Services.AddScoped<IEmpresaForDevRepository, EmpresaForDevRepository>();
            _app.Services.AddScoped<ICartaoCreditoDevToolsRepository, CartaoCreditoDevtoolsRepository>();
            _app.Services.AddScoped<IVeiculosForDevRepository, VeiculosForDevRepository>();
            _app.Services.AddScoped<IRespostasUsuariosRepository, RespostasUsuariosRepository>();
            _app.Services.AddScoped<ICrudFormsInstaladorRepository, CrudFormsInstaladorRepository>();
            _app.Services.AddScoped<IEmailRepository, EmailRepository>();
            _app.Services.AddScoped<IResultadosTabuadaDivertidaRepository, ResultadosTabuadaDivertidaRepository>();
            _app.Services.AddScoped<IRecuperaSenhaRepository, RecuperaSenhaRepository>();
            _app.Services.AddScoped<ILoggerRepository, LoggerRepository>();
            _app.Services.AddScoped<ISimuladosRepository, SimuladoRepository>();
            _app.Services.AddScoped<IUsuariosCrudFormsRepository, UsuariosCrudFormsRepository>();
            _app.Services.AddScoped<ILicencasSunSaleProRepository, LicencasSunSaleProRepository>();
            _app.Services.AddScoped<IComentariosQuestoesRepository, ComentariosQuestoesRepository>();
            _app.Services.AddScoped<ITipoProvaRepository, TipoProvaRepository>();
            _app.Services.AddScoped<ITipoProvaAssociadoRepository, TipoProvaAssociadoRepository>();
            _app.Services.AddScoped<IVerificacaoUsuarioRepository, VerificacaoUsuarioRepository>();
            _app.Services.AddScoped<IAlimentosRepository, AlimentosRepository>();
            _app.Services.AddScoped<ICategoriaAlimentosRepository, CategoriaAlimentosRepository>();
            _app.Services.AddScoped<IMetasRepository, MetasRepository>();
            _app.Services.AddScoped<INotasCorteSisuRepository, NotasCorteSisuRepository>();
            _app.Services.AddScoped<IPesosRepository, PesosRepository>();
            _app.Services.AddScoped<IAdminRepository, AdminRepository>();
            _app.Services.AddScoped<ITipoPerfilRepository, TipoPerfilRepository>();
            _app.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
            _app.Services.AddScoped<IQuestoesAvaliacaoRepository, QuestoesAvaliacaoRepository>();
            _app.Services.AddScoped<IRespostasAvaliacoesRepository, RespostasAvaliacoesRepository>();

            Mapping();

            AddAuthentication();

            // Add services to the container.
            _app.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _app.Services.AddEndpointsApiExplorer();

            AddSwagger(_app.Environment.IsDevelopment());

            _app.Services.AddHttpContextAccessor();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _app.Services.AddSingleton(configuration);
            _app.Services.Configure<EmailSettings>(configuration.GetSection("Email"));

            return this;
        }

        private void Mapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<string, byte[]>().ConvertUsing<Base64Converter>();
                cfg.CreateMap<byte[], string>().ConvertUsing<Base64Converter>();

                cfg.CreateMap<AcaoUsuarioViewModel, AcaoUsuario>();
                cfg.CreateMap<AcaoUsuario, AcaoUsuarioViewModel>();

                cfg.CreateMap<AnexoRespostaViewModel, AnexoResposta>();
                cfg.CreateMap<AnexoResposta, AnexoRespostaViewModel>();

                cfg.CreateMap<UsuariosViewModel, Usuarios>();
                cfg.CreateMap<Usuarios, UsuariosViewModel>();

                cfg.CreateMap<AnexosQuestoesViewModel, AnexosQuestoes>();
                cfg.CreateMap<AnexosQuestoes, AnexosQuestoesViewModel>();

                cfg.CreateMap<ProvaViewModel, Prova>();
                cfg.CreateMap<Prova, ProvaViewModel>();

                cfg.CreateMap<QuestoesViewModel, Questoes>();
                cfg.CreateMap<Questoes, QuestoesViewModel>();

                cfg.CreateMap<RespostasQuestoesViewModel, RespostasQuestoes>();
                cfg.CreateMap<RespostasQuestoes, RespostasQuestoesViewModel>();

                cfg.CreateMap<PessoasForDevViewModel, PessoasForDev>();
                cfg.CreateMap<PessoasForDev, PessoasForDevViewModel>();

                cfg.CreateMap<EmpresaForDevViewModel, EmpresaForDev>();
                cfg.CreateMap<EmpresaForDev, EmpresaForDevViewModel>();

                cfg.CreateMap<CartaoCreditoDevToolsViewModel, CartaoCreditoDevTools>();
                cfg.CreateMap<CartaoCreditoDevTools, CartaoCreditoDevToolsViewModel>();

                cfg.CreateMap<VeiculosForDevViewModel, VeiculosForDev>();
                cfg.CreateMap<VeiculosForDev, VeiculosForDevViewModel>();

                cfg.CreateMap<RespostasUsuariosViewModel, RespostasUsuarios>();
                cfg.CreateMap<RespostasUsuarios, RespostasUsuariosViewModel>();

                cfg.CreateMap<CrudFormsInstaladorViewModel, CrudFormsInstalador>();
                cfg.CreateMap<CrudFormsInstalador, CrudFormsInstaladorViewModel>();

                cfg.CreateMap<EmailViewModel, Email>();
                cfg.CreateMap<Email, EmailViewModel>();

                cfg.CreateMap<ResultadosTabuadaDivertidaViewModel, ResultadosTabuadaDivertida>();
                cfg.CreateMap<ResultadosTabuadaDivertida, ResultadosTabuadaDivertidaViewModel>();

                cfg.CreateMap<RecuperaSenhaViewModel, RecuperaSenha>();
                cfg.CreateMap<RecuperaSenha, RecuperaSenhaViewModel>();

                cfg.CreateMap<LoggerViewModel, Logger>();
                cfg.CreateMap<Logger, LoggerViewModel>();

                cfg.CreateMap<SimuladosViewModel, Simulados>();
                cfg.CreateMap<Simulados, SimuladosViewModel>();

                cfg.CreateMap<UsuariosCrudFormsViewModel, UsuariosCrudForms>();
                cfg.CreateMap<UsuariosCrudForms, UsuariosCrudFormsViewModel>();

                cfg.CreateMap<LicencasSunSaleProViewModel, LicencasSunSalePro>();
                cfg.CreateMap<LicencasSunSalePro, LicencasSunSaleProViewModel>();

                cfg.CreateMap<ComentariosQuestoesViewModel, ComentariosQuestoes>();
                cfg.CreateMap<ComentariosQuestoes, ComentariosQuestoesViewModel>();

                cfg.CreateMap<TipoProvaViewModel, TipoProva>();
                cfg.CreateMap<TipoProva, TipoProvaViewModel>();

                cfg.CreateMap<TipoProvaAssociadoViewModel, TipoProvaAssociado>();
                cfg.CreateMap<TipoProvaAssociado, TipoProvaAssociadoViewModel>();

                cfg.CreateMap<VerificacaoUsuarioViewModel, VerificacaoUsuario>();
                cfg.CreateMap<VerificacaoUsuario, VerificacaoUsuarioViewModel>();
                
                cfg.CreateMap<CategoriaAlimentosViewModel, CategoriaAlimentos>();
                cfg.CreateMap<CategoriaAlimentos, CategoriaAlimentosViewModel>();

                cfg.CreateMap<AlimentosViewModel, Alimentos>();
                cfg.CreateMap<Alimentos, AlimentosViewModel>();

                cfg.CreateMap<MetasViewModel, Metas>();
                cfg.CreateMap<Metas, MetasViewModel>();

                cfg.CreateMap<NotasCorteSisuViewModel, NotasCorteSisu>();
                cfg.CreateMap<NotasCorteSisu, NotasCorteSisuViewModel>();

                cfg.CreateMap<PesosViewModel, Pesos>();
                cfg.CreateMap<Pesos, PesosViewModel>();

                cfg.CreateMap<AdminDataViewModel, AdminData>();
                cfg.CreateMap<AdminData, AdminDataViewModel>();

                cfg.CreateMap<AdminUsuariosDateViewModel, AdminUsuariosDate>();
                cfg.CreateMap<AdminUsuariosDate, AdminUsuariosDateViewModel>();

                cfg.CreateMap<TipoPerfilViewModel, TipoPerfil>();
                cfg.CreateMap<TipoPerfil, TipoPerfilViewModel>();

                cfg.CreateMap<AvaliacaoViewModel, Avaliacao>();
                cfg.CreateMap<Avaliacao, AvaliacaoViewModel>();

                cfg.CreateMap<QuestoesAvaliacaoViewModel, QuestoesAvaliacao>();
                cfg.CreateMap<QuestoesAvaliacao, QuestoesAvaliacaoViewModel>();

                cfg.CreateMap<UsuariosResumedViewModel, Usuarios>();
                cfg.CreateMap<Usuarios, UsuariosResumedViewModel>();

                cfg.CreateMap<RespostasAvaliacoesViewModel, RespostasAvaliacoes>();
                cfg.CreateMap<RespostasAvaliacoes, RespostasAvaliacoesViewModel>();

                cfg.CreateMap<StringPlusIntViewModel, StringPlusInt>();
                cfg.CreateMap<StringPlusInt, StringPlusIntViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            _app.Services.AddSingleton(mapper);
        }

        private void AddAuthentication()
        {
            _app.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _app.Configuration["Jwt:Issuer"],
                    ValidAudience = _app.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_app.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            _app.Services.AddAuthorization();
        }

        private void AddSwagger(bool dev)
        {
            _app.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "SunSale API", 
                    Version = "v1", 
                    Contact= new OpenApiContact() 
                    { 
                        Email = "sunsalesystem@gmail.com" , 
                        Name = "SunSale System", 
                        Url= new Uri("http://sunsalesystem.com.br")
                    },
                    Description = ""
                });

                if (!dev)
                {
                    c.DocumentFilter<SwaggerControllerOrderProd>();
                }
                else
                {
                    c.DocumentFilter<SwaggerControllerOrder>();
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                        Description = "Enter the Bearer Authorization string as following: `Token`"
                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                        }
                    });
                }
            });
        }
    }

    public class Base64Converter : ITypeConverter<string, byte[]>, ITypeConverter<byte[], string>
    {
        public byte[] Convert(string source, byte[] destination, ResolutionContext context)
            => Encoding.UTF8.GetBytes(source);

        public string Convert(byte[] source, string destination, ResolutionContext context)
            => Encoding.UTF8.GetString(source);
    }
}
