using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class JobRequestData
    {
        [Required]
        public JobType JobType { get; set; }

        [Required]
        public IEnumerable<AtomicJobRequestData> InputData { get; set; }
    }
}
