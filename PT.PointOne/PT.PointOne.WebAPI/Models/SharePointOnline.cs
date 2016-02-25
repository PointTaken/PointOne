using Microsoft.SharePoint.Client;
using PT.PointOne.WebAPI.Controllers;
using PT.PointOne.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web;

namespace IOTHubInterface.Models
{
    public class SharePointOnline
    {
        public static bool AddNewOrder(Order order)
        {
            try {
                using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms"))
                {
                    ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                    ctx.Load(ctx.Web);
                    ctx.ExecuteQuery();
                    var list = ctx.Web.Lists.GetByTitle("Purchases");
                    ctx.Load(list);
                    ctx.ExecuteQuery();
                    
                    var lic = new ListItemCreationInformation();
                    var item = list.AddItem(lic);
                    item["Title"] = order.RequestId;
                    item["Beer"] = "34;#Hulken IPA"; // TODO: Support more beer
                    item["Price"] = order.Price.ToString();
                    item["Purchased"] = order.Created;
                    item["Hero"] = new FieldUserValue() { LookupId = int.Parse(order.UserId) }; // TODO: Support more users... 
                    item["Served"] = false; 
                    item.Update();
                    list.Update();
                    ctx.ExecuteQuery();
                    return true;
                }
            }catch(Exception)
            {
                return false; 
            }
        }

        public static bool UpdateOrder(Order order)
        {
            try
            {
                using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms"))
                {
                    ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                    ctx.Load(ctx.Web);
                    ctx.ExecuteQuery();
                    var list = ctx.Web.Lists.GetByTitle("Purchases");
                    ctx.Load(list);
                    ctx.ExecuteQuery();

                    var items = list.GetItems(new CamlQuery()
                    {
                        ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + order.RequestId +
                        "</Value></Eq></Where></Query><RowLimit>1</RowLimit></View>"
                    });
                    if (items == null)
                        return false;

                    ctx.Load(items);
                    ctx.ExecuteQuery();

                    var item = items.FirstOrDefault();

                    item["Served"] = true;
                    item.Update();
                    list.Update();
                    ctx.ExecuteQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        private static SecureString GetPWD()
        {
            var pwd = ConfigurationManager.AppSettings["Password"];
            var ss = new SecureString();
            foreach (var c in pwd)
                ss.AppendChar(c);

            return ss;
        }

    }
}