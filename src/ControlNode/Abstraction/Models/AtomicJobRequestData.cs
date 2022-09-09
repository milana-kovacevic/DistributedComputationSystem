using System.ComponentModel.DataAnnotations;

namespace ControlNode.Abstraction.Models
{
    public class AtomicJobRequestData
    {
        [Required]
        public string InputData { get; set; }
    }
}
