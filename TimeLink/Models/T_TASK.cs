namespace TimeLink.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_TASK
    {
        [Key]
        public int TaskID { get; set; }

        [ForeignKey("T_GOAL")]
        public int GoalID { get; set; }

        [Required]
        [StringLength(50)]
        public string TaskName { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public bool Done { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual T_GOAL T_GOAL { get; set; }
    }
}
