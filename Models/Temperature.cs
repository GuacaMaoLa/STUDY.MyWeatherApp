using System.ComponentModel.DataAnnotations.Schema;

namespace LEARN_MVVM.Models
{
    public class Temperature
    {
        public int Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string City { get; set; } = null!;

        [Column(TypeName = "double(2,2)")]
        public double Temp {  get; set; }
    }
}
