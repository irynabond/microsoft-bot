using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;

using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;  
using Microsoft.Bot.Builder.Luis;  
using Microsoft.Bot.Builder.Luis.Models;  
using System.Collections.Generic;
using TraitifyAPI;


namespace Bot_Application2
{

    [BotAuthentication]
    public class MessagesController : ApiController
    {
       
        public async Task<Message> Post([FromBody]Message message)
        {

            if (message.Type == "Message")
            {
                message.CreateReplyMessage("Hello, choose a topic");
                Decks decks = new Decks();
                var listOfDecks = string.Join(", ", decks.DeckName().ToArray()); 
                return message.CreateReplyMessage("Hello! Please, choose a topic: " + listOfDecks);
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {

            if (message.Type == "BotAddedToConversation")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Welcome to Traitify bot! I will help you to determine your type of personality. Type begin to start conversation";
                return reply;          
            }
            return null;
        }
    }
}