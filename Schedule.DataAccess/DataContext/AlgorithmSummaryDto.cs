namespace Schedule.DataAccess
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AlgorithmSummary")]
    public partial class AlgorithmSummaryDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("result_id")]
        public int ResultId { get; set; }

        [Required]
        [Column("type")]
        public byte AlgorithmType { get; set; }

        [Required]
        [Column("c_star")]
        public decimal Cstar { get; set; }

        [Required]
        [Column("c_max")]
        public decimal Cmax { get; set; }
    }
}
