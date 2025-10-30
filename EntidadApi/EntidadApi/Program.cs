using Microsoft.Data.SqlClient;
using System.Data;
using EntidadApi.Data;

var builder = WebApplication.CreateBuilder(args);

var MiPoliticaCors = "AllowAngularApp";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MiPoliticaCors,
        policy =>
        {

            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IEntidadRepository, EntidadRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MiPoliticaCors);

app.UseAuthorization();

app.MapControllers();

app.Run();