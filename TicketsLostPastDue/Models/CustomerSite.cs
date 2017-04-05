using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SedonaServices.Models
{
    /// <summary>
    /// 
    /// 
    /// </summary>
    public class CustomerSite
    {

        /// <summary>
        /// Sedona internal autonumber for the Customer Site record
        /// </summary>
        public int? CustomerSiteId { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer table (required for POST and PUT, but cannot be changed on a PUT)
        /// </summary>
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Value from AR_Taxing_Group table; set to N/A if not applicable; list of valid options can be retrieved from GET: api/taxinggroup
        /// </summary>
        public string TaxGroupCode { get; set; }

        /// <summary>
        /// Value from AR_Branch table; list of valid options can be retrieved from GET: api/branch
        /// </summary>
        [Required]
        [StringLength(25)]
        public string BranchCode { get; set; }

        /// <summary>
        /// True for Commercial account; false for Residential
        /// </summary>
        [Required]
        public bool? IsCommercial { get; set; }

        /// <summary>
        /// If a person, CustomerName should be passed in as "Last, First"
        /// </summary>
        [Required]
        [StringLength(60)]
        public string SiteName { get; set; }

        [Required]
        [StringLength(60)]
        public string Address1 { get; set; }

        [StringLength(60)]
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
        /// County name; must exist in the GE_Table4 table; defaults to N/A if not supplied
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

        /// <summary>
        /// 
        /// </summary>
        [StringLength(12)]
        public string Phone1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(25)]
        public string Phone2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(25)]
        public string Fax { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(255)]
        public string Comments { get; set; }

        /// <summary>
        /// Map Code (for directional purposes).  This information will display on service tickets.
        /// </summary>
        [StringLength(15)]
        public string MapCode { get; set; }

        /// <summary>
        /// Cross street name for this site.  This information will display on service tickets.
        /// </summary>
        [StringLength(50)]
        public string CrossStreet { get; set; }

        /// <summary>
        /// The installation date of this site; defaults to today if not supplied
        /// </summary>
        public string CustomerSince { get; set; }

        /// <summary>
        /// Set to true to make this site inactive
        /// </summary>
        public bool? Inactive { get; set; }

        /// <summary>
        /// defaults to empty string if not needed
        /// </summary>
        [StringLength(25)]
        public string ExternalLink { get; set; }

        /// <summary>
        /// defaults to empty string if not needed
        /// </summary>
        [StringLength(25)]
        public string ExternalSerialNumber { get; set; }

        /// <summary>
        /// defaults to empty string if not needed
        /// </summary>
        [StringLength(25)]
        public string ExternalVersionNumber { get; set; }

        /// <summary>
        /// Value from AR_Taxing_Group table; Taxing Group that recurring invoices for this site will use; defaults to N/A if not supplied; list of valid options can be retrieved from GET: api/taxinggroup
        /// </summary>
        public string CycleTaxGroupCode { get; set; }

        /// <summary>
        /// Applies only to tax exempt organizations (i.e. government/nonprofit)
        /// </summary>
        [StringLength(20)]
        public string TaxExemptNum { get; set; }

        /// <summary>
        /// Applies only to tax exempt organizations in Canada (i.e. government/nonprofit)
        /// </summary>
        [StringLength(20)]
        public string GSTTaxExemptNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(20)]
        public string SiteNumber { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer_Bill table; links this site to a billing address
        /// </summary>
        [Required]
        public int CustomerBillId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(60)]
        public string BusinessName2 { get; set; }
    }
}