namespace MonitorPet.Ui.Client.Extension.StringExtension;

internal static class StringExtension
{
    public static string MaxLenght(this string value, int lenght)
    {
        if (lenght < 3)
            return string.Empty;

        if (value is null)
            return string.Empty;

        if (value.Length <= lenght)
            return value;

        return string.Concat(value.Take(lenght - 3)) + "...";
    }
}
