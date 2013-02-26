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
    public class BackgroundManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Texture2D background1;

        List<Vector2> positions = new List<Vector2>();

        public static float speed = -15.0f;

        public BackgroundManager(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            game.Components.Add(this);
            this.spriteBatch = spriteBatch;

            background1 = game.Content.Load<Texture2D>(@"Textures/Backgrounds/BackgroundImage_01");

            for (int i = 0; i < 300; i++)
            {
                positions.Add(new Vector2(-500 + (i * background1.Width), 0));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] += new Vector2(speed, 0);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                spriteBatch.Draw(background1, positions[i], Color.White);
            }

            base.Draw(gameTime);
        }
    }
}
