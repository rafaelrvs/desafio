
namespace Desafios.Domain.Rotas;

using desafiocs.Domain.Cliente;
using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;
public static class ClienteRotas
{


    public static void AddRotasClientes(this WebApplication app)
    {





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


            var criadoDto = new ClienteDto
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
                    Tipo = novoCliente.Contato!.Tipo,
                    Texto = novoCliente.Contato.Texto
                }
            };

            return Results.Created($"/consulta/{novoCliente.Id}", criadoDto);
        });

        app.MapGet("consulta", async (AppDbContext context) =>
                  {
                      var listaDto = await context.Cliente
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
                                      Tipo = c.Contato.Tipo,
                                      Texto = c.Contato.Texto
                                  }
               })
                     .ToListAsync();

                      return Results.Ok(listaDto);
                  });




  app.MapPatch("altera/{id:int}", async (int id, UpdateClienteRequest req, AppDbContext ctx) =>
        {
          
            var cliente = await ctx.Cliente
                .Include(c => c.Endereco)
                .Include(c => c.Contato)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente is null)
                return Results.NotFound(new { Mensagem = $"Cliente {id} não encontrado." });

            
            if (!string.IsNullOrWhiteSpace(req.Nome))
                cliente.AlterarNome(req.Nome);     

            if (req.Cep is not null && cliente.Endereco is not null)
            {
                await cliente.Endereco.BuscarEnderecoPorCepAsync(req.Cep);
                if (req.Numero      is not null) cliente.Endereco.Numero      = req.Numero;
                if (req.Complemento is not null) cliente.Endereco.Complemento = req.Complemento;
            }

            if (req.Tipo is not null && req.Texto is not null)
            {
                if (cliente.Contato is null)
                    cliente.Contato = new ClienteContato { ClienteId = cliente.Id };

                cliente.Contato.Tipo  = req.Tipo;
                cliente.Contato.Texto = req.Texto;
            }

            // 3) Salvar e projetar o DTO de volta
            await ctx.SaveChangesAsync();

            var dto = new ClienteDto
            {
                Id           = cliente.Id,
                Nome         = cliente.Nome,
                DataCadastro = cliente.DataCadastro,
                Endereco     = cliente.Endereco is null ? null : new EnderecoDto {
                    Cep         = cliente.Endereco.Cep,
                    Logradouro  = cliente.Endereco.Logradouro,
                    Cidade      = cliente.Endereco.Cidade,
                    Numero      = cliente.Endereco.Numero,
                    Complemento = cliente.Endereco.Complemento
                },
                Contato      = cliente.Contato is null ? null : new ContatoDto {
                    Tipo  = cliente.Contato.Tipo,
                    Texto = cliente.Contato.Texto
                }
            };

            return Results.Ok(dto);
        });



         app.MapDelete("deletar/{id:int}", async (int id, AppDbContext context) =>
        {

            var cliente = await context.Cliente.FindAsync(id);
            if (cliente is null)
            {
                return Results.NotFound(new { Mensagem = $"Cliente {id} não encontrado." });
            }


            context.Cliente.Remove(cliente);
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}