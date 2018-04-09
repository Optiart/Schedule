namespace Schedule.DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Tabs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tabs()
        {
            Results = new HashSet<Results>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public int number_of_devices { get; set; }

        public byte device_type { get; set; }

        [StringLength(2048)]
        public string productivity { get; set; }

        public int number_of_palletes { get; set; }

        public int number_of_work { get; set; }

        [StringLength(2048)]
        public string work_per_pallete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Results> Results { get; set; }
    }
}
