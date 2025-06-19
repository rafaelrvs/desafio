
using Microsoft.EntityFrameworkCore;

namespace Usecase.EndpointExtensions
{
    public static class ListarUseCase
    {
        public static RouteHandlerBuilder MapList<TEntity, TDto>(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            Func<IQueryable<TEntity>, IQueryable<TDto>> selector
        )
            where TEntity : class
        {
            return endpoints.MapGet(pattern, async (AppDbContext db) =>
            {
                try
                {
                    var queryable = db.Set<TEntity>();

                    var projected = selector(queryable);

                    var lista = await projected.ToListAsync();

                    return Results.Ok(lista);
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
