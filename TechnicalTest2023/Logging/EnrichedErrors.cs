namespace TechnicalTest2023.Logging
{
    // Could include type and infer it in the error messages instead of hard coded "user" to make this more generic
    public class EnrichedErrors
    {
        public EnrichedErrorType EnrichedError { get; set; }
        public string ErrorDetails { get; set; } = string.Empty;

        public string ErrorType
        {
            get
            {
                return EnrichedError switch
                {
                    EnrichedErrorType.InvalidUserInputError => "Invalid input provided.",
                    EnrichedErrorType.DuplicateUserError => "User already exists.",
                    _ => "User could not be created.",
                };
            }
        }
    }

    public enum EnrichedErrorType
    {
        InvalidUserInputError,
        DuplicateUserError
    }
}
