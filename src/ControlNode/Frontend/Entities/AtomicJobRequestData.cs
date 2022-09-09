using System.ComponentModel.DataAnnotations;

namespace ControlNode.Frontend.Entities
{
    public class AtomicJobRequestData
    {
        [Required]
        public string InputData { get; set; }
    }
}
