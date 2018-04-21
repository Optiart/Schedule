namespace Schedule.DataAccess
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Tabs")]
    public partial class TabDto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TabDto()
        {
            Results = new HashSet<ResultDto>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("number_of_devices")]
        public int NumberOfDevices { get; set; }

        [Column("device_type")]
        public byte DeviceType { get; set; }

        [Column("productivity")]
        public string Productivity { get; set; }

        [Column("number_of_palletes")]
        public int NumberOfPalletes { get; set; }

        [Column("number_of_work")]
        public int NumberOfWork { get; set; }

        [Column("work_per_pallete")]
        public string WorkPerPallete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResultDto> Results { get; set; }
    }
}
