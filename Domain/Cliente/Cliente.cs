namespace Desafios.Domain.Cliente;
public class Cliente
{

public int Id { get; private set; }
public string Nome { get; private set; }
   private static int _nextId = 1;
   public string DataCadastro { get; private set; }
    public Cliente(string nome)
    {
        Id = _nextId++;
        Nome = nome;
        DataCadastro = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");  
    }
}