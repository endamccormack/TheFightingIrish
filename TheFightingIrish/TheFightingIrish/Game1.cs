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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font;

        Player player;
        Texture2D tank;
        Texture2D cannon;
        Texture2D plane;
        Texture2D transition;

        Camera2D camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
            this.graphics.ApplyChanges();

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera2D(
                new Vector2(this.GraphicsDevice.Viewport.Width / 2,
                    this.GraphicsDevice.Viewport.Height / 2),
                    this.GraphicsDevice.Viewport.Width,
                    this.GraphicsDevice.Viewport.Height);

            InputManager input = new InputManager(this);
            ProjectileManager pm = new ProjectileManager(this, spriteBatch);
            BackgroundManager bm = new BackgroundManager(this, spriteBatch);
            EnemyManager em = new EnemyManager(this, spriteBatch);
            HUD hud = new HUD(this, spriteBatch);

            tank = Content.Load<Texture2D>(@"Textures/Sprites/Tank");
            cannon = Content.Load<Texture2D>(@"Textures/Sprites/Cannon");
            plane = Content.Load<Texture2D>(@"Textures/Sprites/Plane");
            transition = Content.Load<Texture2D>(@"Textures/Sprites/TankToPlane");
            font = Content.Load<SpriteFont>(@"Fonts/Debug");

            player = new Player(
                tank,
                new Vector2(this.GraphicsDevice.Viewport.Width / 2,
                    600),
                tank.Width,
                Color.White,
                0,
                1,
                1.0f,
                0,
                cannon,
                plane,
                transition);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformation(graphics));
            player.Draw(spriteBatch);

            base.Draw(gameTime);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Camera Pos: " + Camera2D.position.ToString(), Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Rel Mouse Pos: " + InputManager.GetRelativeMousePosToCamera().ToString(), new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(font, "Mouse Pos: " + InputManager.GetMousePosition().ToString(), new Vector2(0, 40), Color.White);
            spriteBatch.DrawString(font, "Player Pos: " + player.Position, new Vector2(0, 60), Color.White);
            spriteBatch.DrawString(font, "Cannon Pos: " + Player.cannon.Position, new Vector2(0, 80), Color.White);
            spriteBatch.End();
        }
    }
}
