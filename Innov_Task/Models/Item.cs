//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Innov_Task.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.item_Contract = new HashSet<item_Contract>();
        }
    
        public int Code { get; set; }

        [Required(ErrorMessage = "please Enter a price")]
        //[RegularExpression(@"[0-9]", ErrorMessage = "Please Enter a Correct price ")]
        public Nullable<decimal> Price { get; set; }

        [Required(ErrorMessage = "please Enter a Name")]
        //[RegularExpression(@"[a-zA-Z]", ErrorMessage = "Please Enter a Correct Name")]
        //[StringLength(20, MinimumLength = 2, ErrorMessage = "length must between 2 to 20 ")]
        //[Display(Name = "Item Name")]
        public string name { get; set; }


        [Required(ErrorMessage = "please Enter quantity")]
        //[RegularExpression(@"[0-9]", ErrorMessage = "Please Enter Correct quantity")]
        public Nullable<int> quantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<item_Contract> item_Contract { get; set; }
    }
}
