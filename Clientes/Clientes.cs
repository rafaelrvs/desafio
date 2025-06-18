namespace desafiocs.Clientes;
public class Cliente
 {
        public int    Id           { get; set; }
        public string Nome         { get; set; }
        public string DataCadastro { get; set; }

        public ICollection<Contato>  Contatos  { get; set; } = new List<Contato>();
        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }