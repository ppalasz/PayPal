using System.ComponentModel.DataAnnotations;

namespace PayPal.Business.DAL.Models
{
    public class ProjectType
    {
        [Key]
        public int ProjectTypeId { get; set; }
        public string ProjectTypeName { get; set; }
    }
}
