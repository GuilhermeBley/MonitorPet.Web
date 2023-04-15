namespace MonitorPet.Core.Internal;

internal static class Validation
{
    public const string REGEX_VALIDATION_EMAIL = @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
    public const string LETTER_NUMBER = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@$!%*#?&";
    public const string LETTER_NUMBER_SPACE = LETTER_NUMBER + " ";

    public static bool IsValidEmail(string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, REGEX_VALIDATION_EMAIL);
    }

    public static bool ContainsNotAllowedChars(string allowedChars, string toCheckChars)
        => GetNotAllowed(allowedChars, toCheckChars).Any();

    public static IEnumerable<char> GetNotAllowed(string allowedChars, string toCheckChars)
        => toCheckChars.Where(c => !allowedChars.Contains(c));
}