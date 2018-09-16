using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayPal.Business.DAL.Models
{
    public class Vendor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int VendorId { get; set; }
        public string Email { get; set; }
    }
}
