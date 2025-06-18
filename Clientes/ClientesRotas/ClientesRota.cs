namespace desafiocs.Cliente;

public static class ClientesRotas
{
    public static void AddRotasClientes(this WebApplication app )
    {
        app.MapGet("consulta", ()=> "consultando") ;
        app.MapGet("alterando", ()=> "alterando") ;
        app.MapGet("excluindo", ()=> "excluindo") ;
    }
}