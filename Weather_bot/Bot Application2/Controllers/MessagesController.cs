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
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Net;
using System.Text;

namespace Bot_Application2
{
    class Weather
    {

        public static string WeatherInfo(string zip)
        {
                string weatherRequest = "http://api.wunderground.com/api/98dfafcf9efb4a27/conditions/q/" + zip + ".xml";
                XmlDocument weatherResponse = MakeRequest(weatherRequest);
                return ProcessResponse(weatherResponse);
        }

        public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return (xmlDoc);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                Console.Read();
                return null;
            }
        }
         public static string ProcessResponse(XmlDocument weatherResponse)
        {
            XmlNode temp = weatherResponse.SelectSingleNode("/response/current_observation");
            XmlNode city = weatherResponse.SelectSingleNode("/response/current_observation/display_location");

            string tempf = temp["temp_f"].InnerText;
            string cur_city = city["city"].InnerText;

            return "The temperature in " + cur_city + " is " + tempf + " F.";
           
        }
    }

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