using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryGamesSimulatorCatsEdition
{
    public enum WorldBiome
    {
        Field,
        Basalt,
        Forest,
        Desert,
        Jungle,
        Mushroom,
        Siltlands,
        Basketbiome,
        Spam,
        Wyoming,
        Ohio,
        Raveyard,
        Tundra,
        Titos,
        CrystalForest,
        SpaceSkyFields,
    }

    public enum PersonalityTrait
    {
        //
    }

    public enum NumComparison
    {
        LessThan,
        EqualTo,
        GreaterThan,
    }

    public enum RelationshipLevel
    {
        Nemesis = -4,
        ArchEnemy = -3,
        Enemy = -2,
        Disliked = -1,
        Stranger = 0,
        Familiar = 1,
        Friend = 2,
        CloseFriend = 3,
        Crush = 4,
        Dating = 5,
        Married = 6,
    }

    public static class SimEventHandler
    {
        public static List<SimulationEvent> allPossibleEvents;

        public void LoadAllPossibleEvents()
        {
            //
        }
    }

    public class SimulationEvent
    {
        public string dialogue;
        public IntegerRequirement requiredPlayers;
        public List<WorldBiome> requiredBiomes;
        public int commonness;
        public List<PersonalityTrait> requiredPersonalityTraits;
        
        public List<RelationshipLevelRequirement> requiredRelationshipLevels;
        public List<RelationshipLevelAlteration> relationshipLevelAlterations;
        

        public List<string> requiredMemories;
        public List<string> memoriesToGive;
        public List<string> memoriesToRemove;

        public List<Ailment> requiredConditions;
        public List<Ailment> conditionsToGive;
        public List<Ailment> conditionsToRemove;

        public List<ItemHeld> requiredItems;
        public List<ItemHeld> itemsToGive;
        public List<ItemHeld> itemsToRemove;

        public SimulationEvent (string inputString)
        {
            string[] splitString = inputString.Split('\t');
            
            dialogue = splitString[1].Trim();
            requiredPlayers = new IntegerRequirement(splitString[2]);
            requiredBiomes = new List<WorldBiome>();
            foreach (string str in splitString[3].Split(','))
            {
                WorldBiome biome;
                if (Enum.TryParse(str.Trim(), out biome))
                {
                    WorldBiome value = (WorldBiome)biome;

                    // `value` is what you're looking for

                }
                else { /* error: the string was not an enum member */ }
                requiredBiomes.Add(biome);
            }
            commonness = int.Parse(splitString[4]);
            //requiredPersonalityTraits
        }
    }

    /// <summary>
    /// to be inherited by other classes
    /// </summary>
    public class Ailment
    {
        public string name;
    }

    public class ItemHeld
    {
        public string name;
        public int amount;
    }

    public class RelationshipLevelRequirement
    {
        public IntegerRequirement level;
        public NumComparison comparison;
        public string player1ID = "Cat1";
        public string player2ID = "Cat2";
    }

    public class RelationshipLevelAlteration
    {
        public RelationshipLevel level;
        public string player1ID = "Cat1";
        public string player2ID = "Cat2";
    }

    public class IntegerRequirement
    {
        public int number;
        public NumComparison comparison = NumComparison.EqualTo;

        public IntegerRequirement(string inputString)
        {
            string stringToParse = inputString;

            switch (inputString[0])
            {
                case '<':
                    comparison = NumComparison.LessThan;
                    stringToParse.Remove(0, 1);
                    break;
                case '>':
                    comparison = NumComparison.GreaterThan;
                    stringToParse.Remove(0,1);
                    break;
            }

            number = int.Parse(stringToParse);
        }

        public bool NumberIsValid(int num)
        {
            switch (comparison)
            {
                case NumComparison.LessThan: return num < number;
                case NumComparison.GreaterThan: return num > number;
                case NumComparison.EqualTo: return num == number;
            }
            return false;
        }
    }
}
