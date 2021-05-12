using System;
using System.ComponentModel.DataAnnotations;

namespace DiffChecker.Api.Model
{
    [Serializable]
    public class SetDataRequest
    {
        [Required]
        public string Data { get; set; }
    }
}