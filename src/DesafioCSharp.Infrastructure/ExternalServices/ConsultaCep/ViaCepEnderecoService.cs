// ViaCepEnderecoService.cs
using System.Threading.Tasks;
using Desafios.Domain.Cliente;
using usecaseI;

namespace Desafios.Infrastructure.Services
{
    public class ViaCepEnderecoService : IEnderecoService
    {
        public async Task PreencherAsync(ClienteEndereco endereco, string cep)
        {
            await endereco.BuscarEnderecoPorCepAsync(cep);
        }
    }
}
