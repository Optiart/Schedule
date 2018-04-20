namespace Schedule.DataAccess
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Results")]
    public partial class ResultDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("tab_id")]
        public int TabId { get; set; }

        [Required]
        [StringLength(5096)]
        [Column("result")]
        public string Result { get; set; }

        [Column("chain")]
        [StringLength(1024)]
        public string Chain { get; set; }

        public virtual TabDto Tabs { get; set; }
    }
}
