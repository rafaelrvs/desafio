namespace desafiocs.Domain.Clientes.Request
{

    public record AddEnderecoRequest(
        string Cep,
        string Logradouro,
        string Cidade,
        string Numero,
        string Complemento
    );
}
