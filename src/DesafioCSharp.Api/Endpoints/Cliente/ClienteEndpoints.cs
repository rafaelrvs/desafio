
namespace Usecase.EndpointExtensions;

using desafiocs;
using desafiocs.Domain.Cliente;
using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;
public static class ClienteRotas
{


    public static void AddRotasClientes(this WebApplication app)
    {
        app.MapPost("registrar", async (AddClienteRequest request, AppDbContext context) =>
        {

            var textoJaExiste = await context.Cliente
       .AnyAsync(c => c.Contato != null && c.Contato.Texto == request.Texto);
            if (textoJaExiste)
            {
                return Results.Conflict(new
                {
                    Mensagem = $"Já existe um cliente com o texto de contato '{request.Texto}'."
                });
            }

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


            var criadoDto = new dbClienteDto
            {
                Id = novoCliente.Id,
                Nome = novoCliente.Nome,
                DataCadastro = novoCliente.DataCadastro,
                Endereco = new EnderecoDto
                {
                    Logradouro = endereco.Logradouro,
                    Cidade = endereco.Cidade,
                    Cep = endereco.Cep,
                    Numero = endereco.Numero,
                    Complemento = endereco.Complemento
                },
                Contato = new ContatoDto
                {
                    Id = novoCliente.Id,
                    Tipo = novoCliente.Contato!.Tipo,
                    Texto = novoCliente.Contato.Texto
                }
            };

            return Results.Created($"/consulta/{novoCliente.Id}", criadoDto);
        });

        app.MapList<Cliente, dbClienteDto>(
     "consulta",

     Cliente => Cliente
         .Include(c => c.Endereco)
         .Include(c => c.Contato)
         .Select(c => new dbClienteDto
         {
             Id = c.Id,
             Nome = c.Nome,
             DataCadastro = c.DataCadastro,
             Endereco = c.Endereco == null
                            ? null
                            : new EnderecoDto
                            {
                                Cep = c.Endereco.Cep,
                                Logradouro = c.Endereco.Logradouro,
                                Cidade = c.Endereco.Cidade,
                                Numero = c.Endereco.Numero,
                                Complemento = c.Endereco.Complemento
                            },
             Contato = c.Contato == null
                            ? null
                            : new ContatoDto
                            {
                                Id = c.Id,
                                Tipo = c.Contato.Tipo,
                                Texto = c.Contato.Texto
                            }
         })
 );


        app.MapGetById<Cliente, dbClienteDto>(
                   "consulta/{id:int}",
                   (cliente, id) => cliente
                       .Include(c => c.Endereco)
                       .Include(c => c.Contato)
                       .Where(c => c.Id == id)
                       .Select(c => new dbClienteDto
                       {
                           Id = c.Id,
                           Nome = c.Nome,
                           DataCadastro = c.DataCadastro,
                           Endereco = c.Endereco == null ? null : new EnderecoDto {
                                Cep = c.Endereco.Cep,
                                Logradouro = c.Endereco.Logradouro,
                                Cidade = c.Endereco.Cidade,
                                Numero = c.Endereco.Numero,
                                Complemento = c.Endereco.Complemento
                            },
                           Contato = c.Contato == null ? null : new ContatoDto
                           {
                               Id = c.Contato.Id,
                               Tipo = c.Contato.Tipo,
                               Texto = c.Contato.Texto
                           }
                       })
               );



        app.MapPatch("altera/{id:int}", async (
    int id,
    UpdateClienteRequest req,
    AppDbContext ctx
) =>
{
    var useCase = new UpdateUseCase();

    return await useCase.AtualizarAsync<Cliente, dbClienteDto>(
        ctx,
        id,


        async cliente =>
        {

            await ctx.Entry(cliente).Reference(c => c.Endereco).LoadAsync();
            await ctx.Entry(cliente).Reference(c => c.Contato).LoadAsync();

            if (!string.IsNullOrWhiteSpace(req.Nome))
                cliente.AlterarNome(req.Nome);

            if (req.Cep is not null && cliente.Endereco is not null)
            {
                await cliente.Endereco.BuscarEnderecoPorCepAsync(req.Cep);
                if (req.Numero is not null) cliente.Endereco.Numero = req.Numero;
                if (req.Complemento is not null) cliente.Endereco.Complemento = req.Complemento;
            }

            if (req.Tipo is not null && req.Texto is not null)
            {
                if (cliente.Contato is null)
                    cliente.Contato = new ClienteContato { ClienteId = cliente.Id };

                cliente.Contato.Tipo = req.Tipo;
                cliente.Contato.Texto = req.Texto;
            }
        },


        cliente => new dbClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            DataCadastro = cliente.DataCadastro,
            Endereco = cliente.Endereco is null
                           ? null
                           : new EnderecoDto
                           {
                               Cep = cliente.Endereco.Cep,
                               Logradouro = cliente.Endereco.Logradouro,
                               Cidade = cliente.Endereco.Cidade,
                               Numero = cliente.Endereco.Numero,
                               Complemento = cliente.Endereco.Complemento
                           },
            Contato = cliente.Contato is null
                           ? null
                           : new ContatoDto
                           {
                               Tipo = cliente.Contato.Tipo,
                               Texto = cliente.Contato.Texto
                           }
        }
    );
});
        app.MapDelete("deletar/{id:int}", async (int id, AppDbContext context) =>
       {

           var useCase = new DeleteUseCase();
           return await useCase.DeletarAsync<Cliente>(context, id);
     
       });
    }
}