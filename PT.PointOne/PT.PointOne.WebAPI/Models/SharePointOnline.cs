using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using PT.PointOne.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;

namespace PT.PointOne.WebAPI
{
    public class SharePointOnline
    {
        public static bool AddNewOrder(Order order)
        {
            try
            {
                using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms"))
                {
                    ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                    ctx.Load(ctx.Web);
                    ctx.ExecuteQuery();
                    var list = ctx.Web.Lists.GetByTitle("Purchases");
                    var beerList = ctx.Web.Lists.GetByTitle("Beers");
                    ctx.Load(list);
                    ctx.Load(beerList);
                    ctx.ExecuteQuery();

                    var beer = beerList.GetItemById(order.ProductId);
                    ctx.Load(beer);
                    ctx.ExecuteQuery();

                    var lic = new ListItemCreationInformation();
                    var item = list.AddItem(lic);
                    item["Title"] = order.RequestId;
                    item["Beer"] = string.Format("{1};#{0}", beer["Title"], beer.Id); // "34;#Hulken IPA"; // TODO: Support more beer
                    item["Price"] = order.Price.ToString();
                    item["Purchased"] = order.Created;
                    item["Hero"] = new FieldUserValue() { LookupId = int.Parse(order.UserId) }; // TODO: Support more users... 
                    item["Served"] = false;
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

        public static List<Stock> StockList
        {
            get
            {
                var Stock = new List<Stock>();
                try
                {
                    using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms"))
                    {
                        ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                        ctx.Load(ctx.Web);
                        ctx.ExecuteQuery();
                        var list = ctx.Web.Lists.GetByTitle("Stock");
                        ctx.Load(list);
                        ctx.ExecuteQuery();

                        var items = list.GetItems(new CamlQuery());
                        if (items == null)
                            return Stock;
                        
                        ctx.Load(items);
                        ctx.ExecuteQuery();
                        var beerlist = BeerList; 
                        foreach (var item in items)
                        {                           
                            var beerid = ((FieldLookupValue)item["Beer"]).LookupId;
                            Stock.Add(new Stock()
                            {
                                amount = double.Parse(item["Amount"].ToString()),
                                beer = beerlist.Where(k => k.ID == beerid).FirstOrDefault()
                            });
                        }


                        return Stock;
                    }
                }
                catch (Exception ex)
                {
                    return new List<Stock>();
                }
            }
        }

        private static List<Beer> BeerCache;
        private static DateTime BeerCacheInvalidation; 
        public static List<Beer> BeerList
        {
            get
            {
                if(BeerCache != null && BeerCache.Count > 0)
                {
                    if(BeerCacheInvalidation != null)
                    {
                        if (DateTime.Now.Subtract(BeerCacheInvalidation).TotalMinutes <= 5)
                            return BeerCache; 
                    }
                }

                var Beers = new List<Beer>();
                try
                {
                    using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms"))
                    {
                        ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                        ctx.Load(ctx.Web);
                        ctx.ExecuteQuery();
                        var list = ctx.Web.Lists.GetByTitle("Beers");
                        ctx.Load(list);
                        ctx.ExecuteQuery();

                        var items = list.GetItems(new CamlQuery());
                        if (items == null)
                            return Beers;

                        ctx.Load(items);
                        ctx.ExecuteQuery();

                        foreach (var item in items)
                        {                    
                            Beers.Add(new Beer()
                            {
                                Alcohol = double.Parse((item["Alcohol"] ?? string.Empty).ToString()),
                                Bitterness = double.Parse((item["Bitterness"] ?? string.Empty).ToString()),
                                Brewery = ((FieldLookupValue)item["Brewery"]).LookupId.ToString(),
                                Colour = item["Colour"].ToString(),
                                Country = ((TaxonomyFieldValue)item["Country"]).Label,
                                Freshness = double.Parse((item["Freshness"] ?? string.Empty).ToString()),
                                Out = double.Parse((item["Out"] ?? string.Empty).ToString()),
                                Title = item["Title"].ToString(),
                                ID = int.Parse(item["ID"].ToString())
                            });
                        }
                        BeerCache = Beers;
                        BeerCacheInvalidation = DateTime.Now; 
                        return Beers;
                    }
                }
                catch (Exception ex)
                {
                    return new List<Beer>();
                }
            }
        }


        public static List<Beer> GetBeersByCountry(string country)
        {
            var beers = new List<Beer>();
            using (var ctx = new ClientContext("https://aspc1606.sharepoint.com/sites/PointOneArms"))
            {
                ctx.Credentials = new SharePointOnlineCredentials("hs@aspc1606.onmicrosoft.com", GetPWD());
                ctx.Load(ctx.Web);
                ctx.ExecuteQuery();
                var list = ctx.Web.Lists.GetByTitle("Beers");
                ctx.Load(list);
                ctx.ExecuteQuery();
                var beerItems = list.GetItems(new CamlQuery { ViewXml = string.Format(@"<View>
                            <Query>
                                <Where>
                                    <Eq>
                                        <FieldRef Name='Country'/>
                                        <Value Type='TaxonomyFieldType'>{0}</Value>
                                    </Eq>
                                </Where>
                            </Query>
                        </View>", country) });
                ctx.Load(beerItems);
                ctx.ExecuteQuery();
                foreach (var beer in beerItems)
                {
                    ctx.Load(beer);
                    ctx.ExecuteQuery();
                    beers.Add(new Beer()
                    {
                        Alcohol = double.Parse((beer["Alcohol"] ?? string.Empty).ToString()),
                        Bitterness = double.Parse((beer["Bitterness"] ?? string.Empty).ToString()),
                        Brewery = ((FieldLookupValue)beer["Brewery"]).LookupId.ToString(),
                        Colour = beer["Colour"].ToString(),
                        Country = ((TaxonomyFieldValue)beer["Country"]).Label,
                        Freshness = double.Parse((beer["Freshness"] ?? string.Empty).ToString()),
                        Out = double.Parse((beer["Out"] ?? string.Empty).ToString()),
                        Title = beer["Title"].ToString()
                    });
                }
            }
            return beers;
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


