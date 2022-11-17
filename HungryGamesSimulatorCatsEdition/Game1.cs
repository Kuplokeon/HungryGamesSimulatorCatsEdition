using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HungryGamesSimulatorCatsEdition
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            //_graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.PreferMultiSampling = true;
            _graphics.ApplyChanges();


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        Texture2D grassBackground;

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            CameraScript.screenSize =
                new Point(
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            
            Console.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);

            for (int i = 0; i < 100; i++)
            {
                Cat newCat = new Cat();
                //newCat.position.Y = (i / 10) + (grassBackground.Width / 2);
                newCat.position.Y = (i / 10);
                //newCat.position.X = (i % 10) + (grassBackground.Height / 2);
                newCat.position.X = (i % 20);
                SimRunner.cats.Add(newCat);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            grassBackground = Content.Load<Texture2D>("grass");
            GlobalCatVisuals.debugCat = Content.Load<Texture2D>("cat");

            UiManager.fontFace = Content.Load<SpriteFont>("fontface");

            foreach (string path in GlobalCatVisuals.textures)
            {
                GlobalCatVisuals.contentKeys.Add(path);
                GlobalCatVisuals.contents.Add(path, Content.Load<Texture2D>("cat-visuals/"+path));
            }

            SimEventHandler.LoadAllPossibleEvents();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            CameraScript.Run();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                foreach (Cat cat in SimRunner.cats)
                {
                    cat.TestMove();
                } 
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            foreach (Cat cat in SimRunner.cats)
            {
                cat.Update();
            }

            SimRunner.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(GlobalCatVisuals.debugCat, CameraScript.screenSize.ToVector2() / 2, Color.White);

            //draw background
            _spriteBatch.Draw(
                grassBackground,
                CameraScript.WorldRectToScreenRect(
                    new Rectangle (
                        Point.Zero, 
                        new Point(
                            (int)((grassBackground.Width * CameraScript.tileSize.X * 0.5f)))
                        )),
                Color.White);

            //draw cats
            foreach (Cat cat in SimRunner.cats)
            {
                if (CameraScript.DistanceToCenterOfScreen(cat.position) < MathF.Max(CameraScript.screenSize.X, CameraScript.screenSize.Y) / 2f)
                {
                    DrawCatToScreen.DrawCat(_spriteBatch, cat);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}