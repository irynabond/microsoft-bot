using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Text;

namespace WeatherAPI
{
    class Weather
    {

        public static string WeatherInfo(int zip)
        {
            try
            {
                string weatherRequest = "http://api.wunderground.com/api/98dfafcf9efb4a27/conditions/q/" + zip + ".xml";
                XmlDocument weatherResponse = MakeRequest(weatherRequest);
                return ProcessResponse(weatherResponse);
            }
            catch (Exception e)
            {
                return "Zip code is incorrect. Please, try another zip code";
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
        public static string ProcessResponse(XmlDocument weatherResponse)
        {
            XmlNode temp = weatherResponse.SelectSingleNode("/response/current_observation");
            XmlNode city = weatherResponse.SelectSingleNode("/response/current_observation/display_location");

            string tempf = temp["temp_f"].InnerText;
            string cur_city = city["city"].InnerText;

            return "The temperature in " + cur_city + " is " + tempf + " F.";

        }
    }
}