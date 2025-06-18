

using Desafios.Domain.Rotas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<AppDbContext>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.AddRotasClientes();
app.UseHttpsRedirection();


app.Run();

