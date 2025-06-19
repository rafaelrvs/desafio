using desafiocs.Domain.Cliente;

namespace Desafios.Domain.Cliente;

public class Cliente
{

    public int Id { get; private set; }
    public string Nome { get; private set; }
    private static int _nextId = 1;
    public string DataCadastro { get; private set; }
    public ClienteContato? Contato { get; set; }
    public ClienteEndereco? Endereco { get; set; }
    public Cliente(string nome)
    {
        Nome = nome;
        DataCadastro = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
    }
     public void AlterarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(novoNome));

        Nome = novoNome.Trim();
    }
}