using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PayPal.Business.DAL.Models
{
    public class VwProject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public double? WordCountIce { get; set; }
        public double? WordCountExact { get; set; }
        public double? WordCount99_80 { get; set; }
        public double? WordCount79_70 { get; set; }
        public double? WordCount69 { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Url { get; set; }
        public string ProjectTypeName { get; set; }
        public string StatusName { get; set; }
        public double? Repetition { get; set; }
    }
}
