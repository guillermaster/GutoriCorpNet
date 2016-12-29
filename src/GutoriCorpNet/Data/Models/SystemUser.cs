using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GutoriCorp.Data.Models
{
    public class SystemUser
    {
        public short id { get; set; }

        [StringLength(150)]
        public string first_name { get; set; }

        [StringLength(150)]
        public string last_name { get; set; }

        [StringLength(50)]
        public string user_name { get; set; }

        [StringLength(30)]
        public string user_pwd { get; set; }

        public short status_id { get; set; }

        public DateTime created_on { get; set; }

        public short created_by { get; set; }

        public DateTime modified_on { get; set; }

        public short modified_by { get; set; }

        public override string ToString()
        {
            return first_name + " " + last_name;
        }
    }
}
