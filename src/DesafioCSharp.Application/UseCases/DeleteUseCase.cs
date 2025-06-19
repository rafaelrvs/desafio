
namespace desafiocs
{
    public class DeleteUseCase
    {
       
        public async Task<IResult> DeletarAsync<TEntity>(
            AppDbContext context,
            int id
        ) where TEntity : class
        {
            
            try
            {
                var dbSet = context.Set<TEntity>();


                var entidade = await dbSet.FindAsync(id);
                if (entidade is null)
                    return Results.NotFound(new { Mensagem = $"{typeof(TEntity).Name} {id} não encontrado." });


                dbSet.Remove(entidade);
                await context.SaveChangesAsync();

                return Results.Ok(new { Mensagem = $"{typeof(TEntity).Name} {id} excluído com sucesso." });
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
