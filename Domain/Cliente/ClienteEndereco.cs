namespace Desafios.Domain.Cliente;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
public class ClienteEndereco
{
    public int Id { get; set; }
    public string Cep { get; set; } = "";
    public string Logradouro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Complemento { get; set; } = "";


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