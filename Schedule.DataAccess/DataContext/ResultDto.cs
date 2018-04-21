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
        [Column("result")]
        public string Result { get; set; }

        [Column("chain")]
        public string Chain { get; set; }

        public virtual TabDto Tabs { get; set; }
    }
}
