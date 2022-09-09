using System.ComponentModel.DataAnnotations;

namespace ControlNode.Frontend.Models
{
    public class AtomicJobRequestData
    {
        [Required]
        public string InputData { get; set; }
    }
}
