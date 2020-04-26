using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TambayPortal.Data.Models
{
    public class NetworkUsage
    {
        [Key]
        [Column("id")]
        [MaxLength(300)]
        public string ID { get; set; }
        [MaxLength(100)]
        public string DeviceSerial { get; set; }
        public double Credits { get; set; }
        public bool IsStopped { get; set; }
        [ForeignKey("RateID")]
        public RateClass Rate { get; set; }
        [MaxLength(100)]
        public string RateID { get; set; }
    }
}
