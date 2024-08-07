using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace The_Era.Components
{
    public class Button : Component
    {
        private MouseState MouseState { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }

        private Texture2D Texture { get; set; }
        private Vector2 Pos { get; set; }
        private Rectangle Rect { get; set; }
        private Color Color { get; set; }

        public Action ClickAction { get; set; }

        private string Text { get; set; }

        private SpriteFont Font { get; set; }
        private Vector2 FontPos { get; set; }

        private bool Hovering { get; set; }

        public Button(Texture2D texture, Vector2 pos, int width, int height)
        {
            this.Texture = texture;
            this.Pos = pos;
            this.Width = width;
            this.Height = height;
            this.Color = Color.White;
        }

        public Button(Vector2 pos, int width, int height)
        {
            this.Texture = ResourceManager.LoadTexture("textures/ground");
            this.Pos = pos;
            this.Width = width;
            this.Height = height;
            this.Color = Color.White;
        }

        public void SetText(string text)
        {
            Font = ResourceManager.LoadFont("fonts/baseFont");
            this.Text = text;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState = Mouse.GetState();

            Rect = new Rectangle(
                (int)Pos.X,
                (int)Pos.Y,
                Width,
                Height
            );

            if (Rect.Contains(MouseState.Position))
            {
                Hovering = true;
            }
            else
            {
                Hovering = false;
            }

            if (Hovering && MouseState.LeftButton == ButtonState.Pressed)
            {
                OnClick();
            }

            if (!string.IsNullOrWhiteSpace(Text))
            {
                FontPos = new Vector2(Pos.X + (Width / 2), Pos.Y + (Height / 2));
            }
        }

        public void OnClick()
        {
            ClickAction?.Invoke();
        }

        public bool IsHovering()
        {
            return Hovering;
        }

        public Texture2D GetTexture()
        {
            return Texture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Pos, null, Color, 0f, Vector2.Zero, new Vector2(Width / (float)Texture.Width, Height / (float)Texture.Height), SpriteEffects.None, 0f);

            if (!string.IsNullOrWhiteSpace(Text))
            {
                spriteBatch.DrawString(Font, Text, FontPos - Font.MeasureString(Text) / 2, Color);
            }
        }
    }
}