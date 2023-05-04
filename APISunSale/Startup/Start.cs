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

            Mapping();

            AddAuthentication();

            // Add services to the container.
            _app.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _app.Services.AddEndpointsApiExplorer();

            AddSwagger(_app.Environment.IsDevelopment());

            _app.Services.AddHttpContextAccessor();

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
