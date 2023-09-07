namespace TechnicalTest2023.Logging
{
    public class EnrichedErrors
    {
        public EnrichedErrorType EnrichedError { get; set; }
        public string ErrorDetails { get; set; } = string.Empty;
    }

    public enum EnrichedErrorType
    {
        InvalidUserInputError,
        DuplicateUserError
    }
}
