using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using APEXA_LICP.Models;

namespace APEXA_LICP.Controllers
{
    public class ContractingController : Controller
    {
        // GET: Contracting
        public ActionResult Index()
        {
            using (LICPEntities context = new LICPEntities())
            {
                List<Contract> lstContracts = context.Contract.ToList();

                DataTable dtContracts = new DataTable();
                dtContracts.Columns.Add("Id", typeof(int));
                dtContracts.Columns.Add("Advisor", typeof(string));
                dtContracts.Columns.Add("Carrier", typeof(string));
                dtContracts.Columns.Add("MGA", typeof(string));
                dtContracts.Columns.Add("ContractDate", typeof(DateTime));


                foreach (Contract c in lstContracts)
                {
                    DataRow row = dtContracts.NewRow();
                    row["Id"] = c.Id;
                    if (c.AdvisorId == null)
                    {
                        row["Advisor"] = "";
                    }
                    else
                    {
                        row["Advisor"] = c.Advisor.FirstName + " " + c.Advisor.LastName;
                    }
                    if (c.CarrierId == null)
                    {
                        row["Carrier"] = "";
                    }
                    else
                    {
                        row["Carrier"] = c.Carrier.BusinessName;
                    }
                    if (c.MGAId == null)
                    {
                        row["MGA"] = "";
                    }
                    else
                    {
                        row["MGA"] = c.MGA.BusinessName;
                    }
                    if (c.ContractDate == null)
                    {
                        row["ContractDate"] = "";
                    }
                    else
                    {
                        row["ContractDate"] = c.ContractDate;
                    }
                    dtContracts.Rows.Add(row);
                }

                ViewBag.Contracts = dtContracts;
            }

            return View();
        }

        public List<Advisor> getAdvisorsList()
        {
            List<Advisor> lstAdvisors = new List<Advisor>();
            using (LICPEntities context = new LICPEntities())
            {
                lstAdvisors = context.Advisors.ToList();
            }
            return lstAdvisors;
        }

        public List<MGA> getMGAsList()
        {
            List<MGA> lstMGAs = new List<MGA>();
            using (LICPEntities context = new LICPEntities())
            {
                lstMGAs = context.MGAs.ToList();
            }
            return lstMGAs;
        }

        public List<Carrier> getCarriersList()
        {
            List<Carrier> lstCarriers = new List<Carrier>();
            using (LICPEntities context = new LICPEntities())
            {
                lstCarriers = context.Carriers.ToList();
            }
            return lstCarriers;
        }

        public ActionResult GetContractorList(string name)
        {

            using (LICPEntities context = new LICPEntities())
            {
                switch (name)
                {
                    case "carrier":

                        List<Carrier> lstCarriers = context.Carriers.ToList();
                        ViewBag.Carriers = new SelectList(lstCarriers, "Id", "BusinessName");
                        return PartialView("DisplayCarriers");

                    case "advisor":
                        var lstAdvisors = (from a in context.Advisors
                                           select new
                                           {
                                               a.Id,
                                               Name = a.FirstName + " " + a.LastName
                                           }).ToList();
                        ViewBag.Advisor = new SelectList(lstAdvisors, "Id", "Name");
                        return PartialView("DisplayAdvisors");
                    case "mga":
                        List<MGA> lstMgas = context.MGAs.ToList();
                        ViewBag.Mgas = new SelectList(lstMgas, "Id", "BusinessName");
                        return PartialView("DisplayMgas");
                    default:

                        return Content("");
                }
            }


        }

