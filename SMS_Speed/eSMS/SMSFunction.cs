using Newtonsoft.Json;
using SMS_Speed.DTO;
using SMS_Speed.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS_Speed.eSMS
{
    internal class SMSFunction
    {
        public static bool Send(string phone)
        {
            string APIKey = "E0BF300460D2DE65489DB31934A7B4";
            string SecretKey = "3219839723390BC6133415A002FA2D";

            string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V4/";
            UTF8Encoding encoding = new UTF8Encoding();

            string strResult = string.Empty;

            string customers = "";

            string[] lstPhone = phone.Split(',');

            for (int i = 0; i < lstPhone.Count(); i++)
            {
                customers = customers + @"<CUSTOMER>"
                                + "<PHONE>" + lstPhone[i] + "</PHONE>"
                                + "</CUSTOMER>";
            }
            string SampleXml = @"<RQST>"
                               + "<APIKEY>" + APIKey + "</APIKEY>"
                               + "<SECRETKEY>" + SecretKey + "</SECRETKEY>"
                               + "<ISFLASH>0</ISFLASH>"
                               + "<BRANDNAME>Baotrixemay</BRANDNAME>"  //De dang ky brandname rieng vui long lien he hotline 0902435340 hoac nhan vien kinh Doanh cua ban                                
                               + "<SMSTYPE>2</SMSTYPE>"//SMSTYPE 3: đầu số ngẫu nhiên tốc độ chậm, SMSTYPE=7: đầu số ngẫu nhiên tốc độ cao, SMSTYPE=4: Đầu số 19001534; SMSTYpe=6: đàu số 8755                               
                               + "<CONTENT>" + "" + "</CONTENT>"
                               + "<CONTACTS>" + customers + "</CONTACTS>"
           + "</RQST>";

            string postData = SampleXml.Trim().ToString();
            // convert xmlstring to byte using ascii encoding
            byte[] data = encoding.GetBytes(postData);
            // declare httpwebrequet wrt url defined above
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
            // set method as post
            webrequest.Method = "POST";
            webrequest.Timeout = 500000;
            // set content type
            webrequest.ContentType = "application/x-www-form-urlencoded";
            // set content length
            webrequest.ContentLength = data.Length;
            // get stream data out of webrequest object
            Stream newStream = webrequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // declare & read response from service
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

            // set utf8 encoding
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            // read response stream from response object
            StreamReader loResponseStream =
                new StreamReader(webresponse.GetResponseStream(), enc);
            // read string from stream data
            strResult = loResponseStream.ReadToEnd();
            // close the stream object
            loResponseStream.Close();
            // close the response object
            webresponse.Close();
            // below steps remove unwanted data from response string
            strResult = strResult.Replace("</string>", "");

            return true;
        }

        public static string CheckBalance()
        {
            object body = new
            {
                ApiKey = Config.ReadConfigFile()["ApiKey"],
                SecretKey = Config.ReadConfigFile()["SecretKey"]
            };

            string jsonBody = JsonConvert.SerializeObject(body);
            string url = "http://rest.esms.vn/MainService.svc/json/GetBalance_json";
            string result;
            string err = CallAPIController.CallAPI(url, jsonBody, "application/json", "POST", out result);
            if (!err.Equals(""))
            {
                MessageBox.Show(err, "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            BalanceDTO balance = JsonConvert.DeserializeObject<BalanceDTO>(result);
            if (balance.CodeResponse == 100)
            {
                return balance.Balance + "VND";
            }
            else return "Error";
        }
        
}
}
