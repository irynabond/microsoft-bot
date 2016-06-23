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
        string test_name;
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
                    test_name = message.Text;
                    slideCollection = slides.GetSlides(test_name);
                    await context.PostAsync("Type something to start");
                    context.Wait(MessageReceivedAsync);
                
                } else
                {
                    if (index==0)
                    {
                        string name = slideCollection[index].caption;
                        index++;
                        //await context.PostAsync(name);
                        await context.PostAsync("![" + name + "](" + slideCollection[index-1].image_desktop + ")");
                        context.Wait(MessageReceivedAsync);
                    } else
                    {
                        if (index < slideCollection.Count)
                        {
                            var message = await argument;
                            string name = slideCollection[index].caption;                        
                            slideCollection[index - 1].time_taken = 600;
                            slideCollection[index - 1].response = true;
                            index++;
                            //await context.PostAsync(name);
                            await context.PostAsync(index + "![" + name + "](" + slideCollection[index-1].image_desktop + ")");
                            context.Wait(MessageReceivedAsync);
                        } else if (index==slideCollection.Count)
                        {
                            var message = await argument;
                            slideCollection[index - 1].time_taken = 600;
                            slideCollection[index - 1].response = true;

                            TestType result = new TestType();

                            string personality_type = result.Result("test", slideCollection);
                            await context.PostAsync(personality_type);
                            context.Wait(MessageReceivedAsync);
                        }
                    }                                            
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
            if (message.Type == "BotAddedToConversation")
            {
                Message reply = message.CreateReplyMessage("Hello and welcome to dialog! Type something to start.");
                return reply;

            }

            if (message.Type == "BotRemovedFromConversation")
            {
                Message reply = message.CreateReplyMessage("Thank you! Hope to see you later.");
                return reply;
            }
            return null;
        }
    }
}
    
    
