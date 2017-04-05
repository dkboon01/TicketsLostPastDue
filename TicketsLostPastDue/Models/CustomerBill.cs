using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SedonaServices.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerBill
    {
        /// <summary>
        /// Sedona internal autonumber for the Customer Billing Address record
        /// </summary>
        public int CustomerBillId { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer table
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// True for Commercial account; false for Residential
        /// </summary>
        [Required]
        [Display(Name = "Commercial")]
        public bool IsCommercial { get; set; }

        /// <summary>
        /// If a person, CustomerName should be passed in as "Last, First"
        /// </summary>
        [Required]
        [StringLength(60)]
        [Display(Name = "Bill Name")]
        public string BillName { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [StringLength(60)]
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [StringLength(60)]
        public string Address3 { get; set; }

        /// <summary>
        /// City name; must exist in the GE_Table1 table
        /// </summary>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// State 2 letter abbreviation; must exist in the GE_Table2 table
        /// </summary>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// Zip code, not inclusive of extension (i.e. first 5 digits for U.S. zip); must exist in the GE_Table3 table
        /// </summary>
        [Required]
        public string Zip { get; set; }

        /// <summary>
        /// County name; must exist in the GE_Table4 table
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Last 4 of zip code
        /// </summary>
        [StringLength(10)]
        public string ZipExt { get; set; }

        /// <summary>
        /// Country code; must exist in the SS_Country table (Abbrev column); defaults to "USA" if not supplied
        /// </summary>
        public string CountryAbbrev { get; set; }

        [StringLength(25)]
        public string Phone1 { get; set; }

        [StringLength(25)]
        public string Phone2 { get; set; }

        [StringLength(25)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// Set to true if this is the primary billing address; this should be set to true for only one billing address
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Set to true to make this bill address inactive
        /// </summary>
        public bool? Inactive { get; set; }

        /// <summary>
        /// Value from AR_Branch table; list of valid options can be retrieved from GET: api/branch
        /// </summary>
        [Required]
        [StringLength(25)]
        public string BranchCode { get; set; }

        /// <summary>
        /// Send a copy of invoices via email; whether it is also printed/mailed is controlled at the Customer level
        /// </summary>
        [Required]
        public bool? EmailInvoices { get; set; }

        [StringLength(60)]
        public string BusinessName2 { get; set; }
    }
}