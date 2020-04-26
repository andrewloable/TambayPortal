using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TambayPortal.Data.Enums;

namespace TambayPortal.Data.Models
{
    public class AdminUser
    {
        [Key]
        [Column("id")]
        [MaxLength(300)]
        public string ID { get; set; }
        [MaxLength(200)]
        public string Password { get; set; } 
        public AccessType AccessType { get; set; }
        [MaxLength(100)]
        public string DeviceSerial { get; set; }
    }
}
