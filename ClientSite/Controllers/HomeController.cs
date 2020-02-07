using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MessageParser;
using Microsoft.AspNetCore.Mvc;
using static Shared.TagWrapper.TagWrapper;

namespace ClientSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult SendCommand(string guid, string command)
        {
            List<ParsedMessage> parsedMessages = new List<ParsedMessage>();
            parsedMessages.Add(new ParsedMessage() { TagType = TagType.Health, Message = "10/100" });
            parsedMessages.Add(new ParsedMessage() { TagType = TagType.Mana, Message = "20/100" });
            parsedMessages.Add(new ParsedMessage() { TagType = TagType.Stamina, Message = "30/100" });

            string str = TagType.Health.ToString();

            return Json(parsedMessages);
        }
    }
}
