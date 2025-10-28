using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giadinhthoxinh.Models
{
	public class VnPaymentResponseModel
	{
        public bool Success { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}