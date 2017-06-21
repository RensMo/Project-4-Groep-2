using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Robotparty
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int level;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "content";
            level = 0;
        }
        // inputManager inputmanager;
        // drawManager drawmanager;
        // UpdateVisitor updatevisitor;
        // DrawVisitor drawvisitor;
        // List<ScreenManager> screenmanagers;

        protected override void Initialize() {
            base.Initialize();
            this.IsMouseVisible = false;
            // screenmanagers = new List<ScreenManager>;
            // screenmanagers.add(new ScreenManager);
            // inputmanager = new PCInputAdapter;
            // updatevisitor = new UpdateVisitor(inputmanager);
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // drawmanager = new MonoGameAdapter(spriteBatch, Content);
            // drawvisitor = new DrawVisitor(drawmanager);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // screenmanagers[level].Update(updatevisitor, (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            base.Update(gameTime);
        }
    }
}
