namespace Schedule.DataAccess
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Results
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int tab_id { get; set; }

        [Required]
        [StringLength(5096)]
        public string result { get; set; }

        public virtual Tabs Tabs { get; set; }
    }
}
