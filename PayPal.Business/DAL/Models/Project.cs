using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayPal.Business.DAL.Models
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ProjectId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ProjectName { get; set; }

        [Required]
        public string SourceLanguage { get; set; }

        [Required]
        public string TargetLanguage { get; set; }

        public double? WordCountIce { get; set; }

        public double? WordCountExact { get; set; }

        public double? WordCount99_80 { get; set; }

        public double? WordCount79_70 { get; set; }

        public double? WordCount69 { get; set; }

        public double? Repetition { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int ProjectTypeId { get; set; }

        [Required]
        public int StatusId { get; set; }

        public virtual ProjectType ProjectType { get; set; }

        public virtual Status Status { get; set; }
    }
}