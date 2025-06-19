
using Microsoft.EntityFrameworkCore;


namespace Usecase.EndpointExtensions
{
    public static class ConsultaClienteUseCase
    {
      
        public static RouteHandlerBuilder MapGetById<TEntity, TDto>(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            Func<IQueryable<TEntity>, int, IQueryable<TDto>> selector
        )
            where TEntity : class
        {
            return endpoints.MapGet(pattern, async (int id, AppDbContext db) =>
            {
                try
                {
                  
                    var query = selector(db.Set<TEntity>(), id);

      
                    var item = await query.FirstOrDefaultAsync();
                    if (item is null)
                        return Results.NotFound(new { Mensagem = $"{typeof(TEntity).Name} {id} não encontrado." });

                    return Results.Ok(item);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: $"Ocorreu um erro ao processar sua requisição: {ex.Message}",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            });
        }
    }
}
