using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TheFightingIrish.Projectiles;

namespace TheFightingIrish
{
    public enum PlayerMode { Tank, Plane }

    public class Player : Sprite
    {
        public static float PlayerSpeed { get; set; }

        public static PlayerMode mode;
        public static Sprite cannon;

        Texture2D plane, transition, tank;

        Vector2 cannonCentre;
        Vector2 oldPosition;
        Vector2 mouseToPoint;

        Vector2 cannonOffset = new Vector2(-4.2f, -7.5f);
        Vector2 minigunOffset = new Vector2(105f, 12f);
        Vector2 mineOffset = new Vector2(-110f, 32f);

        Vector2 missileOffset = new Vector2(0, 0);
        Vector2 bombOffse = new Vector2(0, 0);
        Vector2 minigunOffsetPlane = new Vector2(88f, 2f);

        float cannonRadius = 60;
        double maxDegrees = 150;
        double minDegrees = 30;

        bool changeMode = false;
        bool canFireCannon = true;

        int maxTankMoveHeight = 200;
        Rectangle TankMoveRect = new Rectangle(0, 0, 0, 0);
        Rectangle PlaneMoveRect = new Rectangle(0, 0, 0, 0);

        Vector2 tankDropPos, planeRisePos;
        float riseSpeed = 0.2f;
        float dropSpeed = 0.2f;

        float xMoveSpeed = 4;
        float yMoveSpeed = 4;

        public Player(Texture2D texture, Vector2 position, int width, Color color, float rotation, float scale, 
            float drawLayer, int frames, Texture2D cannonTex, Texture2D planeTex, Texture2D transitionTex)
            : base(texture, position, width, color, rotation, scale, drawLayer, frames)
        {
            cannon = new Sprite(cannonTex, this.Position + cannonOffset, cannonTex.Width, Color.White, 0, 1, this.DrawLayer - 0.01f, 1);

            tank = texture;
            plane = planeTex;
            transition = transitionTex;

            TankMoveRect = new Rectangle(
                (int)Position.X,
                Camera2D.ScreenRect.Height - maxTankMoveHeight, 
                Camera2D.ScreenRect.Width,
                maxTankMoveHeight - this.Height / 2);

            PlaneMoveRect = new Rectangle(
                Camera2D.ScreenRect.X,
                0,
                Camera2D.ScreenRect.Width,
                Camera2D.ScreenRect.Height - maxTankMoveHeight);

            tankDropPos = new Vector2(this.Position.X, TankMoveRect.Y + maxTankMoveHeight / 2);
            planeRisePos = new Vector2(this.Position.X, PlaneMoveRect.Height / 3);

            oldPosition = cannonCentre;
            mode = PlayerMode.Tank;
            PlayerSpeed = 0;

            ProjectileManager.CannonJustFired += new CannonFired(ProjectileManager_CannonJustFired);
            HUD.LaunchMissiles += new FullyLocked(HUD_LaunchMissiles);
        }

