//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ByggeriSMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Besked
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Besked()
        {
            this.BeskedStatus = new HashSet<BeskedStatus>();
        }
    
        public int ID { get; set; }
        public Nullable<System.DateTime> ts_oprettet { get; set; }
        public string afsender { get; set; }
        public string afsenderUserID { get; set; }
        public string afsenderNavn { get; set; }
        public string modtager { get; set; }
        public string modtagerType { get; set; }
        public string besked { get; set; }
        public Nullable<int> messagePid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BeskedStatus> BeskedStatus { get; set; }
    }
}
