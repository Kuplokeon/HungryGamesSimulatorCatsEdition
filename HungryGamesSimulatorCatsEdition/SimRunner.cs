using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryGamesSimulatorCatsEdition
{
    
    public enum TimeOfDay
    {
        Morning,
        Day,
        Evening,
        Night,
    }

    internal class SimRunner
    {
        public static Random random = new Random ();

        public static int roundsPerDay;

        public static int currentTime;

        public static int daysPassed = 0;

        public static List<Cat> cats = new List<Cat>();

        static TimeOfDay[] timeOfDayOverTime =
        {
            TimeOfDay.Morning,
            TimeOfDay.Day,
            TimeOfDay.Day,
            TimeOfDay.Day,
            TimeOfDay.Evening,
            TimeOfDay.Night,
            TimeOfDay.Night,
            TimeOfDay.Night,
        };

        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                RunRound();
            }
        }

        public static void RunRound()
        {
            //advance to next day
            currentTime++;

            if (currentTime >= timeOfDayOverTime.Length)
            {
                currentTime = 0;
                daysPassed++;
            }

            RunActivities();
        }

        static int catDetectionRange = 10;

        public static void RunActivities()
        {
            List<int> unactedCatsReferences = new List<int> ();
            int counter = 0;
            
            foreach (Cat cat in cats)
            {
                unactedCatsReferences.Add(counter);
                counter++;
            }

            List<ActivityRequest> activitiesRequested = new List<ActivityRequest>();
            while (unactedCatsReferences.Count > 0)
            {
                List<int> involvedCats = new List<int> ();
                
                //take a cat
                involvedCats.Add(
                    unactedCatsReferences[
                        random.Next(
                            0, 
                            unactedCatsReferences.Count)]);

                //find a number of nearby cats
                involvedCats.AddRange(
                    FindNearbyCats( 
                        cats[involvedCats[0]]));

                //duplicate list of activities
                List<int> availableActivities = new List<int>();
                int activityCounter = 0;
                foreach (SimulationEvent simEvent in SimEventHandler.allPossibleEvents)
                {
                    availableActivities.Add(activityCounter);
                    activityCounter++;
                }

                //remove invalid activities
                for (int eventIndex = availableActivities.Count - 1; eventIndex >= 0; eventIndex--)
                {
                    if (!SimulationEventIsUsable(
                        SimEventHandler.allPossibleEvents[eventIndex],
                        ListOfCatReferencesToCats(involvedCats)))
                    {
                        //event is not usable, remove from list
                        availableActivities.RemoveAt(eventIndex);
                    }
                }

                //pick a random activity
                int simEventSelected = availableActivities[
                    random.Next(0, availableActivities.Count)];

                //request activity
                activitiesRequested.Add (
                    new ActivityRequest (
                        involvedCats, 
                        SimEventHandler.allPossibleEvents[
                            simEventSelected]));

                //remove cat(s) from list
                foreach (int cat in involvedCats)
                {
                    unactedCatsReferences.Remove(cat);
                }
            }

            //apply found requests
            foreach (ActivityRequest request in activitiesRequested)
            {
                ApplyActivity(request);
            }
        }

        public static void ApplyActivity(ActivityRequest request)
        {
            Debug.WriteLine(request.GetDialogue());
            //

            //remove any killed cats from list
        }

        public static bool SimulationEventIsUsable(SimulationEvent simulationEvent, List<Cat> involvedCats)
        {
            return true;
        }

        public static List<Cat> ListOfCatReferencesToCats(List<int> indexes)
        {
            List<Cat> cats = new List<Cat>();
            foreach (int i in indexes)
            {
                cats.Add(CatReferenceToCats(i));
            }
            return cats;
        }

        public static Cat CatReferenceToCats(int index)
        {
            return cats[index];
        }

        public static List<int> FindNearbyCats(Cat host)
        {
            return FindNearbyCats(host.position, host);
        }

        public static List<int> FindNearbyCats(Point position, Cat catToExclude)
        {
            List<int> nearbyCats = new List<int> ();

            int counter = 0;
            foreach (Cat cat in cats)
            {
                if (cat != catToExclude)
                {
                    if (Vector2.Distance(
                        cat.position.ToVector2(), 
                        position.ToVector2())
                        < catDetectionRange)
                    {
                        nearbyCats.Add(counter);
                    }
                }
                counter++;
            }

            return nearbyCats;
        }

        public static TimeOfDay GetCurrentTimeOfDay()
        {
            return timeOfDayOverTime[currentTime];
        }

        public static bool IsDaytime()
        {
            TimeOfDay currentTimeOfDay = GetCurrentTimeOfDay();

            return currentTimeOfDay == TimeOfDay.Morning || currentTimeOfDay == TimeOfDay.Day;
        }
    }

    /// <summary>
    /// Places an order for an activity
    /// </summary>
    public class ActivityRequest
    {
        public List<int> catsList;
        public SimulationEvent simulationEvent;

        public ActivityRequest(List<int> catsList, SimulationEvent simulationEvent)
        {
            this.catsList = catsList;
            this.simulationEvent = simulationEvent;
        }

        public string GetDialogue()
        {
            string dialogueOutput = simulationEvent.dialogue;

            if (dialogueOutput.Contains("Catlist"))
            {
                dialogueOutput.Replace("Catlist", GetCatList());
            }

            int counter = 0;
            foreach (int i in catsList)
            {
                dialogueOutput = dialogueOutput.Replace("Cat" + counter, SimRunner.cats[i].name);
                counter++;
            }

            return dialogueOutput;
        }

        public string GetCatList()
        {
            string output = "";

            int counter = 0;
            foreach (int i in catsList)
            {
                if (counter == 0)
                {
                    output += SimRunner.cats[i].name;
                } else
                if (counter == catsList.Count - 1)
                {
                    output += ", and " + SimRunner.cats[i].name;
                } else
                {
                    output += ", " + SimRunner.cats[i].name;
                }

                counter++;
            }

            return output;
        }
    }

}