        public override void Update(GameTime gameTime)
        {
            #region Movement Code
            TankMoveRect = new Rectangle(
                0,
                Camera2D.ScreenRect.Height - maxTankMoveHeight, 
                Camera2D.ScreenRect.Width,
                maxTankMoveHeight - this.Height / 2);

            PlaneMoveRect = new Rectangle(
                0,
                0,
                Camera2D.ScreenRect.Width,
                Camera2D.ScreenRect.Height - maxTankMoveHeight);

            tankDropPos = new Vector2(this.Position.X, TankMoveRect.Y + maxTankMoveHeight / 2);
            planeRisePos = new Vector2(this.Position.X, PlaneMoveRect.Height / 3);

            #endregion

            #region Transformation Code
            tankDropPos = new Vector2(this.Position.X, TankMoveRect.Y + maxTankMoveHeight / 2);
            planeRisePos = new Vector2(this.Position.X, PlaneMoveRect.Height / 3);

            if (InputManager.IsKeyTapped(Keys.Space))
            {
                this.changeMode = true;
                this.Texture = transition;

                if (mode == PlayerMode.Tank) this.ResetAnimation(8);
                else this.ReverseAnimation(8);
            }

            if (this.changeMode && !this.animationReversed)
            {
                this.Position = Vector2.Lerp(this.Position, planeRisePos, riseSpeed);

                if (currentFrame == frames)
                {
                    mode = PlayerMode.Plane;
                    this.Texture = plane;
                    this.ResetAnimation(0);
                    this.changeMode = false;
                }
            }
            else if (this.changeMode && this.animationReversed)
            {
                this.Position = Vector2.Lerp(this.Position, tankDropPos, dropSpeed);

                if (currentFrame == 0)
                {
                    mode = PlayerMode.Tank;
                    this.Texture = tank;
                    this.ResetAnimation(0);
                    this.changeMode = false;
                }
            }
            #endregion

            #region Tank Code
            if (mode == PlayerMode.Tank)
            {
                Vector2 updatedPos = Vector2.Zero;

                if (InputManager.IsKeyPressed(Keys.W))
                    updatedPos.Y -= yMoveSpeed;
                if (InputManager.IsKeyPressed(Keys.S))
                    updatedPos.Y += yMoveSpeed;
                if (InputManager.IsKeyPressed(Keys.A))
                    updatedPos.X -= xMoveSpeed;
                if (InputManager.IsKeyPressed(Keys.D))
                    updatedPos.X += xMoveSpeed;

                if (TankMoveRect.Contains((int)(updatedPos.X + this.Position.X), (int)(updatedPos.Y + this.Position.Y)))
                    this.Position += updatedPos;

                if (InputManager.IsKeyPressed(Keys.C) && canFireCannon)
                {
                    ProjectileManager.AddCannonProjectile(
                        cannon.Position,
                        Vector2.Normalize(mouseToPoint),
                        ProjectileOwner.Player);
                }

                if (InputManager.IsKeyPressed(Keys.M))
                {
                    ProjectileManager.AddMinigunProjectile(
                        this.Position + minigunOffset, 
                        MoreMathHelpers.GetDirectionVector(
                            (this.Position + minigunOffset) - new Vector2(1, 0), 
                            this.Position + minigunOffset),
                            ProjectileOwner.Player);
                }

                if (InputManager.IsKeyPressed(Keys.N))
                {
                    ProjectileManager.AddMineProjectile(
                        this.Position + mineOffset,
                        Vector2.Zero,
                        ProjectileOwner.Player);
                }

                cannon.Update(gameTime);
            }
            #endregion

            #region Plane Code
            if (mode == PlayerMode.Plane)
            {
                Vector2 updatedPos = Vector2.Zero;

                if (InputManager.IsKeyPressed(Keys.W))
                    updatedPos.Y -= yMoveSpeed;
                if (InputManager.IsKeyPressed(Keys.S))
                    updatedPos.Y += yMoveSpeed;
                if (InputManager.IsKeyPressed(Keys.A))
                    updatedPos.X -= xMoveSpeed;
                if (InputManager.IsKeyPressed(Keys.D))
                    updatedPos.X += xMoveSpeed;

                if (PlaneMoveRect.Contains((int)(updatedPos.X + this.Position.X), (int)(updatedPos.Y + this.Position.Y)))
                    this.Position += updatedPos;
                
                if (InputManager.IsKeyPressed(Keys.M))
                {
                    ProjectileManager.AddMinigunProjectile(
                        this.Position + minigunOffsetPlane,
                        MoreMathHelpers.GetDirectionVector(
                            (this.Position + minigunOffsetPlane) - new Vector2(1, 0),
                            this.Position + minigunOffsetPlane),
                            ProjectileOwner.Player);
                }
            }
            #endregion


            UpdateCannon();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (mode == PlayerMode.Tank && !changeMode)
            {
                cannon.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        void UpdateCannon()
        {
            cannonCentre = this.Position + cannonOffset;

            Vector2 difference = cannonCentre - oldPosition ;

            mouseToPoint = cannonCentre - new Vector2(InputManager.GetRelativeMousePosToCamera().X, InputManager.GetRelativeMousePosToCamera().Y);
            mouseToPoint.Normalize();

            float tempRotation = (float)Math.Atan2((double)mouseToPoint.Y, (double)mouseToPoint.X);
            double degree = MoreMathHelpers.RadianToDegree((double)tempRotation);

            if (degree > minDegrees && degree < maxDegrees)
            {
                cannon.Rotation = (float)Math.Atan2((double)mouseToPoint.Y, (double)mouseToPoint.X);
                mouseToPoint *= cannonRadius;
                mouseToPoint *= -1;
                cannon.Position = cannonCentre + mouseToPoint;
                canFireCannon = true;
            }
            else
            {
                cannon.Position += difference;
                canFireCannon = false;
            }

            oldPosition = cannonCentre;
        }

        void ProjectileManager_CannonJustFired()
        {
            
        }

        void HUD_LaunchMissiles()
        {
            ProjectileManager.LaunchMissiles(this.Position + missileOffset, ProjectileOwner.Player);
        }
    }
}
