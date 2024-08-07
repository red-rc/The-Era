using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using The_Era.GameWindows;

namespace The_Era
{
    public enum GameWindow
    {
        Game,
        MainMenu,
        Settings,
        GameOver
    }

    public class Main : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public GameConfig config = new();
        public GameWindow currentWindow;
        private readonly MainGame Game;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            config.EditConfig("Special Feature", "CreatorTool", "true");
            config.LoadConfig();

            // Тимчасово, тому що game повинна ініціалізуватися при натисканні кнопки грати,
            // А спочатку завжди ініціалізується mainmenu
            Game = new MainGame(this);

            graphics.PreferredBackBufferWidth = config.Width;
            graphics.PreferredBackBufferHeight = config.Height;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ResourceManager.Initialize(Content);

            switch (currentWindow)
            {
                case GameWindow.Game:
                    Game.Initialize();
                    break;
                default:
                    break;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Game.SetSpriteBatch(spriteBatch);

            switch (currentWindow)
            {
                case GameWindow.Game:
                    Game.LoadContent();
                    break;
                default:
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            switch (currentWindow)
            {
                case GameWindow.Game:
                    Game.Update(gameTime);
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch(currentWindow)
            {
                case GameWindow.Game:
                    Game.Draw(gameTime);
                    break;
                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
