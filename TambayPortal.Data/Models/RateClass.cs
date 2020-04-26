using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Text;
using TambayPortal.Data.Constants;
using TambayPortal.Data.Services;

namespace TambayPortal.Data.Models
{
    public class RateClass
    {
        [Key]
        [Column("id")]
        [MaxLength(300)]
        public string ID { get; set; }
        [MaxLength(100)]
        public string DeviceSerial { get; set; }
        public double MinutesPerCredit { get; set; }
        [MaxLength(100)]
        public string NetworkRate { get; set; }
        [MaxLength(100)]
        public string BurstNetworkRate { get; set; }
        [MaxLength(100)]
        public string RateName { get; set; }
        public string ClassID
        {
            get
            {
                return Generate.Code(4);
            }
        }
    }
}
