using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Xml;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using zdemo.Services;

namespace zdemo.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> GetData(string address, string city, string state)
        {
            var apiUrl = WebConfigurationManager.AppSettings["APIUrl"];
            var apiKey = WebConfigurationManager.AppSettings["APIKey"];
            
            var service = new APIService();
            var url = string.Format("{0}?zws-id={1}&address={2}&citystatezip={3}", apiUrl, apiKey,
                Server.UrlEncode(address), Server.UrlEncode(string.Format("{0},{1}", city, state)));
            
            var result = await service.GetExternalResponse(url);
            
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(result);
            
            var xmlCommentNodes = xmlDoc.SelectNodes("//comment()");
            foreach (XmlNode node in xmlCommentNodes)
            {
                node.ParentNode.RemoveChild(node);
            }
          
            var resultJson = JsonConvert.SerializeXmlNode(xmlDoc);

            return resultJson;
        }

    }
}