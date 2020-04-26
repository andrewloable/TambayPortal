using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TambayPortal.Data.Models
{
    public class SaleTransaction
    {
        [Key]
        [Column("id")]
        [MaxLength(300)]
        public string ID { get; set; }
        [MaxLength(100)]
        public string DeviceSerial { get; set; }
        public double Amount { get; set; }
        public double Minutes { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
