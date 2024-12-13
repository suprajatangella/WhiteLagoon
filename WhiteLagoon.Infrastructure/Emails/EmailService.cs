using Amazon;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Contract;

namespace WhiteLagoon.Infrastructure.Emails
{
    public class EmailService : IEmailService
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly RegionEndpoint _region;

        public EmailService(IConfiguration configuration)
        {
            var awsConfig = configuration.GetSection("AWS");
            _accessKey = awsConfig["AccessKey"];
            _secretKey = awsConfig["SecretKey"];
            _region = RegionEndpoint.GetBySystemName(awsConfig["Region"]);
        }
        public async Task<bool> SendEmailAsync(string fromAddress,string toAddress, string subject, string message)
        {
            using var client = new AmazonSimpleEmailServiceClient(_accessKey, _secretKey, _region);

            var sendRequest = new SendEmailRequest
            {
                Source = fromAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toAddress }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Text = new Content(message)
                    }
                }
            };

            var response = await client.SendEmailAsync(sendRequest);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Email failed to send. Status Code: {response.HttpStatusCode}");
            }
            return true;
        }
    }
}
