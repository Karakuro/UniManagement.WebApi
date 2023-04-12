using Microsoft.EntityFrameworkCore;
using UniManagement.DAL.Data;
using UniManagement.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string connStr = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSqlServer<UniDbContext>(connStr);
builder.Services.AddSingleton<Mapper>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using(var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetService<UniDbContext>();
    ctx.Database.Migrate();
}

app.UseHttpsRedirection();

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