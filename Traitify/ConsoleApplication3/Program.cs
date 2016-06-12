
using com.traitify.net.TraitifyLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program
    {
        //public class SlideSelected
        //{
        //    public string id { get; set; }
        //    public bool response { get; set; }
        //    public int time_taken { get; set; }
        //}
        static void Main(string[] args)
        {
            Traitify traitify = new Traitify("https://api.traitify.com", "bt5bo444mk38efl4crbat39scv", "pl17doqlrachpa7jg5oqe3ja7p", "v1");

            List<Deck> decks = traitify.GetDecks();
            Assessment assess = traitify.CreateAssesment(decks[0].id);
            //Assessment assess = traitify.CreateAssesment("career-deck");
            string assessment_id = assess.id;
            Assessment assessment = traitify.GetAssessment(assessment_id);
            List<Slide> slides = traitify.GetSlides(assessment_id);
            //List<object>SlidesList = new List<object>();

            foreach (Slide slide in slides)
            {
                slide.time_taken = 600;
                slide.response = true;
                              
            }
            //SlidesList.Add(new SlideSelected() { id = slide.id, response = true, time_taken = 600 });
            traitify.SetSlideBulkUpdate(assessment_id, slides);
            AssessmentPersonalityTypes types = traitify.GetPersonalityTypes(assessment_id);
            List<AssessmentPersonalityType> personalityTypes = types.personality_types;
            Console.WriteLine("Type: " + personalityTypes[0].personality_type.name);
        }
    }
}
