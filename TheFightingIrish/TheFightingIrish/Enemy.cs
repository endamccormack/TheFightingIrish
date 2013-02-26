using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheFightingIrish
{
    public class Enemy : Sprite
    {
        public float Speed { get; set; }
        public bool IsTargeted { get; set; }
        public int Index { get; set; }
        public Texture2D TargetTexture { get; set; }

        SpriteFont font;

        public Enemy(Texture2D texture, Vector2 position, int width, Color color, 
            float rotation, float scale, float drawLayer, int frames, float speed, int index, Texture2D targetTex, SpriteFont font)
            : base(texture, position, width, color, rotation, scale, drawLayer, frames)
        {
            this.Speed = speed;
            this.Index = index;
            this.TargetTexture = targetTex;

            this.font = font;

            this.IsTargeted = false;
        }

        public override void Update(GameTime gameTime)
        {
            this.Position += new Vector2(Speed, 0);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (this.IsTargeted)
                spriteBatch.Draw(TargetTexture,
                    this.Position,
                    null,
                    Color.White,
                    this.Rotation,
                    this.Origin,
                    this.Scale,
                    SpriteEffects.None,
                    this.DrawLayer + 0.01f);

            //spriteBatch.DrawString(
            //    font,
            //    this.IsTargeted.ToString(),
            //    this.Position,
            //    Color.Black,
            //    0,
            //    Vector2.Zero,
            //    1,
            //    SpriteEffects.None,
            //    this.DrawLayer + 0.02f);
        }
    }
}
