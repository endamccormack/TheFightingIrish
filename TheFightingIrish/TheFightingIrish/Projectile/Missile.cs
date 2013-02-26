using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheFightingIrish.Projectiles
{
    public class Missile : Projectile
    {
        public int MissileTargetIndex { get; set; }
        float rotationRate = 0.005f;
        Vector2 targetDirection = Vector2.Zero;

        public Missile(Texture2D texture, Vector2 position, int width, Color color, float rotation, float scale, float drawLayer, 
            int frames, int targetEnemyIndex, ProjectileType type, Vector2 direction, float speed, ProjectileOwner owner)
            : base(texture, position, width, color, rotation, scale, drawLayer, frames, type, direction, speed, owner)
        {
            this.MissileTargetIndex = targetEnemyIndex;
        }

        public override void Update(GameTime gameTime)
        {
            int count = 0;

            Vector2 missileDirection = Vector2.Zero;
            for (int i = 0; i < EnemyManager.Enemies.Count; i++)
            {
                if (EnemyManager.Enemies[i].Index == this.MissileTargetIndex)
                {
                    targetDirection = EnemyManager.Enemies[i].Position - this.Position;
                    targetDirection.Normalize();

                    missileDirection = Vector2.Lerp(missileDirection, targetDirection, rotationRate);
                    count++;
                }
            }

            this.Direction = missileDirection;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
