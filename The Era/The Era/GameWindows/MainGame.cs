using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using The_Era.SceneRepository;

namespace The_Era.GameWindows
{
    public class MainGame
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GraphicsDevice device;
        public Main main;
        public Scene scene;

        public GameConfig config;

        public MainGame(Main main)
        {
            this.main = main;
            this.graphics = main.graphics;
            this.device = main.GraphicsDevice;
            this.config = main.config;
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
            main.currentWindow = The_Era.GameWindow.Game;
        }

        public void LoadContent()
        {
            scene = new Scene(graphics, spriteBatch);
            scene.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            scene.Update(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            scene.Draw(gameTime);
        }
    }
}