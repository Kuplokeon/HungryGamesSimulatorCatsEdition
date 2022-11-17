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

        static bool justHitSpace;
        public static void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !justHitSpace)
            {
                RunRound();
                justHitSpace = true;
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                justHitSpace = false;
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

            foreach (Cat cat in cats)
            {
                cat.TestMove();
            }
        }

        public static void ApplyActivity(ActivityRequest request)
        {
            string dialogue = request.GetDialogue();

            Debug.WriteLine(dialogue);

            foreach (int i in request.catsList)
            {
                cats[i].lastDialogue = dialogue;

                Debug.WriteLine(cats[i].name + " --> " + dialogue);
            }
            
            //remove any killed cats from list
        }

        public static bool SimulationEventIsUsable(SimulationEvent simulationEvent, List<Cat> involvedCats)
        {
            if (!simulationEvent.requiredPlayers.NumberIsValid(involvedCats.Count))
            {
                return false;
            }

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

        public static Cat GetCatInPostion(Point pos)
        {
            foreach (Cat cat in cats)
            {
                if (cat.position == pos)
                {
                    return cat;
                }
            }
            return null;
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
                        if (nearbyCats.Count >= 5)
                        {
                            return nearbyCats;
                        }
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

        public static Cat GetCatClosestToCenter()
        {
            if (cats.Count == 0)
            {
                return null;
            }

            Cat closestToCenter = cats[0];
            float closestToCenterDist = 999999;
            foreach (Cat cat in cats)
            {
                float newDistance = CameraScript.DistanceToCenterOfScreen(cat.position);

                if (newDistance < closestToCenterDist)
                {
                    closestToCenterDist = newDistance;
                    closestToCenter = cat;
                }
            }

            return closestToCenter;
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
            string dialogueOutput = simulationEvent.dialogue.Trim();

            if (dialogueOutput.Contains("Catlist"))
            {
                dialogueOutput = dialogueOutput.Replace("Catlist", GetCatList());
            }

            int counter = 0;
            foreach (int i in catsList)
            {
                dialogueOutput = dialogueOutput.Replace("Cat" + (counter + 1), SimRunner.cats[i].name);
                Debug.WriteLine(SimRunner.cats[i].name);
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
