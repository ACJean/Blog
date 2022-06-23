using AutoMapper;
using Blog.Service.AMProfile;
using Blog.Service.Post;
using Blog.Service.Security;
using Blog.Util;
using Lavel.STD.Entities.NoSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var blogConfigSection = builder.Configuration.GetSection("BlogConfig");
var blogConfig = blogConfigSection.Get<BlogConfig>();
builder.Services.Configure<BlogConfig>(blogConfigSection);

builder.Services.AddCors();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new CamelCaseNamingStrategy
        {
            OverrideSpecifiedNames = false
        }
    };
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
});

builder.Services.AddEndpointsApiExplorer();

var key = Encoding.UTF8.GetBytes(blogConfig.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = "Blog API",
        Description = ""
    });

    //Autenticación por Bearer token
    OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Inserte el token de autorización.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    };
    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement() { { securityScheme, new string[] { } } };

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDefinition);
    options.AddSecurityRequirement(securityRequirements);
});

var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton<INoSQLClient, MongoDBClient>(i => new MongoDBClient(new MongoClient($"mongodb://localhost:27017")));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

var acceptedUrls = blogConfig.AllowedOrigins.Split(",");
app.UseCors(builder => builder.WithOrigins(acceptedUrls).AllowAnyHeader().AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog.Api");
    });
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
