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
using com.traitify.net.TraitifyLibrary;


namespace Bot_Application2
{
    [Serializable]
    public class PersonalityDialog : IDialog<object>
    {
        bool showDecks = false;
        bool slideStarted = false;
        int index = 0;
        //List<Slide> copy;
        List<Slide> slideCollection;
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)
        {

            if (showDecks == false)
            {
                showDecks = true;
                TestType names = new TestType();
                var listOfDecks = string.Join(", ", names.GetNames().ToArray());
                await context.PostAsync("Hello! Please, choose a topic: " + listOfDecks);
                context.Wait(MessageReceivedAsync);

            }
            else
            {
                if (slideStarted == false)
                {
                    slideStarted = true;
                    var message = await argument;
                    TestType slides = new TestType();
                    slideCollection = slides.GetSlides(message.Text);
                   // copy = slideCollection;
                    await context.PostAsync("Type omething to start");
                    context.Wait(MessageReceivedAsync);
                
                } else
                {
                    string name = slideCollection[index].caption;
                    index++;
                    await context.PostAsync(name);
                    context.Wait(MessageReceivedAsync);
                }
                   
            }

        }

    }

    [BotAuthentication]
    public class MessagesController : ApiController
    {

        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                

                return await Conversation.SendAsync(message, () => new PersonalityDialog());
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            return null;
        }
    }



}
    
    





    //[BotAuthentication]
    //public class MessagesController : ApiController
    //{
    //    public int state = 1;

    //    public async Task<Message> Post([FromBody]Message message)
    //    {

    //        if (message.Type == "Message"&&state==1)
    //        {
    //            state++;
    //            TestType names = new TestType();            
    //            var listOfDecks = string.Join(", ", names.GetNames().ToArray());
    //            return message.CreateReplyMessage("Hello! Please, choose a topic: " + listOfDecks + state);
    //        }
    //        else
    //        {
    //            return HandleSystemMessage(message);
    //        }
    //    }

    //    private Message HandleSystemMessage(Message message)
    //    {

    //        if (message.Type == "BotAddedToConversation")
    //        {
    //            Message reply = message.CreateReplyMessage();
    //            reply.Type = "Welcome to Traitify bot! I will help you to determine your type of personality. Type begin to start conversation";
    //            return reply;
    //        }
    //        return null;
    //    }
    //}
