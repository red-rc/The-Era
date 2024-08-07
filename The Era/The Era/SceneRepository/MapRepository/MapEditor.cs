using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using The_Era.Components;
namespace The_Era.MapRepository
{
    public class MapEditor
    {
        private readonly Map map;

        private readonly List<Button> Buttons;

        public Dictionary<string, Texture2D> textures = new();
        private KeyboardState previousKeyboardState;
        private Texture2D CurrentTexture { get; set; }
        private Tile HoveredTile { get; set; }

        public GraphicsDeviceManager graphics;
        private static int incrementor = 1;

        public MapEditor(GraphicsDeviceManager graphics, Map map)
        {
            this.graphics = graphics;
            this.map = map;
            this.textures = map.textures;
            this.Buttons = new List<Button>();

            if (map.Tiles.Count == 0)
            {
                CreateMap(map);
            }

            Initialize();
        }

        public void Initialize()
        {
            int y = graphics.PreferredBackBufferHeight - 32;
            int x = 0;

            foreach (var texture in textures)
            {
                if (x + 32 > graphics.PreferredBackBufferWidth)
                {
                    y -= 32;
                    x = 0;
                }

                Buttons.Add(new Button(texture.Value, new Vector2(x, y), 32, 32));
                x += 32;
            }
            foreach (Button button in Buttons)
            {
                button.ClickAction = () =>
                {
                    CurrentTexture = button.GetTexture();
                };
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Button button in Buttons)
            {
                button.Update(gameTime);
            }


            if (Mouse.GetState().LeftButton == ButtonState.Pressed && CurrentTexture != null && !Buttons.Any(button => button.IsHovering()))
            {
                HoveredTile = map.GetHoveredTile();
                if (HoveredTile != null)
                {
                    HoveredTile.TextureId = textures.FirstOrDefault(key => key.Value == CurrentTexture).Key;
                }
            }

            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
            {
                HoveredTile.Rotation += 90;
                if (HoveredTile.Rotation >= 360)
                {
                    HoveredTile.Rotation -= 360;
                }
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
            {
                HoveredTile.Rotation -= 90;
                if (HoveredTile.Rotation < 0)
                {
                    HoveredTile.Rotation += 360;
                }
            }

            previousKeyboardState = currentKeyboardState;

            map.Update(gameTime);
        }

        public static void CreateMap(Map map)
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    map.Tiles.Add(new Tile { Id = incrementor++, X = x, Y = y, Rotation = 0, TextureId = "Grass", IsCollision = false });
                }
            }

            map.SaveMapJson();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Draw(spriteBatch);
            foreach (Button button in Buttons)
            {
                button.Draw(gameTime, spriteBatch);
            }
        }
    }
}