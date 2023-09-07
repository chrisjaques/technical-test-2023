using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace TechnicalTest2023.Logging
{
    public static class ErrorExtensions
    {
        /// <summary>
        /// A very basic and primitive way of converting errors into something that a user could understand
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static string ToUserFacingDescription(this ValueEnumerable errors)
        {
            return errors.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid user input";
        }

        /// <summary>
        /// A very basic way of extracting the information around errors that we actually care about to store in logs
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static string ToLogs(this ValueEnumerable errors)
        {
            var result = string.Empty;
            foreach (var error in errors)
            {
                foreach (var value in error.Errors)
                {
                    result += value.ErrorMessage + "\n";
                }
            }

            return result;
        }
    }
}
