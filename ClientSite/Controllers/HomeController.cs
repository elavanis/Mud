using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClientTelentCommucication;
using MessageParser;
using Microsoft.AspNetCore.Mvc;
using TelnetCommunication;
using static Shared.TagWrapper.TagWrapper;

namespace ClientSite.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, ClientHandler> clientHandlers = new Dictionary<string, ClientHandler>();
        private static DateTime dateTime = DateTime.Now;

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult SendCommand(string guid, string command)
        {
            ClientHandler clientHandler = null;
            lock (clientHandlers)
            {
                if (!clientHandlers.TryGetValue(guid, out clientHandler))
                {
                    clientHandler = new ClientHandler("127.0.0.1", 52475, new JsonMudMessage());
                    clientHandlers.Add(guid, clientHandler);
                }

                if (DateTime.Now.Subtract(dateTime).TotalMinutes >= 10)
                {
                    RemoveDeadConnections();
                }
            }

            if (!string.IsNullOrEmpty(command))
            {
                clientHandler.OutQueue.Enqueue(command);
            }

            List<ParsedMessage> parsedMessages = new List<ParsedMessage>();

            string message;
            while (clientHandler.InQueue.TryDequeue(out message))
            {
                foreach (var parsedMessage in Parser.Parse(message))
                {
                    if (parsedMessage.TagType == TagType.Sound)
                    {
                        //handle sound
                    }
                    else if (parsedMessage.TagType == TagType.Map)
                    {
                        //update map

                        //jquery 
                        //https://stackoverflow.com/questions/623172/how-to-get-image-size-height-width-using-javascript
                        //imageElement.naturalHeight & imageElement.naturalWidth

                        parsedMessages.Add(parsedMessage);
                    }
                    else if (parsedMessage.TagType == TagType.Data)
                    {
                        //not going to local cache stuff so n/a?
                    }
                    else if (parsedMessage.TagType == TagType.FileValidation)
                    {
                        //not going to local cache stuff so n/a?
                    }
                    else
                    {
                        parsedMessages.Add(parsedMessage);
                    }
                }
            }

            List<Tuple<string, string>> tuples = ConvertParsedMessageToTuples(parsedMessages);

            return Json(tuples);
        }

        private static void RemoveDeadConnections()
        {
            List<string> copiedKeys = new List<string>(clientHandlers.Keys);
            foreach (string key in copiedKeys)
            {
                if (!clientHandlers[key].Connected)
                {
                    clientHandlers.Remove(key);
                }
            }
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
