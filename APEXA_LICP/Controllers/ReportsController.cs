using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using APEXA_LICP.Models;

namespace APEXA_LICP.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult GetContractingChain(string strC1, int c1, string strC2, int c2)
        {

            using (LICPEntities context = new LICPEntities())
            {
                List<Contract> lstlstAllContracts = context.Contract.ToList();

                DataTable dtCt = new DataTable();
                dtCt.Columns.Add("Advisor", typeof(string));
                dtCt.Columns.Add("Carrier", typeof(string));
                dtCt.Columns.Add("MGA", typeof(string));


                foreach (Contract ct in lstlstAllContracts)
                {
                    DataRow row = dtCt.NewRow();
                    if (ct.AdvisorId == null)
                    {
                        row["Advisor"] = "";
                    }
                    else
                    {
                        row["Advisor"] = ct.Advisor.FirstName.ToString().Trim() + " " + ct.Advisor.LastName.ToString().Trim();
                    }
                    if (ct.CarrierId == null)
                    {
                        row["Carrier"] = "";
                    }
                    else
                    {
                        row["Carrier"] = ct.Carrier.BusinessName.ToString().Trim();
                    }
                    if (ct.MGAId == null)
                    {
                        row["MGA"] = "";
                    }
                    else
                    {
                        row["MGA"] = ct.MGA.BusinessName.ToString().Trim();
                    }
                    dtCt.Rows.Add(row);
                }

                string entity1;
                string entity2;
                switch (strC1)
                {
                    case "carrier":
                        
                        entity1 = context.Carriers.FirstOrDefault(a => a.Id == c1).BusinessName;
                        break;
                    case "advisor":
                        entity1 = context.Advisors.FirstOrDefault(a => a.Id == c1).FirstName + " " + context.Advisors.FirstOrDefault(a => a.Id == c1).LastName; 
                        break;
                    case "mga":
                        entity1 = context.MGAs.FirstOrDefault(a => a.Id == c1).BusinessName;
                        break;
                    default:
                        return Content("<div class='alert alert-warning' role='alert'>No entity1 information is given.</div >", "text/html");
                }
                switch (strC2)
                {
                    case "carrier":
                        entity2 = context.Carriers.FirstOrDefault(a => a.Id == c2).BusinessName;
                        break;
                    case "advisor":
                        entity2 = context.Advisors.FirstOrDefault(a => a.Id == c2).FirstName + " " + context.Advisors.FirstOrDefault(a => a.Id == c2).LastName;
                        break;
                    case "mga":
                        entity2 = context.MGAs.FirstOrDefault(a => a.Id == c2).BusinessName;
                        break;
                    default:
                        return Content("<div class='alert alert-warning' role='alert'>No entity2 information is given.</div >", "text/html");
                }
                
                string contractChain = entity1 + " <--> " + entity2;
                /*
                 * 
                 * To find the contract chain between two entities, it is necessary to review the Contracts and find the shortest way to find it.
                 * This becomes an algorithm that recursively compares the possible paths between entities, and finds out how many contracts 
                 * the original entity has, and compare the entities with which it has a contract one by one. After that, look in each of these entities, 
                 * until you find the paths that reach the destination entity.
                 * The algorithm stops when finding the other entity.  The goal is to output the number of steps between entities to get the other entity.
                 * 
                 */



                return Content("<div class='alert alert-success' role='alert'><strong>" + contractChain + "</strong></div>", "text/html");
              
            }
        }
    }
}