﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RobotParty
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bg;
        public int level;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "content";
            level = 0;
        }
        IinputManager inputmanager;
        IDrawManager drawmanager;
        Ielementvisitor updatevisitor;
        Ielementvisitor drawvisitor;
        ScreenManager screenmanager;
        IonCollision collisioncalculator;

        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
            screenmanager = new ScreenManager();
            screenmanager.Create(0);
            inputmanager = new PCInputAdapter();
            collisioncalculator = new onCollision();
            updatevisitor = new UpdateVisitor(inputmanager, collisioncalculator);
        }

        protected override void LoadContent()
        {
            bg = Content.Load<Texture2D>("stara");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawmanager = new MonoGameAdapter(spriteBatch, Content);
            drawvisitor = new DrawVisitor(drawmanager);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            screenmanager.Update(updatevisitor, (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            
            base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Rectangle(0, 0, 800, 480), Color.White);
            screenmanager.Draw(drawvisitor, 0f);
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}