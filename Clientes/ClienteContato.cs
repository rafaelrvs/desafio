using desafiocs.Clientes;

public class Contato
    {
        public int    Id        { get; set; }
        public string Tipo      { get; set; }
        public string Texto     { get; set; }

        // FK e navegação
        public int     ClienteId { get; set; }
        public Cliente Cliente   { get; set; }
    }