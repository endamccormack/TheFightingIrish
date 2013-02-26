using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheFightingIrish.Projectiles
{
    public class Cannon : Projectile
    {
        public Cannon(Texture2D texture, Vector2 position, int width, Color color, float rotation,
            float scale, float drawLayer, int frames, ProjectileType type, Vector2 direction, float speed, ProjectileOwner owner)
            : base(texture, position, width, color, rotation, scale, drawLayer, frames, type, direction, speed, owner)
        {
        }
    }
}
