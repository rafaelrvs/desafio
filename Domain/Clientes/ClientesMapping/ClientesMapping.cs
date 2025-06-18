using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
                    // Seu DbContext
using desafiocs.Domain.Clientes.Request;     // AddClienteRequest, AddContatoRequest, AddEnderecoRequest
using desafiocs.Clientes;                    // Cliente, Contato, Endereco
using System.Linq;

namespace desafiocs.Clientes
{
    public static class ClientesMapping
    {
        public static void AddRotasClientes(this WebApplication app)
        {
            var rotas = app.MapGroup("/clientes");

            // POST /clientes
            rotas.MapPost("", async (AddClienteRequest request, AppDbContext context) =>
            {
                // Mapeia DTO de contato para entidade
                // var contatos = request.Contatos
                //     .Select(c => new Contato
                //     {
                //         Tipo = c.Tipo,
                //         Texto = c.Texto
                //     })
                //     .ToList();

                // // Mapeia DTO de endereço para entidade
                // var enderecos = request.Enderecos
                //     .Select(e => new Endereco
                //     {
                //         Cep = e.Cep,
                //         Logradouro = e.Logradouro,
                //         Cidade = e.Cidade,
                //         Numero = e.Numero,
                //         Complemento = e.Complemento
                //     })
                //     .ToList();

                // Cria entidade de domínio incluindo coleções

                var cliente = new Cliente(request.Nome);

          
              await context.Clientes.AddAsync(cliente);
              
              var a =    await context.SaveChangesAsync();

                return Results.Created($"/clientes", cliente);
            });
        }
    }
}
