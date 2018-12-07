using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Models
{
    public partial class Consultation
    {
        partial void Initialize()
        {
            this.vaccin = new Vaccin();
            this.ordonnance = new Ordonnance();
            this.antecedent = new Antecedent();
        }

        
        Antecedent Antecedent { get; set; }
        public Nullable<int> ID_Ordonnance { get; set; }
        public Nullable<int> ID_Vaccin { get; set; }
        public Nullable<int> ID_Antecedent { get; set; }

        public virtual Ordonnance ordonnance { get; set; }
        public virtual Vaccin vaccin { get; set; }
        public virtual Antecedent antecedent { get; set; }

    }
}