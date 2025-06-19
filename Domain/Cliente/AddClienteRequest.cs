namespace desafiocs.Domain.Cliente;

public record AddClienteRequest(string Nome, string Cep,string Numero, string Complemento, string Tipo,string Texto);