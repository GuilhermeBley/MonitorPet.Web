using System.Net;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MonitorPet.Infrastructure.Options;

namespace MonitorPet.Infrastructure.Email.Client;

internal class EmailClient
{
    private const bool HAS_SSL = true;
    private const int SMTP_PORT = 587;
    private const int SMTP_CRIPT_PORT = 465;
    
    private readonly NetworkCredential _credential;
    private readonly string _host;
    private readonly InternalMailAddress _defaultFromMailAddress;
    public InternalMailAddress DefaultFromMailAddress => _defaultFromMailAddress;

    public EmailClient(IOptions<EmailOptions> options)
    {
        if (options.Value is null ||
            string.IsNullOrEmpty(options.Value.UserName) ||
            string.IsNullOrEmpty(options.Value.Password) ||
            string.IsNullOrEmpty(options.Value.Host) ||
            string.IsNullOrEmpty(options.Value.AddressFrom))
            throw new ArgumentNullException(nameof(options));

        var optionsValue = options.Value;

        _credential = new NetworkCredential(optionsValue.UserName, optionsValue.Password);
        _host = optionsValue.Host;
        
        _defaultFromMailAddress = new (optionsValue.AddressNameFrom, optionsValue.AddressFrom);
    }

    public async Task SendHtmlMessageWithDefaultFrom(
        string subject,
        string bodyHtml,
        IEnumerable<string> to,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        InternalMailPriority mailPriority = InternalMailPriority.Normal,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await SendHtmlMessage(subject, bodyHtml, 
            new InternalMailAddress[] { DefaultFromMailAddress }, to, cc, bcc, mailPriority, cancellationToken);
    }

    public async Task SendHtmlMessage(
        string subject,
        string bodyHtml,
        IEnumerable<InternalMailAddress> from,
        IEnumerable<string> to,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        InternalMailPriority mailPriority = InternalMailPriority.Normal,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var mimeMessage = CreateMessage(subject, bodyHtml, from, to, cc, bcc, mailPriority, cancellationToken);

        await SendMimeMessage(mimeMessage, cancellationToken);
    }

    public async Task SendMimeMessage(MimeMessage mimeMessage, CancellationToken cancellationToken = default)
    {
        await SendMimeMessages(new MimeMessage[] { mimeMessage }, cancellationToken);
    }

    public async Task SendMimeMessages(IEnumerable<MimeMessage> mimeMessages, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!mimeMessages.Any())
            return;

        using var client = new SmtpClient();

        await client.ConnectAsync(_host, SMTP_PORT, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
        client.AuthenticationMechanisms.Remove("XOAUTH2");
        await client.AuthenticateAsync(_credential, cancellationToken);

        foreach (var mimeMessage in mimeMessages)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await client.SendAsync(mimeMessage, cancellationToken);
        }

        await client.DisconnectAsync(true);
    }
    
    private static MimeMessage CreateMessage(
        string subject,
        string bodyHtml,
        IEnumerable<InternalMailAddress> from,
        IEnumerable<string> to,
        IEnumerable<string>? cc = null,
        IEnumerable<string>? bcc = null,
        InternalMailPriority mailPriority = InternalMailPriority.Normal,
        CancellationToken cancellationToken = default)
    {
        if (!to.Any())
            throw new ArgumentOutOfRangeException("'To' must have one destination.");

        if (string.IsNullOrEmpty(subject))
            throw new ArgumentNullException(nameof(subject));

        var mailMessage = new MimeMessage();

        mailMessage.From.AddRange(from.Select(internalMailAddress =>
            new MailboxAddress(internalMailAddress.Name, internalMailAddress.AddressFrom)));

        mailMessage.To.AddRange(to.Select(mailTo => new MailboxAddress(string.Empty, mailTo)));

        if (cc is null)
            cc = Enumerable.Empty<string>();

        mailMessage.Cc.AddRange(cc.Select(mailCc => new MailboxAddress(string.Empty, mailCc)));

        if (bcc is null)
            bcc = Enumerable.Empty<string>();

        mailMessage.Bcc.AddRange(bcc.Select(mailBcc => new MailboxAddress(string.Empty, mailBcc)));

        mailMessage.Priority = Map(mailPriority);

        mailMessage.Subject = subject;

        mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = bodyHtml
        };

        return mailMessage;
    }

    private static MessagePriority Map(InternalMailPriority internalMailPriority)
    {
        if (internalMailPriority == InternalMailPriority.Normal)
            return MessagePriority.Normal;

        if (internalMailPriority == InternalMailPriority.Low)
            return MessagePriority.NonUrgent;

        return MessagePriority.Urgent;
    }
}