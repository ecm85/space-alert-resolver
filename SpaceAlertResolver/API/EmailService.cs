using System.Globalization;
using System.Net;
using System.Net.Mail;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace API
{
	public class EmailService
	{
		private IAmazonSimpleEmailService AmazonSimpleEmailService { get; }

		public EmailService(IAmazonSimpleEmailService amazonSimpleEmailService)
		{
			AmazonSimpleEmailService = amazonSimpleEmailService;
		}

		public async Task<string> SendEmailAsync(
			IEnumerable<string> toAddresses,
			IEnumerable<string> ccAddresses,
			IEnumerable<string> bccAddresses,
			string bodyHtml,
			string bodyText,
			string subject,
			string senderAddress
		)
		{
			var messageId = "";
			try
			{
				var response = await AmazonSimpleEmailService.SendEmailAsync(
					new SendEmailRequest
					{
						Destination = new Destination
						{
							BccAddresses = bccAddresses.ToList(),
							CcAddresses = ccAddresses.ToList(),
							ToAddresses = toAddresses.ToList()
						},
						Message = new Message
						{
							Body = new Body
							{
								Html = new Content { Charset = "UTF-8", Data = bodyHtml },
								Text = new Content { Charset = "UTF-8", Data = bodyText }
							},
							Subject = new Content { Charset = "UTF-8", Data = subject }
						},
						Source = senderAddress
					}
				);
				messageId = response.MessageId;
			}
			catch (Exception ex)
			{
				Console.WriteLine("SendEmailAsync failed with exception: " + ex.Message);
			}

			return messageId;
		}
	}
}
