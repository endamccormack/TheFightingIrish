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
using TheFightingIrish.Projectiles;


namespace TheFightingIrish
{
    public delegate void FullyLocked();

    public class HUD : DrawableGameComponent
    {
        public static event FullyLocked LaunchMissiles;

        Texture2D pointer, paintPointer;
        SpriteBatch spriteBatch;

        bool inTargetMode = false;
        Rectangle targetRect = new Rectangle();
        int maxTargets = 10;
        int currentTargets = 0;

        float coolDown = 10;
        float coolDownTimer = 10;

        public HUD(Game game, SpriteBatch spriteBatch)
            : base(game)
        {
            game.Components.Add(this);
            this.spriteBatch = spriteBatch;

            pointer = game.Content.Load<Texture2D>(@"Textures/HUD/Pointer");
            paintPointer = game.Content.Load<Texture2D>(@"Textures/HUD/PointerPainter");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            coolDownTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InputManager.IsKeyPressed(Keys.T) && coolDownTimer > coolDown && Player.mode == PlayerMode.Plane)
            {
                inTargetMode = true;
            }

            if (inTargetMode)
            {
                targetRect = new Rectangle(
                    (int)InputManager.GetRelativeMousePosToCamera().X - (paintPointer.Width / 2),
                    (int)InputManager.GetRelativeMousePosToCamera().Y - (paintPointer.Height / 2),
                    paintPointer.Width,
                    paintPointer.Height);

                for (int i = 0; i < EnemyManager.Enemies.Count; i++)
                {
                    if (targetRect.Contains(EnemyManager.Enemies[i].BoundingRect) && currentTargets < maxTargets)
                    {
                        if (!ProjectileManager.enemyIndexes.Contains(EnemyManager.Enemies[i].Index))
                        {
                            ProjectileManager.enemyIndexes.Add(EnemyManager.Enemies[i].Index);
                            EnemyManager.Enemies[i].IsTargeted = true;
                            currentTargets++;
                        }
                    }
                }

                if (currentTargets >= maxTargets)
                {
                    if (LaunchMissiles != null)
                        LaunchMissiles();

                    coolDownTimer = 0;
                    inTargetMode = false;
                    currentTargets = 0;
                }
            }

            targetRect = new Rectangle(
                (int)InputManager.GetRelativeMousePosToCamera().X - (paintPointer.Width / 2),
                (int)InputManager.GetRelativeMousePosToCamera().Y - (paintPointer.Height / 2),
                paintPointer.Width,
                paintPointer.Height);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!inTargetMode)
                spriteBatch.Draw(
                    pointer,
                    InputManager.GetRelativeMousePosToCamera(),
                    null,
                    Color.White,
                    0,
                    new Vector2(pointer.Width / 2, pointer.Height / 2),
                    1.0f,
                    SpriteEffects.None,
                    1);
            else
                spriteBatch.Draw(
                    paintPointer,
                    InputManager.GetRelativeMousePosToCamera(),
                    null,
                    Color.White,
                    0,
                    new Vector2(paintPointer.Width / 2, paintPointer.Height / 2),
                    1,
                    SpriteEffects.None,
                    1);

            //spriteBatch.Draw(paintPointer,
            //    targetRect,
            //    null,
            //    Color.Red,
            //    0,
            //    Vector2.Zero,
            //    SpriteEffects.None,
            //    1);

            base.Draw(gameTime);
        }
    }
}
