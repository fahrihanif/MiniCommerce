namespace MiniCommerce.API.Common;

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string FromName { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
}