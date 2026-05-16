namespace IronIQ.Application.Common.Models;

public enum ErrorType { Failure, NotFound, Conflict, Unauthorized, Validation }

public record Error(ErrorType Type, string Message)
{
    public static Error NotFound(string entity, object id) =>
        new(ErrorType.NotFound, $"{entity} with id '{id}' was not found.");

    public static Error Conflict(string message) =>
        new(ErrorType.Conflict, message);

    public static Error Unauthorized(string message = "Unauthorized") =>
        new(ErrorType.Unauthorized, message);

    public static Error Failure(string message) =>
        new(ErrorType.Failure, message);
}
