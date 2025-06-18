namespace desafiocs.Domain.Clientes.Request;

public record AddContatoRequest(
      string Tipo,
      string Texto
  );