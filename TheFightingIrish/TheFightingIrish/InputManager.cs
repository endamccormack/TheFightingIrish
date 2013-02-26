using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TheFightingIrish
{
    public class InputManager : GameComponent
    {
        static MouseState mouse, oldMouse;
        static KeyboardState keys, oldKeys;

        public InputManager(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            oldKeys = keys;
            oldMouse = mouse;

            mouse = Mouse.GetState();
            keys = Keyboard.GetState();

            base.Update(gameTime);
        }

        public static Vector2 GetMousePosition()
        {
            return new Vector2(mouse.X, mouse.Y);
        }

        public static Vector2 GetRelativeMousePosToCamera()
        {
            Vector2 pos = Vector2.Transform(GetMousePosition(), Matrix.Invert(Camera2D.transform));
            return pos;
        }

        public static bool IsKeyPressed(Keys key)
        {
            if (keys.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public static bool IsKeyTapped(Keys key)
        {
            if (oldKeys.IsKeyUp(key) && keys.IsKeyDown(key))
                return true;
            else
                return false;
        }
    }
}
