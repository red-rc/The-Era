using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using The_Era.MapRepository;

namespace The_Era.SceneRepository
{
    public class Scene
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public MapEditor mapEditor;
        public Map map;

        public Scene(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
        }

        public void LoadContent()
        {
            LoadMap();

            if (GameConfig.CreatorTool == true)
            {
                mapEditor = new MapEditor(graphics, map);
            }
        }

        private void LoadMap()
        {
            map = new Map(graphics);
            map.LoadTexturesJson();

            map.LoadMapJson();
        }

        public void Update(GameTime gameTime)
        {
            if (GameConfig.CreatorTool == true)
            {
                mapEditor.Update(gameTime);
            }
        }
        public void Draw(GameTime gameTime)
        {
            if (GameConfig.CreatorTool == true)
            {
                mapEditor.Draw(gameTime, spriteBatch);
            }
            else
            {
                map.Draw(spriteBatch);
            }
        }
    }
}
