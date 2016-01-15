using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Models
{
    public class WalletTransModel
    {
        public decimal TransID { get; set; }
        public int UserID { get; set; }
        public int Amount { get; set; }
        public string CRDR { get; set; }
        public System.DateTime TransDt { get; set; }
    }
}
