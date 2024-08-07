using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace The_Era.MapRepository
{
    public class Camera
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly Zoom zoom;
        private Vector2 mousePos;
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        public Tile RightTile { get; set; }
        public Tile LeftTile { get; set; }
        public Tile UpTile { get; set; }
        public Tile DownTile { get; set; }
        public Vector2 CameraPos { get; set; }
        public Matrix CameraMatrix { get; private set; }

        private Vector2 cameraMovement;
        private readonly float moveSpeed = 5f;

        public Camera(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;

            cameraMovement = new Vector2();
            zoom = new Zoom();

            CameraMatrix = Matrix.Identity;
        }

        public void SetCamera(List<Tile> tiles)
        {
            this.Tiles = tiles;

            this.RightTile = Tiles.FirstOrDefault(tile => tile.X == Tiles.Max(t => t.X));
            this.LeftTile = Tiles.FirstOrDefault(tile => tile.X == Tiles.Min(t => t.X));
            this.DownTile = Tiles.FirstOrDefault(tile => tile.Y == Tiles.Max(t => t.Y));
            this.UpTile = Tiles.FirstOrDefault(tile => tile.Y == Tiles.Min(t => t.Y));
        }

        public void Update(GameTime gameTime, MouseState mouseState, int scale)
        {
            zoom.Update(gameTime, mouseState);

            this.mousePos = new Vector2(mouseState.X, mouseState.Y);

            if (mousePos.X >= graphics.PreferredBackBufferWidth - 10)
            {
                if (RightTile.X * scale + GetCameraPos().X >= graphics.PreferredBackBufferWidth)
                {
                    cameraMovement.X -= moveSpeed;
                }
            }

            if (mousePos.X <= 10)
            {
                if (LeftTile.X * scale + GetCameraPos().X < 0)
                {
                    cameraMovement.X += moveSpeed;
                }
            }

            if (mousePos.Y >= graphics.PreferredBackBufferHeight - 10)
            {
                if (DownTile.Y * scale + GetCameraPos().Y >= graphics.PreferredBackBufferHeight)
                {
                    cameraMovement.Y -= moveSpeed;
                }
            }

            if (mousePos.Y <= 10)
            {
                if (UpTile.Y * scale + GetCameraPos().Y < 0)
                {
                    cameraMovement.Y += moveSpeed;
                }
            }

            CameraMatrix = Matrix.CreateTranslation(cameraMovement.X, cameraMovement.Y, 0);
            CameraMatrix = Matrix.CreateScale(zoom.GetScrollValue());
            // Там купа методів, подивись потім
        }

        public Vector2 GetCameraPos()
        {
            return new(CameraMatrix.Translation.X, CameraMatrix.Translation.Y);
        }
        public int GetCameraScale()
        {
            return zoom.GetScrollValue();
        }
    }
}
