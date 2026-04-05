namespace Appetit.Domain.Common.Exceptions;

public class InternalServerErrorException : Exception
{
    public InternalServerErrorException()
    {
    }

    public InternalServerErrorException(string message) : base(message)
    {
    }

}
