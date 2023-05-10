using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniManagement.DAL.Data;
using UniManagement.DAL.Repositories;
using UniManagement.WebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
string connStr = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSqlServer<UniDbContext>(connStr);
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UniDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

builder.Services.AddSingleton<Mapper>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 7 Web API",
        Description = "Authentication and Authorization in ASP.NET 7 with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET 7 Web API v1"));
}

using(var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetService<UniDbContext>();
    ctx.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


/*
* NUGET
* 
* Nella DAL:
* Installare Microsoft.EntityFrameworkCore.Tools v6.0.15
* Installare Microsoft.EntityFrameworkCore.SqlServer v6.0.15
* 
* Nella WebApi:
* 
* Installare Microsoft.EntityFrameworkCore.Design v6.0.15
* Installare Microsoft.EntityFrameworkCore.SqlServer v6.0.15
* 
* ------------------------------------------------------------------
* 
* CLASSI IN DAL
* 
* Creare il nuovo LocaleDbContext che eredita da DbContext
* Creare i due costruttori standard: 
*      vuoto e con il parametro DbContextOptions tipizzato sulla classe attuale
* 
* Creare le classi relative alle entità (tabelle) del database con le relative relazioni
* In ogni classe prevedere un campo Id costruito secondo lo schema "ClassNameId" (Es: StudentId)
* In ogni classe con una relazione 1 a n, inserire anche il campo Id della foreign key (Es: CourseId in Student)
* Aggiungere ogni entità così creata come DbSet al DbContext
* 
* CLASSI IN WEBAPI
* 
* Inserire la connection string nel file appsettings.json
* Es:
*   "ConnectionStrings": {
        "Default": "Server=.;Database=UniDbWeb;Integrated Security=SSPI;TrustServerCertificate=True"
    },
* 
* Inserire nella program la dependency injection del sqlserver
* Es:
*   string connStr = builder.Configuration.GetConnectionString("Default");
    builder.Services.AddSqlServer<UniDbContext>(connStr);
* 
* -------------------------------------------------------------------------------------
* 
* MIGRATIONS & DB
* Dal Package Manager Console (Tools => NuGet Package Manager => Package Manager Console)
* Creare la prima migrazione con il comando "add-migration {NAME}", assegnare a {NAME} un nome sensato
* Applicare le modifiche al database, così creandolo, con il comando "update-database"
* 
* -------------------------------------------------------------------------------------
* 
* MAPPER
* 
* Creare una classe NON statica che contenga tutti i metodi di mappatura tra entità e modelli
* Aggiungere la classe mapper come Singleton per la dependency injection
* Es:
*   builder.Services.AddSingleton<Mapper>();
* 
*/