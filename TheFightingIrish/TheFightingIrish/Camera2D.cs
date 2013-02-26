using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheFightingIrish
{
    class Camera2D
    {
        static public Matrix transform;
        static public Vector2 position;
        static public Rectangle ScreenRect = new Rectangle(0, 0, 0, 0);
        static int screenWidth, screenHeight;
        float zoom;

        public Camera2D(Vector2 pos, int screenW, int screenH)
        {
            position = pos;
            zoom = 1.0f;
            screenWidth = screenW;
            screenHeight = screenH;
            ScreenRect = CalculateScreenRect();
        }

        public Matrix GetTransformation(GraphicsDeviceManager graphics)
        {
            transform = 
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * 
                                            Matrix.CreateScale(zoom) * 
                                            Matrix.CreateTranslation(new Vector3(graphics.PreferredBackBufferWidth * 0.5f, 
                                                        graphics.PreferredBackBufferHeight * 0.5f, 0));

            ScreenRect = CalculateScreenRect();

            return transform;
        }

        Rectangle CalculateScreenRect()
        {
            int x = (int)position.X - (screenWidth / 2);
            int y = (int)position.Y - (screenHeight / 2);

            return new Rectangle(x, y, screenWidth, screenHeight);
        }

        public void LerpCamera(Vector2 newPosition, float speed)
        {
            position = Vector2.Lerp(position, newPosition, speed);
        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = MathHelper.Clamp(value, 0f, 2f); }
        }
    }
}
