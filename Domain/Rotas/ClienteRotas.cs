
namespace Desafios.Domain.Rotas;

using desafiocs.Domain.Cliente;
using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;
public static class ClienteRotas
{


    public static void AddRotasClientes(this WebApplication app)
    {
        app.MapGet("consulta", async (AppDbContext context) =>
                   {
                       var lista = await context.Cliente
                           .Include(c => c.Endereco)
                           .Include(c => c.Contato)
                           .ToListAsync();

                       return Results.Ok(lista);
                   });

        app.MapPost("registrar", async (AddClienteRequest request, AppDbContext context) =>
        {
            var novoCliente = new Cliente(request.Nome)
            {
                Endereco = new ClienteEndereco(),
                Contato = new ClienteContato
                {
                    Tipo = request.Tipo,
                    Texto = request.Texto
                }
            };

            var endereco = novoCliente.Endereco!;
            await endereco.BuscarEnderecoPorCepAsync(request.Cep);
            endereco.Numero = request.Numero;
            endereco.Complemento = request.Complemento;


            await context.Cliente.AddAsync(novoCliente);
            await context.SaveChangesAsync();
            return "Cadastro realizado com sucesso";
        });
    }
}