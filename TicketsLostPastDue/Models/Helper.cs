using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using System.Net;
using System.IO;
using System.Configuration;
using System.Data;
using System.Collections;
using TicketsLostPastDue;

using System.Data.Entity;

using System.Data.SqlClient;
using TicketsLostPastDue.Models;
using System.Globalization;

namespace TicketsLostPastDue.Models
{

    public class Helper
    {
        //public static List<GetAuthorizationForLeads_Result> GetAuthorizationforLeads<T>(string username, int appid)

        //where T : class
        //{
           
        //    var db = new Cust_SilcoEntities();
        //    List<GetAuthorizationForLeads_Result> authorizations = db.GetAuthorizationForLeads(username, appid).ToList();
        //    return authorizations;
        //}
        public static List<GetADGroups_Result> GetAD_Groups<T>(int appid) where T : class
        {
            var db = new Cust_SilcoEntities();
            List<GetADGroups_Result> groups = db.GetADGroups(appid).ToList();
            //foreach (var grp in groups)
            //{
            //    grp.adsecuritygroupallowed
            //}
            // List<GetAuthorizationForLeads_Result> groups = db.GetAuthorizationForLeads(appid).ToList();  
            return groups;
        }
        public static List<FindSecurityPastDue_Result> FindSecurity<T>(int appid, string title, string dept, string account) where T : class
        {

            var sb = new Cust_SilcoEntities();

            List<FindSecurityPastDue_Result> seclist = sb.FindSecurityPastDue(appid, title, dept, account).ToList();
            return seclist;
        }

    }
}


