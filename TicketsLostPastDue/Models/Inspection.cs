using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SedonaServices.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TicketsLostPastDue.Models
{
    public class Inspection
    {
        public int ServiceTicketId { get; set; }
        public string TicketStatus { get; set; }
        public string TicketStatusName { get; set; }
        [Display(Name = "Ticket Number")]
        public string TicketNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerNumber { get; set; }
        public string SiteName { get; set; }
        public int CustomerSiteId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CustomerSiteAddress { get; set; }
        public string CustomerSystemId { get; set; }
        public string AlarmAccount { get; set; }
        public string SystemCode { get; set; }
        public string MultipleSystems { get; set; }
        public string CreationDate { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedByPhone { get; set; }
        public string RequestedByPhoneExt { get; set; }
        public string AutoNotify { get; set; }
        public string ProblemCode { get; set; }
        public string SubProblemCode { get; set; }
        public string ScheduledFor { get; set; }
        public string ServiceTechCode { get; set; }
        public int EstimatedLength { get; set; }
        public string ResolutionCode { get; set; }
        public bool Billable { get; set; }
        public bool Billed { get; set; }
        public decimal EquipmentCharge { get; set; }
        public decimal LaborCharge { get; set; }
        public decimal OtherCharge { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal RegularHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal HolidayHours { get; set; }
        public decimal TripCharge { get; set; }
        public int InvoiceNumber { get; set; }
        public decimal RegularRate { get; set; }
        public decimal OvertimeRate { get; set; }
        public decimal HolidayRate { get; set; }

        public string CustomerComments { get; set; }
        public List<Note> Notes { get; set; }
        public List<Part> Parts { get; set; }
        public Customer Customer { get; set; }
        public CustomerSite Site { get; set; }
        // public List<Customer> customer { get; set; }
        public SdSystem System { get; set; }
        public string RouteCode { get; set; }
        public int ServiceCompanyID { get; set; }
        public Competitors comp { get; set; }
        public string ServiceCompanyCode { get; set; }
        public bool IsInspection { get; set; }
        public string UserCode { get; set; }
        public string EnteredBy { get; set; }
        public string CategoryCode { get; set; }
        public string PONumber { get; set; }
        public string ServiceLevelCode { get; set; }
        public string Priority { get; set; }
        public int InspectionId { get; set; }

        public int ExpertiseLevel { get; set; }
        public string Signer { get; set; }
        public int CustomerContactId { get; set; }
    }
    //public class Note
    //{

    //    public int NoteId { get; set; }
    //    public int ServiceTicketNumber { get; set; }
    //    public string note { get; set; }
    //    public int AccessLevel { get; set; }
    //    public string UserCode { get; set; }
    //    public bool IsResolution { get; set; }
    //    public DateTime CreationDate { get; set; }

    //}
    //public class Part
    //{
    //    public string partno { get; set; }
    //}
    //public class SdSystem
    //{
    //    public int CustomerSystemId { get; set; }
    //    public int CSCustomerSystemId { get; set; }
    //    public int CustomerId { get; set; }
    //    public int CustomerSiteId { get; set; }
    //    [DisplayName("System")]
    //    public string SystemCode { get; set; }
    //    public string PanelTypeCode { get; set; }
    //    public string PanelLocation { get; set; }
    //    public string Memo { get; set; }
    //    public string JobCode { get; set; }
    //    public string ContactFromCode { get; set; }
    //    public string WarrantyCode { get; set; }
    //    public string ServiceLevelCode { get; set; }
    //    public int ServiceLevelId { get; set; }
    //    [DisplayName("Quantity")]
    //    public int AlarmAccount { get; set; }
    //    public string AlarmCompanyCode { get; set; }
    //    public string ServiceCompanyCode { get; set; }
    //    public int ServiceCompanyId { get; set; }
    //    public DateTime ContractStartDate { get; set; }
    //    public int Months { get; set; }
    //    public int RenewalMonths { get; set; }
    //    public int InvoiceDescriptionId { get; set; }
    //    public string CyclePONumber { get; set; }
    //    public DateTime CyclePOExpire { get; set; }
    //    public DateTime LastInspectionDate { get; set; }
    //    public DateTime NextInspectionDate { get; set; }
    //    public string InspectionNotes { get; set; }
    //    public string SystemComments { get; set; }
    //    public DateTime OkToIncrDate { get; set; }
    //    public DateTime WarrantyDate { get; set; }
    //    public bool Inactive { get; set; }
    //    public bool PORequired { get; set; }

    //}
    public class Inspections
    {
        public List<SearchInspections> SearchInspections { get; set; }
    }
    public class Customer
    {
        /// <summary>
        /// Sedona internal autonumber for the Customer record
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Customer Number the customer sees
        /// </summary>
        [Required]
        [StringLength(15)]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// If a person, CustomerName should be passed in as "Last, First"
        /// </summary>
        [Required]
        [StringLength(60)]
        public string CustomerName { get; set; }

        /// <summary>
        /// Customer Status:
        /// AR = Active Recurring;
        /// ANR = Active Nonrecurring;
        /// CANC = Canceled;
        /// defaults to config setting for customerStatusCode if not supplied
        /// </summary>
        [StringLength(25)]
        public string StatusCode { get; set; }

        /// <summary>
        /// Value from AR_Type_Of_Customer table; list of valid options can be retrieved from GET: api/customertype
        /// </summary>
        [Required]
        [StringLength(25)]
        public string TypeCode { get; set; }


        /// <summary>
        /// Value from AR_Collection_Status table; defaults to config setting for customerCollectionStatusCode if not supplied
        /// </summary>
        [StringLength(25)]
        public string CollectionStatusCode { get; set; }

        /// <summary>
        /// Value from SY_Employee table; defaults to N/A if not supplied; list of valid options can be retrieved from GET: api/salesperson
        /// </summary>
        [StringLength(25)]
        public string SalespersonCode { get; set; }

        /// <summary>
        /// Always N/A
        /// </summary>
        //public int DealerCode { get; set; }

        /// <summary>
        /// Value from AR_Term table (Payment terms); list of valid options can be retrieved from GET: api/term
        /// </summary>
        [Required]
        [StringLength(25)]
        public string TermCode { get; set; }

        /// <summary>
        /// Applies only to tax exempt organizations (i.e. government/nonprofit)
        /// </summary>
        [StringLength(20)]
        public string TaxExemptNum { get; set; }

        /// <summary>
        /// A Blanket PO is used as the default Purchase Order number for the Customer.
        /// </summary>
        [StringLength(20)]
        public string BlanketPO { get; set; }

        /// <summary>
        /// date the blanket PO expires; defaults to null date (12/30/1899) if not needed
        /// </summary>
        [StringLength(25)]
        public string BlanketPOExpire { get; set; }

        /// <summary>
        /// Can store Customer Number from other systems
        /// </summary>
        [StringLength(15)]
        public string OldCustomerNumber { get; set; }

        /// <summary>
        /// read only; do not set explicitly; calculated automatically
        /// </summary>
        [StringLength(25)]
        public string LastStatementDate { get; set; }

        /// <summary>
        /// Set to true for Customer to be by-passed for Late Fees and/or Finance Charges.
        /// </summary>
        public bool? NoLateFees { get; set; }

        /// <summary>
        /// Set to true for Customer to be skipped when printing Statements.
        /// </summary>
        public bool? NoStatements { get; set; }

        /// <summary>
        /// Set to true for Customer to be disregarded for Collections.
        /// </summary>
        public bool? NoCollections { get; set; }

        /// <summary>
        /// Date on which the Customer should be bypassed for all Company Rate Increases; defaults to 12/30/1899 if not needed
        /// </summary>
        [StringLength(25)]
        public string OkToIncreaseDate { get; set; }

        /// <summary>
        /// read only; do not set explicitly
        /// </summary>
        public double StatementBalance { get; set; }

        /// <summary>
        /// Set to true to print Site information on Invoices and Statements
        /// </summary>
        public bool? PrintSiteOnInvoices { get; set; }

        /// <summary>
        /// Determines if you want to print Statements for this Customer.  If false, this Customer will be skipped when printing Statements.
        /// </summary>
        public bool? PrintStatements { get; set; }

        /// <summary>
        /// Determines if Cycle Invoices should be printed for this Customer.  If false, then Cycle (Recurring) Invoices will not be printed.
        /// </summary>
        public bool? PrintCycleInvoices { get; set; }

        /// <summary>
        /// Roll up all recurring items to one site.
        /// </summary>
        public bool? RollUpRecurring { get; set; }

        /// <summary>
        /// Generally the installation date of the first site; defaults to today if not supplied
        /// </summary>
        [StringLength(25)]
        public string CustomerSince { get; set; }

        /// <summary>
        /// Value from AR_Branch table; defaults to config setting for branchCode if not supplied; list of valid options can be retrieved from GET: api/branch
        /// </summary>
        [StringLength(25)]
        public string BranchCode { get; set; }

        /// <summary>
        /// Do not set explicitly; pass in GroupCode and this will be validated and set automatically
        /// </summary>
        public int? CustomerGroupID { get; set; }

        /// <summary>
        /// Customer Group security feature; value from AR_Customer_Group table; list of valid options can be retrieved from GET: api/customergroup
        /// </summary>
        [StringLength(25)]
        public string GroupCode { get; set; }

        /// <summary>
        /// Secondary Customer Group; value from AR_Customer_Group table; list of valid options can be retrieved from GET: api/customergroup
        /// </summary>
        [StringLength(25)]
        public string Group2Code { get; set; }

        /// <summary>
        /// Secondary Customer Name
        /// </summary>
        [StringLength(60)]
        public string CustomerName2 { get; set; }

        /// <summary>
        /// Master Account Number this customer should be under (if applicable); value from AR_Master_Account table; defaults to N/A if not supplied; list of valid options can be retrieved from GET: api/masteraccount
        /// </summary>
        [StringLength(25)]
        public string MasterAccountCode { get; set; }

        /// <summary>
        /// Value from AR_Customer_Relation table; for chain accounts (reporting purposes); defaults to N/A if not supplied
        /// </summary>
        [StringLength(25)]
        public string CustomerRelationCode { get; set; }

        /// <summary>
        /// Set to true if this account is a Master Account; will be added to AR_Master_Account table
        /// </summary>
        public bool? IsMaster { get; set; }

        /// <summary>
        /// If set to true, all sub account invoices are billed to Primary Master
        /// </summary>
      //  public bool? MasterAccountBilling { get; set; }

        /// <summary>
        /// The amount currently owed by the customer.  Returned only when getting a single customer.  Not used with adds and updates.
        /// </summary>
        public decimal? BalanceDue { get; set; }

        /// <summary>
        /// The total active RMR for the customer.  Returned only when getting a single customer.  Not used with adds and updates.
        /// </summary>
        public decimal? TotalActiveRMR { get; set; }

        /// <summary>
        /// The total active RAR for the customer.  Returned only when getting a single customer.  Not used with adds and updates.
        /// </summary>
        public decimal? TotalActiveRAR { get; set; }


        //New Fields
        /// <summary>
        /// Customer Name - Customer Number
        /// </summary>
        public string FullNameAndNumber { get; set; }

        public CustomerBill CustomerBillPrimary { get; set; }
    }
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