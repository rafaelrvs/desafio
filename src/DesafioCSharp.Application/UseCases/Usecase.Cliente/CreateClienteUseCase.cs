using desafiocs;
using desafiocs.Domain.Cliente;
using Desafios.Domain.Cliente;
using Microsoft.EntityFrameworkCore;
using usecaseI;
using Desafios.Application.Exceptions;
public class CreateClienteUseCase : ICreateClienteUseCase
{
    private readonly AppDbContext _context;
    private readonly IEnderecoService _enderecoService;

    public CreateClienteUseCase(AppDbContext context, IEnderecoService enderecoService)
    {
        _context = context;
        _enderecoService = enderecoService;
    }

    public async Task<ClienteDto> ExecuteAsync(AddClienteRequest request)
    {
   
        var existe = await _context.Cliente
            .AnyAsync(c => c.Contato != null && c.Contato.Texto == request.Texto);
        if (existe)
            throw new DuplicateContatoException(
                $"Já existe um cliente com o texto de contato '{request.Texto}'."
            );

        var novo = new Cliente(request.Nome)
        {
            Endereco = new ClienteEndereco(),
            Contato = new ClienteContato
            {
                Tipo  = request.Tipo,
                Texto = request.Texto
            }
        };

        await _enderecoService.PreencherAsync(novo.Endereco!, request.Cep);
        novo.Endereco!.Numero      = request.Numero;
        novo.Endereco.Complemento = request.Complemento;


        await _context.Cliente.AddAsync(novo);
        await _context.SaveChangesAsync();

  
        return new ClienteDto
        {
            Id           = novo.Id,
            Nome         = novo.Nome,
            DataCadastro = novo.DataCadastro,
            Endereco = new EnderecoDto
            {
                Logradouro  = novo.Endereco.Logradouro,
                Cidade      = novo.Endereco.Cidade,
                Cep         = novo.Endereco.Cep,
                Numero      = novo.Endereco.Numero,
                Complemento = novo.Endereco.Complemento
            },
            Contato = new ContatoDto
            {
                Id    = novo.Contato!.Id,
                Tipo  = novo.Contato.Tipo,
                Texto = novo.Contato.Texto
            }
        };
    }
}
