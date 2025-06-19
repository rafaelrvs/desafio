
namespace Desafios.Application.Exceptions
{
    public class DuplicateContatoException : Exception
    {
        public DuplicateContatoException(string message)
            : base(message)
        { }
    }
}
