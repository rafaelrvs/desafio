

using System.Text.Json.Serialization;
using Desafios.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Usecase.EndpointExtensions;
using usecaseI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AppDbContext>();
builder.Services.AddScoped<IEnderecoService, ViaCepEnderecoService>();
builder.Services.AddScoped<ICreateClienteUseCase, CreateClienteUseCase>();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new() { Title = "MinhaAPI", Version = "v1" });
});


builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    opts.SerializerOptions.WriteIndented     = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
     app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MinhaAPI v1");
        });
{
    app.MapOpenApi();
}
app.AddRotasClientes();
app.UseHttpsRedirection();


app.Run();

