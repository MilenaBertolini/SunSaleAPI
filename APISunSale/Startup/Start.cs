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

            // Repositories
            _app.Services.AddScoped<IAcaoUsuarioRepository, AcaoUsuarioRepository>();
            _app.Services.AddScoped<IAnexoRespostaRepository, AnexoRespostaRepository>();
            _app.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

            Mapping();

            AddAuthentication();

            // Add services to the container.
            _app.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _app.Services.AddEndpointsApiExplorer();

            AddSwagger();

            return this;
        }

        private void Mapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AcaoUsuarioViewModel, AcaoUsuario>();
                cfg.CreateMap<AcaoUsuario, AcaoUsuarioViewModel>();

                cfg.CreateMap<AnexoRespostaViewModel, AnexoResposta>();
                cfg.CreateMap<AnexoResposta, AnexoRespostaViewModel>();

                cfg.CreateMap<UsuariosViewModel, Usuarios>();
                cfg.CreateMap<Usuarios, UsuariosViewModel>();
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
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_app.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            _app.Services.AddAuthorization();
        }

        private void AddSwagger()
        {
            _app.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SunSale API", Version = "v1" });
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
            });
        }
    }
}
