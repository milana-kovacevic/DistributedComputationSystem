using ControlNode.Abstraction.Models;
using System.ComponentModel.DataAnnotations;

namespace ControlNode.Frontend.Entities
{
    public class JobRequestData
    {
        [Required]
        public JobType JobType { get; set; }

        [Required]
        public IEnumerable<AtomicJobRequestData> InputData { get; set; }
    }
}
