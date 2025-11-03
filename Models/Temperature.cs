using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LEARN_MVVM.Models
{
    public class Temperature
    {
        public int Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string City { get; set; } = null!;

        public double Temp {  get; set; }
    }
}
