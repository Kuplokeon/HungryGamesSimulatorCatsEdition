using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HungryGamesSimulatorCatsEdition
{
    internal class CameraScript
    {
        public static Point tileSize = new Point (320);
        public static Point screenSize;

        public static Point position;
        public static float zoom = 1;


        public static void Run()
        {
            float minZoom = 0.5f;
            float maxZoom = 20f;
            float zoomSpeed = 0.1f;
            int cameraSpeed = (int)(15 * (zoom * 4));

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                position.Y += cameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position.Y -= cameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= cameraSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += cameraSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M) && zoom > minZoom)
            {
                zoom -= zoomSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.N) && zoom < maxZoom)
            {
                zoom += zoomSpeed;
            }
        }

        /// <returns>screenPosition</returns>
        public static Point WorldPositionToScreenPosition(Point worldPosition)
        {
            Point screenPos = worldPosition;

            screenPos.X *= tileSize.X / 2;
            screenPos.Y *= tileSize.Y / 2;

            screenPos -= new Point (screenSize.X / 2, screenSize.Y / 2);
            
            screenPos.X -= position.X;
            screenPos.Y += position.Y;
            
            screenPos.X = (int)(screenPos.X / (zoom)) + (screenSize.X / 2);
            screenPos.Y = (int)(screenPos.Y / (zoom)) + (screenSize.Y / 2);


            return screenPos;
        }

        /// <returns>screenSize</returns>
        public static float WorldSizeToScreenSize(float worldSize)
        {
            return worldSize / zoom;
        }

        public static Rectangle WorldRectToScreenRect(Rectangle worldRect)
        {
            Rectangle screenRect =
                new Rectangle(
                    WorldPositionToScreenPosition(worldRect.Location),
                    new Point(
                        (int)WorldSizeToScreenSize(worldRect.Width),
                        (int)WorldSizeToScreenSize(worldRect.Height)
                        )
                    );

            return screenRect;
        }
    }
}
