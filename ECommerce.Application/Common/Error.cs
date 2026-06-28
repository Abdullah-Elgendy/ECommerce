using System.Text.Json.Serialization;

namespace ECommerce.Application.Common
{
    public record Error(string Code, string Description, ErrorType errorType = ErrorType.Failure)
    {
        public static Error Failure(string code = "General.Falure", string description = "General Failure has Occured") => new(code, description, ErrorType.Failure);
        public static Error Validation(string code = "General.Validation", string description = "General Validation Error has Occured") => new(code, description, ErrorType.Validation);
        public static Error NotFound(string code = "General.NotFound", string description = "Resource Not Found") => new(code, description, ErrorType.NotFound);
        public static Error Conflict(string code = "General.Conflict", string description = "General Conflict has Occured") => new(code, description, ErrorType.Conflict);
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Access Is Denied Due To Bad Authorization") => new(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code = "General.Forbidden", string description = "This Operation Is Forbidden") => new(code, description, ErrorType.Forbidden);
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "Provided Credentials Are Invalid") => new(code, description, ErrorType.InvalidCredentials);

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        InvalidCredentials = 6
    }
}