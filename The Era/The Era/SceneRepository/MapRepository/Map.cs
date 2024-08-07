using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using System.Diagnostics;

namespace The_Era.MapRepository
{
    public class Tile
    {
        public int Id = 1;
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; set; }
        public string TextureId { get; set; }
        public bool IsCollision { get; set; }
    }

    public class Map
    {
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        private readonly string mapPath = "map.json";
        private readonly string texturesPath = "textures.json";

        public Dictionary<string, Texture2D> textures = new();
        private readonly GraphicsDeviceManager graphics;

        private readonly Camera camera;

        public Vector2 tilePos;
        public Vector2 cameraPos;
        public float tileRotation;

        public int scale;

        public Map(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;

            if (!File.Exists(mapPath))
            {
                MapEditor.CreateMap(this);
            }

            camera = new Camera(graphics);
        }

        public void LoadTexturesJson()
        {
            if (!File.Exists(texturesPath))
            {
                System.Windows.Forms.MessageBox.Show("textures.json doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var json = File.ReadAllText(texturesPath);
            var texturePaths = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            if (texturePaths == null)
            {
                System.Windows.Forms.MessageBox.Show("textures.json has no data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var texture in texturePaths)
            {
                textures.Add(texture.Key, ResourceManager.LoadTexture(texture.Value));
            }
        }

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime, Mouse.GetState(), scale);

            cameraPos = camera.GetCameraPos();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in Tiles)
            {
                Texture2D texture = textures[tile.TextureId];
                scale = texture.Width + camera.GetCameraScale();
                Vector2 tileOrigin = new(texture.Width / 2, texture.Height / 2);
                // Застосуйте матрицю камери до позиції тайла
                tilePos = Vector2.Transform(new Vector2(tile.X * scale, tile.Y * scale), camera.CameraMatrix);
                //tilePos = new Vector2(tile.X * scale, tile.Y * scale) + cameraPos + tileOrigin * 2;

                if (tilePos.X < graphics.PreferredBackBufferWidth + scale && tilePos.Y < graphics.PreferredBackBufferHeight + scale)
                {
                    spriteBatch.Draw(
                        texture,
                        tilePos,
                        null,
                        Color.White,
                        MathHelper.ToRadians(tile.Rotation),
                        tileOrigin,
                        new Vector2((float)scale / texture.Width, (float)scale / texture.Height),
                        SpriteEffects.None,
                        0
                    );
                }
            }
        }

        public Tile GetHoveredTile()
        {
            Vector2 mousePos = new(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

            int tileX = (int)((mousePos.X - cameraPos.X) / scale);
            int tileY = (int)((mousePos.Y - cameraPos.Y) / scale);

            return Tiles.Find(tile => tile.X == tileX && tile.Y == tileY);
        }

        public void SaveMapJson()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            Json.SaveJson(json, mapPath);
        }

        public void LoadMapJson()
        {
            if (!File.Exists(mapPath))
            {
                System.Windows.Forms.MessageBox.Show("map.json doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var json = Json.LoadJson(mapPath);
            var map = JsonConvert.DeserializeObject<Map>(json);
            if (map == null)
            {
                System.Windows.Forms.MessageBox.Show("map.json has no data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Tiles = map.Tiles;

            camera.SetCamera(Tiles);
        }
    }
}
