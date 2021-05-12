using System;
using System.Text;
using DiffChecker.Api.Services.Interfaces;
using DiffChecker.Domain.Error;

namespace DiffChecker.Api.Services
{
    public class DecodeService : IDecodeService
    {
        public string DecodeString(string encodedData)
        {
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