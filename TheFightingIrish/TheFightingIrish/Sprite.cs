using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheFightingIrish
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public float DrawLayer { get; set; }
        public float Scale { get; set; }
        public bool IsEnabled { get; set; }

        public Rectangle TextureRect { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        protected int frames = 0;
        protected int currentFrame = 0;
        float timer = 0;
        float frameTimer = 0.02f;

        protected bool animate = true;
        protected bool animationReversed = false;

        public Sprite(Texture2D texture, Vector2 position, int width, Color color, float rotation, float scale, float drawLayer, int frames)
        {
            this.Texture = texture;
            this.Position = position;
            this.Width = Width;
            this.Color = color;
            this.Rotation = rotation;
            this.Scale = scale;
            this.DrawLayer = drawLayer;
            this.Width = width;
            this.frames = frames;

            this.Height = texture.Height;
            this.IsEnabled = true;

            this.Origin = new Vector2(this.Width / 2, this.Height / 2);
            this.TextureRect = new Rectangle(currentFrame * Width, 0, Width, Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                if (animate)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timer > frameTimer)
                    {
                        if (!animationReversed)
                        {
                            if (currentFrame < frames)
                            {
                                currentFrame++;
                                this.TextureRect = new Rectangle(currentFrame * Width, 0, Width, Height);
                            }
                            else currentFrame = 0;
                        }
                        else
                        {
                            if (currentFrame > 0)
                            {
                                currentFrame--;
                                this.TextureRect = new Rectangle(currentFrame * Width, 0, Width, Height);
                            }
                            else currentFrame = frames;
                        }

                        timer = 0;
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsEnabled)
            {
                spriteBatch.Draw(
                    this.Texture,
                    this.Position,
                    this.TextureRect,
                    this.Color,
                    this.Rotation,
                    this.Origin,
                    this.Scale,
                    SpriteEffects.None,
                    this.DrawLayer);
            }
        }

        public void ResetAnimation(int frames)
        {
            this.frames = frames;
            this.currentFrame = 0;
            this.animationReversed = false;
            this.TextureRect = new Rectangle(currentFrame * Width, 0, Width, Height);
        }

        public void ReverseAnimation(int frames)
        {
            animationReversed = true;
            this.frames = frames;
            currentFrame = frames;
            this.TextureRect = new Rectangle(currentFrame * Width, 0, Width, Height);
        }

        public Rectangle BoundingRect
        {
            get
            {
                return new Rectangle((int)this.Position.X - (this.Width / 2),
                (int)this.Position.Y - (this.Height / 2),
                this.Width,
                this.Height);
            }
        }
    }
}
