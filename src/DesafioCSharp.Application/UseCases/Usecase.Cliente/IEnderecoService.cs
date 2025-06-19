
using System.Threading.Tasks;
using Desafios.Domain.Cliente;
namespace  usecaseI;

    public interface IEnderecoService
    {
        Task PreencherAsync(ClienteEndereco endereco, string cep);
    }

