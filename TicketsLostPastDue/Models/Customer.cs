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
        public bool? MasterAccountBilling { get; set; }

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


    /// <summary>
    /// 
    /// </summary>
    public class CustomerNote
    {
        /// <summary>
        /// Sedona internal autonumber for the Customer Note record
        /// </summary>
        public int CustomerNoteId { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer table
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// The note
        /// </summary>
        [Required]
        public string Notes { get; set; }

        /// <summary>
        /// the user who initially added the note; defaults to UserCode value from config file if not set explicitly
        /// </summary>
        [StringLength(25)]
        public string UserName { get; set; }

        /// <summary>
        /// Date the note was created
        /// </summary>
        public DateTime? NoteDate { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer_Site table, provide if the message references a site record
        /// </summary>
        public int? CustomerSiteId { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer_System table, provide if the message references a system record
        /// </summary>
        public int? CustomerSystemId { get; set; }

        /// <summary>
        /// Allowed values: 1 = viewed by all, including customers, 2 = viewed by office users, 3 = highest level of security; defaults to 2 if not supplied
        /// </summary>
        public int AccessLevel { get; set; }

        /// <summary>
        /// the user making this change; defaults to UserCode value from config file if not set explicitly
        /// </summary>
        [StringLength(25)]
        public string EditUserCode { get; set; }

        /// <summary>
        /// Date the note was edited
        /// </summary>
        public DateTime? EditDate { get; set; }


    }

    public class CustomerDashboard
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
        [StringLength(60)]
        public string CustomerName { get; set; }

        /// <summary>
        /// Sum of Net Due for open invoices
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// Last Date an invoice was paid
        /// </summary>
        public DateTime? LastPaymentDate { get; set; }

        /// <summary>
        /// Amount of last invoice payment
        /// </summary>
        public decimal? LastPaymentAmount { get; set; }

        /// <summary>
        /// Next date an invoice is due
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// Number of open Service Tickets (not closed, i.e. OP, RS, SC, IP, GB, etc.)
        /// </summary>
        public int OpenServiceTicketCount { get; set; }

        /// <summary>
        /// Customer is signed up for Paperless billing, i.e. AR_Customer.Print_Cycle_Invoices = "N"
        /// </summary>
        public bool PaperlessBilling { get; set; }

        /// <summary>
        /// Customer has a bank or credit card record where UsedForAutoProcess = "Y"
        /// </summary>
        public bool AutoBillPay { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomerSubAccount
    {
        /// <summary>
        /// Sedona internal autonumber for the Customer record
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Foreign key from AR_Customer_Site table
        /// </summary>
        public int CustomerSiteId { get; set; }

        /// <summary>
        /// Sedona internal autonumber for the AR Customer System record (AR_Customer_System table); required for PUT
        /// </summary>
        public int? CustomerSystemId { get; set; }

        /// <summary>
        /// Customer Number the customer sees
        /// </summary>
        [StringLength(15)]
        public string CustomerNumber { get; set; }

        /// <summary>
        /// If a person, CustomerName should be passed in as "Last, First"
        /// </summary>
        [StringLength(60)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteCity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteZip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AlarmAccount { get; set; }
    }
}