namespace TechnicalTest2023
{
    public class EnrichedErrors
    {
        public EnrichedErrorType EnrichedError { get; set; }
        public string ErrorDetails { get; set; }
    }

    public enum EnrichedErrorType
    {
        InvalidUserInputError,
        DuplicateUserError
    }
}
