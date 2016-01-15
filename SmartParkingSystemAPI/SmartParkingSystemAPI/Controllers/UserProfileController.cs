using SmartParkingSystemAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
using ASPSnippets.SmsAPI;
using System.Net;
using System.Collections.Specialized;

namespace SmartParkingSystemAPI.Controllers
{
    public class UserProfileController : ApiController
    {
        private SmartParkingEntities _DBContext;
        [Authorize(Roles="Admin")]
        public List<UserProfileModel> GetUserProfileList()
        {
            //GetOTP("919096868317");
            List<UserProfileModel> result = new List<UserProfileModel>();
            using (SmartParkingEntities context = new SmartParkingEntities())
            {
                var qry = context.UserProfiles.ToList();

                if (qry.Count() > 0)
                {
                    result = qry.Select(u => new UserProfileModel()
                    {
                        ActiveVehicleNumber = u.ActiveVehicleNumber,
                        Address1 = u.Address1,
                        Address2 = u.Address2,
                        City = u.City,
                        EmailID = u.EmailID,
                        FirstName = u.FirstName,
                        IsActive = u.IsActive,
                        LastName = u.LastName,
                        MobileNumber = u.MobileNumber,
                        Pincode = u.Pincode,
                        State = u.State,
                        UserID = u.UserID
                    }).ToList();
                }
            }
            return result;
        }
        public UserProfileModel GetUserProfile(int inputId)
        {
            UserProfileModel result = new UserProfileModel();
            using (SmartParkingEntities context = new SmartParkingEntities())
            {
                var usr = context.UserProfiles.Where(u => u.UserID == inputId).SingleOrDefault();
                if (usr != null)
                {
                    result = ConvertDBToModelObject(usr);
                }
            }
            return result;
        }

        public int PostUserProfile(UserProfile entity)
        {
            using (_DBContext = new SmartParkingEntities())
            {
                UserProfile thisEntity = _DBContext.UserProfiles.Find(entity.UserID);
                if (thisEntity == null)
                    CreateUserProfile(entity);
                else
                    thisEntity = entity;
                _DBContext.SaveChanges();
            }
            return entity.UserID;
        }
        private void CreateUserProfile(UserProfile entity)
        {
            entity.WalletAmount = 100;
            _DBContext.UserProfiles.Add(entity);
            _DBContext.WalletTrans.Add(new WalletTran { CRDR = "CR", Amount = 100, TransDt = DateTime.Now });
        }
        public UserProfileModel ConvertDBToModelObject(UserProfile usr)
        {
            return new UserProfileModel()
             {
                 ActiveVehicleNumber = usr.ActiveVehicleNumber,
                 Address1 = usr.Address1,
                 Address2 = usr.Address2,
                 City = usr.City,
                 EmailID = usr.EmailID,
                 FirstName = usr.FirstName,
                 IsActive = usr.IsActive,
                 LastName = usr.LastName,
                 MobileNumber = usr.MobileNumber,
                 Pincode = usr.Pincode,
                 State = usr.State,
                 UserID = usr.UserID
             };
        }

        public List<UserProfileModel> SearchUserProfile(UserSearchModel model)
        {
            List<UserProfileModel> result = new List<UserProfileModel>();
            using (SmartParkingEntities context = new SmartParkingEntities())
            {
                var qry = context.UserProfiles.Where(s => s.IsActive == true).ToList();

                if (model != null)
                {
                    if (model.UserName != null)
                        qry = qry.Where(u => u.FirstName.Contains(model.UserName)).ToList();
                    if (model.City != null)
                        qry = qry.Where(u => u.City.Contains(model.City)).ToList();
                    if (model.Email != null)
                        qry = qry.Where(u => u.EmailID.Contains(model.Email)).ToList();
                }

                if (qry.Count() > 0)
                {
                    result = qry.Select(u => new UserProfileModel()
                    {
                        ActiveVehicleNumber = u.ActiveVehicleNumber,
                        Address1 = u.Address1,
                        Address2 = u.Address2,
                        City = u.City,
                        EmailID = u.EmailID,
                        FirstName = u.FirstName,
                        IsActive = u.IsActive,
                        LastName = u.LastName,
                        MobileNumber = u.MobileNumber,
                        Pincode = u.Pincode,
                        State = u.State,
                        UserID = u.UserID
                    }).ToList();
                }
            }
            return result;
        }
        public string GetOTP(string MobileNo)  
        {
            string result;
            string OTP = "";
            string Message = "";
            string hashKey = "02fe658139ca1dafb8d2d4f9bb17027e4d364b2a";
            string userName = "vijaybkodare@gmail.com";
            OTP = GetRandomOTP();
            //Message = "OTP for Smart Parking app Registration: " + OTP;
            //Message = HttpUtility.UrlEncode(Message);
            //using (var wb = new WebClient())
            //{
            //    byte[] response = wb.UploadValues("http://api.textlocal.in/send/", new NameValueCollection()
            //    {
            //    {"username" , userName},
            //    {"hash" , hashKey},
            //    {"numbers" , MobileNo},
            //    {"message" , Message},
            //    {"sender" , "TXTLCL"}
            //    });
            //    result = System.Text.Encoding.UTF8.GetString(response);
            //}
            return OTP;
        }
        public int GetWalletAmount(int userID)
        {
            using (SmartParkingEntities DBContext = new SmartParkingEntities())
            {
                UserProfile usr = DBContext.UserProfiles.Find(userID);
                usr.WalletAmount = DBContext.WalletTrans.Where(e => e.UserID == userID && e.CRDR == "CR").Sum(e => e.Amount).GetValueOrDefault(0)
                - DBContext.WalletTrans.Where(e => e.UserID == userID && e.CRDR == "DR").Sum(e => e.Amount).GetValueOrDefault(0);
                return usr.WalletAmount.GetValueOrDefault(0);
            }
        }
        private string GetRandomOTP()
        {
            Random rndm = new Random();
            return rndm.Next(100000, 999999).ToString(); 
        }
    }
}
