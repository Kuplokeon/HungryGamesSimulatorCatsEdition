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

        public Cat(string name)
        {
            this.name = name;
            Awake();
        }

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
    }

    public class CatVisuals
    {
        public Color furColor = Color.White;
        public Color leftEye = Color.CornflowerBlue;
        public Color rightEye = Color.CornflowerBlue;

        public List<Texture2D> decorations = new List<Texture2D> ();

        public void AutoLoad()
        {
            for (int i = 0; i < 3; i++)
            {
                decorations.Add(
                    GlobalCatVisuals.contents[
                        GlobalCatVisuals.contentKeys[
                            Cat.random.Next(0, GlobalCatVisuals.contents.Count)
                            ]]);
            }
        }
    }

    public static class DrawCatToScreen
    {
        public static void DrawCat(SpriteBatch spriteBatch, Cat catToDraw)
        {
            Rectangle drawRect = new Rectangle(catToDraw.position, CameraScript.tileSize);

            spriteBatch.Draw(
                GlobalCatVisuals.debugCat,
                CameraScript.WorldRectToScreenRect(drawRect),
                catToDraw.visuals.furColor);

            if (CameraScript.zoom < 2)
            {
                foreach (Texture2D decoration in catToDraw.visuals.decorations)
                {
                    spriteBatch.Draw(
                        decoration,
                        CameraScript.WorldRectToScreenRect(drawRect),
                        Color.White);
                }
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
        };

        static string[] lastNames = new string[]
        {
            "storm",
            "paw",
            "leg",
            "tail",
            "star",
            "cat",
            "kit",
            "graph",
            "cluster",
            "pon",
            "ton",
            "men",
            "tan",
            "deth",
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
            "head",
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
        };
    }
}
