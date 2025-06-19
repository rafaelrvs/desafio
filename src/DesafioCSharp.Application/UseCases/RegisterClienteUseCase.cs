
namespace desafiocs
{
    public static class RegisterClienteUseCase
    {
        public static RouteHandlerBuilder MapCreate<TEntity, TRequest, TDto>(
            this IEndpointRouteBuilder app,
            string postPattern,                  
            string getPatternTemplate,            
            Func<TRequest, TEntity> entityFactory,
            Func<TEntity, TDto> dtoSelector
        )
            where TEntity : class
        {
            return app.MapPost(postPattern, async (
                TRequest request,
                AppDbContext db,
                ILoggerFactory loggerFactory
            ) =>
            {
                var logger = loggerFactory.CreateLogger($"Create{typeof(TEntity).Name}");

                try
                {
             
                    var entity = entityFactory(request)!;

                    await db.Set<TEntity>().AddAsync(entity);
                    await db.SaveChangesAsync();

                    var dto = dtoSelector(entity);

                 
                    var location = getPatternTemplate
                        .Replace("{id:int}", entity
                            .GetType()
                            .GetProperty("Id")!
                            .GetValue(entity)!
                            .ToString()!);

                    return Results.Created($"/{location}", dto);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro ao criar {Entity}", typeof(TEntity).Name);
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            });
        }
    }
}
