using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core.Utils
{
    public class LogonInstagram
    {

        static string postData = string.Empty;
        static string mid = string.Empty;
        static string csrtoken = string.Empty;
        static string sessionid = string.Empty;
        static string urlForceLogin = string.Empty;

        public async Task<string> GetToken()
        {
            try
            {
                OAuth auth = new OAuth();
                var linkGet = auth.AuthLink();

                if (!String.IsNullOrEmpty(linkGet))
                {
                    //Call de First URL and recover cokies (mid, scrToken) to use in the POST
                    string secondLink = await this.GetFormAndRecorverCookies(linkGet);

                    if (!String.IsNullOrEmpty(secondLink))
                    {
                        //Post Form with credencial and recover cookies and the last Url to get Token
                        string thirdLink = await this.PostFormWithCredencials(secondLink);

                        if (!String.IsNullOrEmpty(thirdLink))
                        {
                            //Call the last URL to get Token
                            return await GetToken(thirdLink);
                        }
                        else
                            return string.Empty;

                    }
                    else
                        return string.Empty;
                }
                else
                    return String.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private async Task<string> GetToken(string thirdLink)
        {        
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(thirdLink);
            request.Host = "www.instagram.com";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            request.Headers.Add("Cookie", "mid=" + mid + "; csrftoken=" + csrtoken + "; sessionid=" + sessionid);
            request.Referer = urlForceLogin;
            request.AllowAutoRedirect = false;

            string urlToken = string.Empty;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                urlToken = response.Headers.Get("Location");
            }

            if (!string.IsNullOrEmpty(urlToken))
            {
                string[] arrayValores = new string[] { "code=" };
                var resultado = urlToken.Split(arrayValores, StringSplitOptions.None);

                if (resultado.Length == 2)
                {
                    var token = resultado[1];

                    if (!string.IsNullOrEmpty(token))
                        return token;
                }
            }
            
            return urlToken;
        }

        private async Task<string> PostFormWithCredencials(string urlAction)
        {
            urlForceLogin = String.Concat("https://www.instagram.com", urlAction);

            string userApp = ConfigurationManager.AppSettings["InstagramUserApp"];
            string passwordApp = ConfigurationManager.AppSettings["InstagramPasswordApp"];

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlForceLogin);
            String response_ = String.Empty;
            postData = "username=" + userApp;
            postData += "&password=" + passwordApp;
            postData += "&csrfmiddlewaretoken=" + csrtoken;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";
            request.Headers.Add("authority", "www.instagram.com");
            request.Headers.Add("origin", "www.instagram.com");
            request.Headers.Add("path", urlAction);
            request.Headers.Add("scheme", "https");
            request.Headers.Add("Cookie", "mid=" + mid + "; csrftoken=" + csrtoken);
            request.Referer = urlForceLogin;
            request.AllowAutoRedirect = false;
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                if (response.Headers != null)
                {
                    String cookies = response.Headers.Get("Set-Cookie");

                    if (!String.IsNullOrEmpty(cookies))
                    {
                        Dictionary<String, String> valores = new Dictionary<string, string>();

                        string[] arrayValores = new string[] { "sessionid=", "csrftoken=" };

                        var resultado = cookies.Split(arrayValores, StringSplitOptions.None);

                        if (resultado.Length == 3)
                        {
                            csrtoken = resultado[1].Split(';')[0];
                            sessionid = resultado[2].Split(';')[0];
                        }

                    }

                    return response.Headers.Get("Location");
                }
            }

            return string.Empty;
        }

        private async Task<string> GetFormAndRecorverCookies(string linkGet)
        {
            string urlAction = String.Empty;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(linkGet);
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36";

            using (var resp = req.GetResponse())
            {
                if (resp.Headers != null)
                {
                    String cookies = resp.Headers.Get("Set-Cookie");

                    // Recover and save token to send in next POST
                    if (!String.IsNullOrEmpty(cookies))
                    {
                        Dictionary<String, String> valores = new Dictionary<string, string>();
                        string[] arrayValores = new string[] { "mid=", "csrftoken=" };
                        var result = cookies.Split(arrayValores, StringSplitOptions.None);
                        if (result.Length == 3)
                        {
                            csrtoken = result[1].Split(';')[0];
                            mid = result[2].Split(';')[0];
                        }
                    }
                }

                var html = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                HtmlDocument resultHtml = new HtmlDocument();
                resultHtml.LoadHtml(html);

                //Recover url action in the Form
                var form = resultHtml.GetElementbyId("login-form");
                if (form != null)
                {
                    if (form.Attributes != null && form.Attributes.Count > 0)
                    {
                        foreach (var item in form.Attributes)
                        {
                            if (item.Name == "action")
                            {
                                urlAction = item.Value;
                                break;
                            }
                        }
                    }
                }

                return urlAction;
            }
        }
    }
}
