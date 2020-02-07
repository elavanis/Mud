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

            List<Tuple<string, string>> tuples = ConvertParsedMessageToTuples(parsedMessages);


            return Json(tuples);
        }

        private List<Tuple<string, string>> ConvertParsedMessageToTuples(List<ParsedMessage> parsedMessages)
        {
            List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();

            foreach (ParsedMessage parsedMessage in parsedMessages)
            {
                tuples.Add(new Tuple<string, string>(parsedMessage.TagType.ToString(), parsedMessage.Message));
            }

            return tuples;
        }
    }
}
