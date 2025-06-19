
using desafiocs;
using desafiocs.Domain.Cliente;
using Desafios.Application.Exceptions;
using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;

namespace Usecase.EndpointExtensions;
public static class ClienteRotas
{


    public static void AddRotasClientes(this WebApplication app)
    {
      app.MapPost("registrar", async (
    AddClienteRequest       request,
    ICreateClienteUseCase   createUseCase
) =>
{
    try
    {
        var dto = await createUseCase.ExecuteAsync(request);
        return Results.Created($"/consulta/{dto.Id}", dto);
    }
    catch (DuplicateContatoException ex)
    {
       
        return Results.Conflict(new { Mensagem = ex.Message });
    }
    catch (Exception ex)
    {
 
        return Results.Problem(
            detail: $"Ocorreu um erro ao criar o cliente: {ex.Message}"
        );
    }
});

        app.MapList<Cliente, ClienteDto >(
     "consulta",

     Cliente => Cliente
         .Include(c => c.Endereco)
         .Include(c => c.Contato)
         .Select(c => new ClienteDto 
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


        app.MapGetById<Cliente, ClienteDto >(
                   "consulta/{id:int}",
                   (cliente, id) => cliente
                       .Include(c => c.Endereco)
                       .Include(c => c.Contato)
                       .Where(c => c.Id == id)
                       .Select(c => new ClienteDto 
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
    var useCase = new AtualizarUseCase();

    return await useCase.AtualizarAsync<Cliente, ClienteDto >(
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


        cliente => new ClienteDto 
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