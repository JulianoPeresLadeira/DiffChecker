using System;
using System.Text;
using DiffChecker.Api.Services.Interfaces;
using DiffChecker.Domain.Error;
using Microsoft.Extensions.Logging;

namespace DiffChecker.Api.Services
{
    public class DecodeService : IDecodeService
    {
        private readonly ILogger<DecodeService> _logger;

        public DecodeService(ILogger<DecodeService> logger)
        {
            _logger = logger;
        }

        public string DecodeString(string encodedData)
        {
            _logger.LogInformation($"Decoding {encodedData}");

            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
            }
            catch (Exception)
            {
                throw new DataDecodingException(encodedData);
            }
        }
    }
}