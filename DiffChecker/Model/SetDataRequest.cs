using System;
using System.ComponentModel.DataAnnotations;

namespace DiffChecker.Model
{
    [Serializable]
    public class SetDataRequest
    {
        [Required]
        public string Data { get; set; }
    }
}