using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryGamesSimulatorCatsEdition
{
    public class Cat
    {
        public static Random random = new Random();

        public string name;

        public CatVisuals visuals = new CatVisuals ();

        public Point position;

        public List<Relationship> relationships = new List<Relationship> ();

        public Cat()
        {
            Awake();
        }

        public void TestMove()
        {
            position.X += random.Next(-1, 2);
            position.Y += random.Next(-1, 2);
        }

        public void Awake()
        {
            name = CatNameGenerator.GetRandomName();

            position = new Point(
                random.Next(1, CameraScript.screenSize.X), 
                random.Next(1, CameraScript.screenSize.Y));

            visuals.furColor = new Color(
                random.Next(50, 100) / 100f,
                random.Next(50, 100) / 100f,
                random.Next(50, 100) / 100f
                );

            visuals.AutoLoad();
        }

        public void SetRelationshipStatus(Cat other, RelationshipLevel level)
        {
            foreach (Relationship relationship in relationships)
            {
                if (relationship.other == other)
                {
                    relationship.level = level;
                    return;
                }
            }

            //no relationship found, making a new one.
            relationships.Add(new Relationship(other, level));
        }

        public RelationshipLevel GetRelationshipStatus(Cat other)
        {
            foreach (Relationship relationship in relationships)
            {
                if (relationship.other == other)
                {
                    return relationship.level;
                }
            }

            return RelationshipLevel.Stranger;
        }
    }

    public class Relationship
    {
        public Cat other;
        public RelationshipLevel level;

        public Relationship (Cat other, RelationshipLevel level)
        {
            this.other = other;
            this.level = level;
        }
    }

    public class CatVisuals
    {
        public Color furColor = Color.White;
        public Color leftEye = Color.CornflowerBlue;
        public Color rightEye = Color.CornflowerBlue;

        public List<CatDecoration> decorations = new List<CatDecoration> ();

        public void AutoLoad()
        {
            for (int i = 0; i < Cat.random.Next(1, 6); i++)
            {
                decorations.Add(new CatDecoration(
                    GlobalCatVisuals.contents[
                        GlobalCatVisuals.contentKeys[
                            Cat.random.Next(0, GlobalCatVisuals.contents.Count)
                            ]],
                            new Color(
                                Cat.random.Next(50, 100) / 100f,
                                Cat.random.Next(50, 100) / 100f,
                                Cat.random.Next(50, 100) / 100f)));
            }
        }
    }

    public class CatDecoration
    {
        public Texture2D texture;
        public Color color;

        public CatDecoration(Texture2D texture, Color color)
        {
            this.texture = texture;
            this.color = color;
        }
    }

    public static class DrawCatToScreen
    {
        public static void DrawCat(SpriteBatch spriteBatch, Cat catToDraw)
        {
            Rectangle drawRect = new Rectangle(catToDraw.position, CameraScript.tileSize);

            drawRect = CameraScript.WorldRectToScreenRect(drawRect);

            spriteBatch.Draw(
                GlobalCatVisuals.debugCat,
                drawRect,
                catToDraw.visuals.furColor);

            if (CameraScript.zoom < 2)
            {
                foreach (CatDecoration decoration in catToDraw.visuals.decorations)
                {
                    spriteBatch.Draw(
                        decoration.texture,
                        drawRect,
                        decoration.color);
                }
            }

            if (CameraScript.zoom < 1)
            {
                spriteBatch.DrawString(
                    UiManager.fontFace, 
                    catToDraw.name, 
                    drawRect.Center.ToVector2(), 
                    Color.DarkRed,
                    0,
                    Vector2.Zero,
                    3,
                    SpriteEffects.None,
                    0);
            }
        }
    }

    public static class CatNameGenerator
    {
        static string[] firstNames = new string[]
        {
            "star",
            "wave",
            "rain",
            "water",
            "fire",
            "sun",
            "moon",
            "buster",
            "train",
            "rain",
            "yari",
            "yumi",
            "tate",
            "kiba",
            "deka",
            "mega",
            "maho",
            "robo",
            "tori",
            "canno",
            "chari",
            "grenn",
            "mini",
            "centi",
            "yippee",
            "rose",
            "pik",
            "red",
            "orange",
            "yellow",
            "green",
            "blue",
            "purple",
            "pink",
            "grey",
            "gray",
            "experiment",
            "ESP",
            "george",
            "tender",
            "loving",
            "hugging",
            "heart",
            "cherry",
            "maggot",
            "flea",
            "meat",
            "wood",
            "soft",
            "water",
            "magma",
            "lava",
            "rock",
            "shotgun",
            "shrink",
            "grow",
            "machine",
            "self",
            "double ",
            "baby ",
            "monster",
            "mono",
            "poly",
            "super",
            "dream",
            "tally",
            "bi",
            "pan",
            "refriger",
            "psycho",
            "hit",
#region bad language words
            "shit",
            "bitch",
            "bastard",
            "ass",
            "pussy",
            "wet ass",
#endregion
            "sexy",
            "moist",
            "morb",
            "prison",
            "back",
            "king",
            "queen",
            "lizard",
            "jungle",
            "airport",
            "stuff",
            "robert downey ",
            "peanut",
            "scunge",
            "the ",
            "tater",
            "Mr. ",
            "Ms. ",
            "Mrs. ",
            "Dr. ",
            "final ",
            "",
        };

        static string[] lastNames = new string[]
        {
            "storm",
            "paw",
            "leg",
            "butter",
            "sauce",
            "juice",
            " jr.",
            "tail",
            "port",
            " johnson",
            "a",
            "star",
            "hall",
            "gizzard",
            "wizard",
            "cat",
            "kit",
            "graph",
            "cluster",
            "pon",
            "ton",
            "men",
            "money",
            "tan",
            "jail",
            "deth",
            "quartz",
            "bean",
            "killer",
            "man",
            "woman",
            "min",
            "dog",
            " eater",
            "heart",
            "soul",
            "mind",
            "room",
            "rooms",
            "berry",
            "blood",
            "eye",
            "brain",
            "loaf",
            "3000",
            "melt",
            "rot",
            "stone",
            "ius",
            " store",
            "athon",
            "path",
            "ator",
            " warrior",
            "inator",
            "sexual",
            "tot",
            "toe",
            "",
        };

        public static string GetRandomName()
        {
            string firstHalf = firstNames[Cat.random.Next(0, firstNames.Length)];
            string lastHalf = lastNames[Cat.random.Next(0, lastNames.Length)];

            return firstHalf + lastHalf;
        }
    }

    public static class GlobalCatVisuals
    {
        public static Texture2D debugCat;

        public static List<string> contentKeys = new List<string>();
        public static Dictionary<string, Texture2D> contents = new Dictionary<string, Texture2D>();

        public static string[] textures = new string[]
        {
            "back legs",
            "eyes",
            "front legs",
            //"head",
            "tail",
            "torso",

            "decorations/chad-chin",
            "decorations/chest-tuff",
            "decorations/cloak",
            "decorations/collar",
            "decorations/crying-eyes",
            "decorations/ear-fluff",
            "decorations/flowers",
            "decorations/pants",
            "decorations/wizard-hat",
            "decorations/yippee",
            "decorations/amogus",
            "decorations/refrigerator hat",
            "decorations/tummy fur",
            "decorations/straw hat",
            "decorations/sunglasses",
            "decorations/spamton glasses",
            "decorations/glasses",
            "decorations/spamton nose",
            "decorations/megamind cap",
            "decorations/spider legs",
            "decorations/contraption",
            "decorations/skateboard",
            "decorations/smile",
            "decorations/eyebrow",
            "decorations/slight tear",
            "decorations/blush",
            "decorations/unicorn horn",
            "decorations/goat horns",
            "decorations/big eyes",
            "decorations/sword in back",
            "decorations/eyebrows",
        };
    }
}
