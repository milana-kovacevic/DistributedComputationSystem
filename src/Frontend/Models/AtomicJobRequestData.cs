using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class AtomicJobRequestData
    {
        [Required]
        public string InputData { get; set; }
    }
}
