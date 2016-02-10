//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _2Hours_Ver2
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductSearch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductSearch()
        {
            this.OrderProducts = new HashSet<OrderProduct>();
        }
    
        public int productID { get; set; }
        public string productName { get; set; }
        public string unit { get; set; }
        public Nullable<decimal> price { get; set; }
        public string producType { get; set; }
        public Nullable<int> supplierID { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual Supplier Supplier { get; set; }
        public object ProductName { get; internal set; }
    }
}
