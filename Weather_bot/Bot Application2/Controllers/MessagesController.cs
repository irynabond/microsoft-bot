using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;

using Newtonsoft.Json;
using System.Xml.Linq;
using WeatherAPI;
using Microsoft.Bot.Builder.Dialogs; 
using Microsoft.Bot.Builder.Luis; 
using Microsoft.Bot.Builder.Luis.Models; 
using System.Collections.Generic;



namespace Weather_Bot
{
    [LuisModel("c413b2ef-382c-45bd-8ff0-f76d60e2a821", "6d0966209c6e4f6b835ce34492f3e6d9")]
    [Serializable]

    public class WeatherDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            try
            {
                int zip = Int32.Parse(result.Query);
                string message = Weather.WeatherInfo(zip);
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
            catch
            {
                string message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
                await context.PostAsync(message);
                context.Wait(MessageReceived);
            }
           
        }

        [LuisIntent("builtin.intent.weather.question_weather")]
        [LuisIntent("builtin.intent.weather.check_weather")]
        public async Task Checking(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("What is your zip code?");
            context.Wait(MessageReceived);
           
        }

    

        [BotAuthentication]
        public class MessagesController : ApiController
        {
            public async Task<Message> Post([FromBody]Message message)
            {
                return await Conversation.SendAsync(message, () => new WeatherDialog());
            }
        }
    }
}


//public async Task<Message> Post([FromBody]Message message)
//{
//    string text = message.Text;
//    string result = Weather.WeatherInfo(text);
//    return message.CreateReplyMessage(result);
//}