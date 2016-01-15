using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartParkingSystemAPI.Controllers
{
    public class WalletTransController : ApiController
    {
        [HttpPost]
        public bool PostWalletTrans(WalletTran entity)
        {
            using (SmartParkingEntities DBContext = new SmartParkingEntities())
            {
                //UserProfile usr = DBContext.UserProfiles.Find(entity.UserID);
                //usr.WalletAmount = DBContext.WalletTrans.Where(e => e.UserID == entity.UserID && e.CRDR == "CR").Sum(e => e.Amount) -
                //    DBContext.WalletTrans.Where(e => e.UserID == entity.UserID && e.CRDR == "DR").Sum(e => e.Amount);
                //usr.WalletAmount += entity.CRDR == "CR" ? entity.Amount : -entity.Amount;
                entity.TransDt = DateTime.Now;
                DBContext.WalletTrans.Add(entity);
                DBContext.SaveChanges();
                return true;
            }
        }
        [HttpGet]
        public List<WalletTranModel> GetWalletTrans(int userID)
        {
            List<WalletTranModel> result = new List<WalletTranModel>();
            using (SmartParkingEntities context = new SmartParkingEntities())
            {
                result= context.WalletTrans.Where(e => e.UserID == userID).Select(e=> new WalletTranModel{ 
                    Amount=e.Amount, 
                    CRDR = e.CRDR,
                    TransDt = e.TransDt
                }).ToList(); 
            }
            return result;
        }
    }
    public partial class WalletTranModel
    {
        public decimal TransID { get; set; }
        public int UserID { get; set; }
        public Nullable<int> Amount { get; set; }
        public string CRDR { get; set; }
        public Nullable <System.DateTime> TransDt { get; set; }
    }
}
