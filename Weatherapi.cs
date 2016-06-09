using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;

namespace ConsoleApplication2
{


    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter zip code: ");
                string zip = Console.ReadLine();
                string weatherRequest = "http://api.wunderground.com/api/98dfafcf9efb4a27/conditions/q/" + zip + ".xml";
                XmlDocument weatherResponse = MakeRequest(weatherRequest);
                ProcessResponse(weatherResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.Read();
            }
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
        static public void ProcessResponse(XmlDocument weatherResponse)
        {
            XmlNode temp = weatherResponse.SelectSingleNode("/response/current_observation");
            XmlNode city = weatherResponse.SelectSingleNode("/response/current_observation/display_location");
            
                string tempf = temp["temp_f"].InnerText;
                string cur_city = city["city"].InnerText;

            Console.WriteLine("Temperature in " + cur_city + " is " + tempf + " F.");

        }
    }
}

