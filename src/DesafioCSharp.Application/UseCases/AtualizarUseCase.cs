
namespace desafiocs
{
    public class AtualizarUseCase
    {
        public async Task<IResult> AtualizarAsync<TEntity, TDto>(
            AppDbContext context,
            int id,
            Func<TEntity, Task> applyChangesAsync,
            Func<TEntity, TDto> dtoSelector
        )
            where TEntity : class
        {
            try
            {
                var dbSet = context.Set<TEntity>();
                var entity = await dbSet.FindAsync(id);
                if (entity is null)
                    return Results.NotFound(new { Mensagem = $"{typeof(TEntity).Name} {id} não encontrado." });
                await applyChangesAsync(entity);
                await context.SaveChangesAsync();
                var dto = dtoSelector(entity);
                return Results.Ok(dto);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: $"Ocorreu um erro ao processar sua requisição: {ex.Message}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
