
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
    
public partial class OrderDetail
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public OrderDetail()
    {

        this.OrderProducts = new HashSet<OrderProduct>();

    }


    public int orderNumber { get; set; }

    public Nullable<System.DateTime> orderDate { get; set; }

    public Nullable<System.DateTime> deliveryTime { get; set; }

    public Nullable<bool> deliveryStatus { get; set; }

    public Nullable<decimal> totalPrice { get; set; }

    public string Id { get; set; }



    public virtual AspNetUser AspNetUser { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<OrderProduct> OrderProducts { get; set; }

}

}
