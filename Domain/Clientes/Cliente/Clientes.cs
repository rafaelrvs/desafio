namespace desafiocs.Clientes
{
    public class Cliente
    {
        private static int _nextId = 1;

        public int    Id           { get; private set; }
        public string Nome         { get; private set; }
        public string DataCadastro { get; private set; }

        public ICollection<Contato>  Contatos  { get; private set; } = new List<Contato>();
        public ICollection<Endereco> Enderecos { get; private set; } = new List<Endereco>();

        public Cliente(string nome)
        {
            Id           = _nextId++;
            Nome         = nome;
            DataCadastro = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

       
    }
}
