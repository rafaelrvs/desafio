namespace desafiocs.Domain.Cliente;
using Desafios.Domain.Cliente;

public class ClienteContato
{
    public int Id { get; set; }
    public string Tipo { get; set; }
    public string Texto { get; set; }
      public int ClienteId { get; set; }
      public Cliente? Cliente { get; set; }
}