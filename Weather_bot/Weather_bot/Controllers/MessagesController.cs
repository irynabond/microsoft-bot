using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using System.Xml.Linq;
using WeatherAPI;


namespace Bot_Application2
{
    

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<Message> Post([FromBody]Message message)
        {
                string text = message.Text;
                string result =  Weather.WeatherInfo(text);
                return message.CreateReplyMessage(result);        
        }       
    }
}