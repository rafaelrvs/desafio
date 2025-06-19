using desafiocs;
using desafiocs.Domain.Cliente;

public interface ICreateClienteUseCase
{
    Task<ClienteDto> ExecuteAsync(AddClienteRequest request);
}
