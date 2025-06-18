namespace desafiocs.Domain.Cliente;

public record AddClienteRequest(string Nome, string Cep, string Tipo,string Texto);