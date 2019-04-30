namespace TimeLink.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_GOAL
    {
        [Key]
        public int GoalID { get; set; }

        [Required]
        [StringLength(50)]
        public string GoalName { get; set; }

        public bool Done { get; set; }

        [Required]
        [StringLength(50)]
        [ForeignKey("T_ACCOUNT")]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual T_ACCOUNT T_ACCOUNT { get; set; }
        public virtual ICollection<T_TASK> T_TASK { get; set; }
    }
}
