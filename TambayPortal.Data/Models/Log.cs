using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TambayPortal.Data.Models
{
    public class Log
    {
        [Key]
        [Column("id")]
        [MaxLength(300)]
        public string ID { get; set; }
        public string Message { get; set; }
        public DateTime EventDate { get; set; }
        public string Type { get; set; }
    }
}
