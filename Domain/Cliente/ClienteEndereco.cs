namespace Desafios.Domain.Cliente;

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
public class ClienteEndereco
{
    
    public int Id { get; set; }
      private string _numero = "";
      private string _complemento = "";
    public string Cep { get; set; } = "";
    public string Logradouro { get; set; } = "";
        public string Cidade { get; set; } = "";
    public string Complemento
    {
        get => _complemento;

        set =>_complemento = (value??"").Trim();
        
         } 
    public string Numero
    {
        get => _numero;
        set => _numero = (value ?? "").Trim();
    }
      public int ClienteId { get; set; }
      public Cliente? Cliente { get; set; }



    public async Task<bool> BuscarEnderecoPorCepAsync(string cep)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var dadosCep = JsonSerializer.Deserialize<CepResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dadosCep is not null && !string.IsNullOrEmpty(dadosCep.Cep))
            {
                Logradouro = dadosCep.Logradouro;
                Complemento = dadosCep.Complemento;
                Cidade = dadosCep.Localidade;
                return true;
            }
        }
    



        return false;
    }

}