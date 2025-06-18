namespace desafiocs.dto;


  
public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string DataCadastro { get; set; }
    public List<ClienteDto> Contatos { get; set; }
    public List<ClienteEnderecoDto> Enderecos { get; set; }
}
