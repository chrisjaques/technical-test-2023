using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace TechnicalTest2023
{
    public static class ErrorExtensions
    {
        public static string ToHuman(this ValueEnumerable errors)
        {
            return errors.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid user input";
        }

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
