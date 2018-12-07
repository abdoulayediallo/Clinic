using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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