using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public partial class Patient
    {
        partial void Initialize()
        {
            this.adress = new Adress();
            this.cp = new ContactPAtient();
        }

        public virtual Adress adress { get; set; }
        public virtual ContactPAtient cp {get; set ;}
    }
}