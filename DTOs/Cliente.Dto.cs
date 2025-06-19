public class ClienteDto
{
    public int Id { get; init; }
    public string Nome { get; init; } = "";
    public string DataCadastro { get; init; } = "";
    public EnderecoDto? Endereco { get; init; }
    public ContatoDto? Contato { get; init; }
}