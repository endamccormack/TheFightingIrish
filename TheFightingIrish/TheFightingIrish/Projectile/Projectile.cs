using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheFightingIrish.Projectiles
{
    public enum ProjectileType { Minigun, Cannon, Mines, Missile, Bomb }
    public enum ProjectileOwner { Player, Enemy }

    public class Projectile : Sprite
    {
        public ProjectileType Type { get; set; }
        public ProjectileOwner Owner { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public bool IsActive { get; set; }

        public Projectile(Texture2D texture, Vector2 position, int width, Color color, float rotation, 
            float scale, float drawLayer, int frames, ProjectileType type, Vector2 direction, float speed, ProjectileOwner owner)
            : base(texture, position, width, color, rotation, scale, drawLayer, frames)
        {
            this.Type = type;
            this.Owner = owner;
            this.Direction = direction;
            this.Speed = speed;

            this.IsActive = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsActive)
            {
                this.Position += this.Direction * this.Speed;
                this.Rotation = MoreMathHelpers.GetRotation(this.Direction);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public Rectangle BoundingRect
        {
            get { return new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height); }
        }
    }
}
