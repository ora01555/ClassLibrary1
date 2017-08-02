using HtmlAgilityPack;
using System.Activities;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace ClassLibrary1
{
    public class Class1 : CodeActivity
    {
        [Category("Input")]
        public InArgument<NameValueCollection> TstCollection { get; set; }

        [Category("Output")]
        public OutArgument<string> TstCollectionRes { get; set; }

        [Category("Output")]
        public OutArgument<string> Response { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            TstCollectionRes.Set(context, string.Join(";", TstCollection.Get(context)));

            using (var client = new WebClient())
            {
                var loginResponse = client.UploadValues("http://edata.customs.ru/FtsPersonalCabinetWeb/Auth/Authenticate", TstCollection.Get(context));

                var setCookie = client.ResponseHeaders["Set-Cookie"].Split(';');

                client.Headers[HttpRequestHeader.Cookie] = $"{setCookie.Single(t => t.StartsWith("ASP.NET_SessionId=")).Trim()}; {setCookie.Single(t => t.Contains("FtsCabinet=")).Trim().Replace("HttpOnly,", string.Empty)}";

                string htmlPage = Encoding.UTF8.GetString(client.DownloadData("http://edata.customs.ru/FtsCabinetServices/Currency/ExportGoodsList.aspx"));
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlPage);
                var n = htmlDoc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATEGENERATOR']").GetAttributeValue("value", string.Empty);
                Response.Set(context, n);

                //client.Headers[HttpRequestHeader.Accept] = @"*/*";
                //client.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                //client.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU";
                //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                //client.Headers[HttpRequestHeader.Referer] = "http://edata.customs.ru/FtsCabinetServices/Currency/ExportGoodsList.aspx";
                //client.Headers["X-Ext.Net"] = "delta=true";
                //client.Headers[HttpRequestHeader.Host] = "edata.customs.ru";


                //var values = new NameValueCollection();
                //values["submitDirectEventConfig"] = @"{""config"":{""extraParams"":{""page"":1,""start"":0,""limit"":3,""sort"":""[{\""property\"":\""CreationDateTime\"",\""direction\"":\""DESC\""}]""}}}";
                //values["ComboRecordsCount"] = @"Все";
                //values["_ComboRecordsCount_state"] = @"[{""value"":""\u0412\u0441\u0435"",""text"":""\u0412\u0441\u0435"",""index"":3}]";
                //values["__EVENTTARGET"] = @"ctl02";
                //values["__EVENTARGUMENT"] = @"StoreProcedures|postback|read";
                //values["__VIEWSTATE"] = @"/wEPDwUJNjU3NjUwNTE3D2QWAgIDD2QWAgIDD2QWBGYPZBYGZg9kFgJmDxYCHgVjbGFzcwUIeC1oaWRkZW4WAgIBDw8WAh4NT25DbGllbnRDbGljawUddG9nZ2xlUmVlZChBcHAuQ3VyckNvbW1lbnRzKTtkZAIBDw8WAh4GSGlkZGVuaGQWAmYPFgIfAAUIeC1oaWRkZW5kAgMPZBYEZg8PFgIeD1NvcnRhYmxlQ29sdW1uc2hkFgRmDw8WAh4NSXNQYWdpbmdTdG9yZWdkZAICD2QWDGYPDxYCHgxNZW51RGlzYWJsZWRnZGQCAQ8PFgIfBWdkZAICDw8WAh8FZ2RkAgMPDxYCHwVnZGQCBA8PFgIfBWdkZAIFDw8WAh8FZ2RkAgIPFgIfAAUIeC1oaWRkZW5kAgEPZBYCZg9kFgJmD2QWAmYPFgIfAAUIeC1oaWRkZW5kGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYOBQVjdGwwMgUFY3RsMDcFEUJ1dHRvbkV4cG9ydEdvb2RzBRNHcmlkUGFuZWxQcm9jZWR1cmVzBQ5DaGVja2JveE9ubHlNeQURQ29tYm9SZWNvcmRzQ291bnQFCmJ0blJlZnJlc2gFEVdpbmRvd1JlcXVlc3RGb3JtBQt0ZkdURFJlZ051bQUFY3RsNTIFDnRmR29vZHNOdW1lcmljBQVjdGw1NwUJYnRuQ2FuY2VsBQdidG5TZW5k7ldCXj5mi+2tJG/rM59eLlLe9nU=";
                //values["__VIEWSTATEGENERATOR"] = @"9B03632D";
                //values["__EVENTVALIDATION"] = @"/wEdAAIszgw0v+s/Cm5XHabgj/mg4q8s12WcMm4dben72M8A3+g0PAX6t5iN6bVSYVscKhavJXAm";
                //values["tfGTDRegNum"] = @"";
                //values["tfGoodsNumeric"] = @"";


                //var jsonResponse = client.UploadValues("http://edata.customs.ru/FtsCabinetServices/Currency/ExportGoodsList.aspx", values);
                //Response.Set(context, Encoding.UTF8.GetString(jsonResponse));
            }

        }
    }
}