        public bool EstablishContract(string strC1, int c1, string strC2, int c2)
        {

            using (LICPEntities context = new LICPEntities())
            {
                if (strC1 == strC2)
                {
                    ViewBag.Message = "It is not possible to contract with self";
                    return false;
                }


                string entity1;
                string entity2;
                switch (strC1)
                {
                    case "carrier":
                        entity1 = "CarrierId=";
                        break;
                    case "advisor":

                        entity1 = "AdvisorId=";

                        break;
                    case "mga":
                        entity1 = "MGAId=";
                        break;
                    default:
                        ViewBag.Message = "No entity1 information is given.";
                        return false;
                }
                switch (strC2)
                {
                    case "carrier":
                        entity2 = "CarrierId=";
                        break;
                    case "advisor":
                        entity2 = "AdvisorId=";
                        break;
                    case "mga":
                        entity2 = "MGAId=";
                        break;
                    default:
                        ViewBag.Message = "No entity2 information is given.";
                        return false;
                }

                string query = "SELECT Id FROM Contracts WHERE " + entity1 + c1.ToString() + " AND " + entity2 + c2.ToString();

                int ctrId = context.Database.SqlQuery<int>(query).FirstOrDefault();

                if (ctrId == 0)
                {
                    int contractId = 0;
                    var lstContracts = context.Contract.ToList();
                    if (lstContracts.Any())
                    {
                        contractId = (from ct in context.Contract
                                      orderby ct.Id
                                      descending
                                      select ct.Id).First();
                    }
                    contractId++;

                    Contract c = new Contract();
                    c.Id = contractId;
                    switch (strC1)
                    {
                        case "carrier":
                            c.CarrierId = c1;
                            break;
                        case "advisor":

                            c.AdvisorId = c1;

                            break;
                        case "mga":

                            c.MGAId = c1;
                            break;
                        default:
                            ViewBag.Message = "No entity1 information is given.";
                            return false;
                    }
                    switch (strC2)
                    {
                        case "carrier":
                            c.CarrierId = c2;

                            break;
                        case "advisor":

                            c.AdvisorId = c2;

                            break;
                        case "mga":

                            c.MGAId = c2;
                            break;
                        default:
                            ViewBag.Message = "No entity2 information is given.";
                            return false;
                    }
                    c.ContractDate = DateTime.Now;
                    context.Contract.Add(c);
                    context.SaveChanges();
                    return true;

                }
                else
                {
                    ViewBag.Message = "This contract already exists!";
                    return false;
                }
            }

        }


        public bool TerminateContract(int contractId)
        {



            return false;
        }


        public ActionResult GetContractsList()
        {
            using (LICPEntities context = new LICPEntities())
            {
                List<Contract> lstContracts = context.Contract.ToList();

                DataTable dtContracts = new DataTable();
                dtContracts.Columns.Add("Id", typeof(int));
                dtContracts.Columns.Add("Advisor", typeof(string));
                dtContracts.Columns.Add("Carrier", typeof(string));
                dtContracts.Columns.Add("MGA", typeof(string));
                dtContracts.Columns.Add("ContractDate", typeof(DateTime));


                foreach (Contract c in lstContracts)
                {
                    DataRow row = dtContracts.NewRow();
                    row["Id"] = c.Id;
                    if (c.AdvisorId == null)
                    {
                        row["Advisor"] = "";
                    }
                    else
                    {
                        row["Advisor"] = c.Advisor.FirstName + " " + c.Advisor.LastName;
                    }
                    if (c.CarrierId == null)
                    {
                        row["Carrier"] = "";
                    }
                    else
                    {
                        row["Carrier"] = c.Carrier.BusinessName;
                    }
                    if (c.MGAId == null)
                    {
                        row["MGA"] = "";
                    }
                    else
                    {
                        row["MGA"] = c.MGA.BusinessName;
                    }

                    if (c.ContractDate == null)
                    {
                        row["ContractDate"] = "";
                    }
                    else
                    {
                        row["ContractDate"] = c.ContractDate;
                    }

                    dtContracts.Rows.Add(row);
                }

                ViewBag.Contracts = dtContracts;
            }

            return PartialView("DisplayContractsList");
        }

        // GET: Advisors/Delete/5
        public ActionResult DeleteContract(int? id)
        {
            using (LICPEntities context = new LICPEntities())
            {
                Contract contract = context.Contract.Find(id);
                context.Contract.Remove(contract);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}