using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Models
{
    public partial class Staff
    {
        partial void Initialize()
        {
            this.adress = new Adress();
        }
        
        public DateTime creationDate { get; set; }
        public virtual Adress adress { get; set; }

    }
}