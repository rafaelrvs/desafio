namespace dtos;

public class AddClienteRequest
{
    public string Nome        { get; set; } = "";
    public string Cep         { get; set; } = "";
    public string Numero      { get; set; } = "";
    public string Complemento { get; set; } = "";
    public string Tipo        { get; set; } = "";  
    public string Texto       { get; set; } = "";  
}
