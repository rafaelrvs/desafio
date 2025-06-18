
namespace Desafios.Domain.Rotas;

using desafiocs.Domain.Cliente;
using Desafios.Domain.Cliente;

public static class ClienteRotas
{


    public static void AddRotasClientes(this WebApplication app)
    {
        app.MapGet("consulta", () =>  new Cliente("rafael"));


        app.MapPost("registrar", async (AddClienteRequest request,AppDbContext context)=>
        {
            var novoCliente = new Cliente(request.Nome);
            var endereco = new ClienteEndereco();

             await endereco.BuscarEnderecoPorCepAsync(request.Cep);
            Console.WriteLine(endereco.Logradouro);

            // await context.Cliente.AddAsync(novoCliente);
        
            // await context.SaveChangesAsync();
        } );
 }   
}