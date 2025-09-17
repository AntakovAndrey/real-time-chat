namespace ChatClientWpf.Exceptions;

public class ServerNotRespondsException : Exception
{
    public ServerNotRespondsException(string message)
        : base(message) { }
}
