namespace TimeLink.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_ACCOUNT
    {
        [Key]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public bool IsMale { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<T_GOAL> T_GOAL { get; set; }
    }
}
