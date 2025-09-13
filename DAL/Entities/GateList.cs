using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities
{
    public class GateList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string GateCode { get; set; }
        public string GateName { get; set; }
        public string ParentCode { get; set; }
    }
}
