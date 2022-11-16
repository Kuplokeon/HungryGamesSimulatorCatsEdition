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

    internal class SimEventHandler
    {

    }

    public class SimulationEvent
    {
        public string dialogue;
        public int requiredPlayers;
        public List<WorldBiome> requiredBiomes;
        public int commonness;
        
        public List<PersonalityTrait> requiredPersonalityTraits;
        
        public List<RelationshipLevelRequirement> requiredRelationshipLevels;
        

        public List<string> requiredMemories;
        public List<string> memoriesToGive;
        public List<string> memoriesToRemove;
    }

    public class RelationshipLevelRequirement
    {
        public RelationshipLevel level;
        public NumComparison comparison;
        public string player1ID = "Cat1";
        public string player2ID = "Cat2";
    }
}
