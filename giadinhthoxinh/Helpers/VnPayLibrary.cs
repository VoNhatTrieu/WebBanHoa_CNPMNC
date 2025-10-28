using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace giadinhthoxinh.Helpers
{
    public class VnPayLibrary
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : "";
        }

        public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }

            var querystring = data.ToString();
            baseUrl += "?" + querystring;

            if (querystring.Length > 0)
            {
                querystring = querystring.TrimEnd('&');
            }

            var vnpSecureHash = Utils.HmacSHA512(vnpHashSecret, querystring);
            baseUrl += "vnp_SecureHash=" + vnpSecureHash;

            return baseUrl;
        }

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            var raw = GetResponseRaw();
            var myChecksum = Utils.HmacSHA512(secretKey, raw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private string GetResponseRaw()
        {
            var data = new StringBuilder();

            foreach (var kv in _responseData)
            {
                if (kv.Key == "vnp_SecureHash" || kv.Key == "vnp_SecureHashType")
                    continue;
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
            }

            if (data.Length > 0)
                data.Length -= 1;

            return data.ToString();
        }
    }
}
