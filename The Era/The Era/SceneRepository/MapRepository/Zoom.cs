using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace The_Era.MapRepository
{
    public enum ScrollVector
    {
        Up,
        Down
    }

    public class Zoom
    {
        private MouseState previousMouseState;
        private MouseState currentMouseState;
        private int scrollDifference = 25;
        private float scrollDelayTimer = 0f;
        private float deltaTime = 0f;
        private bool isScrolling = false;
        private int wantedScrollValue = 0;
        private ScrollVector scrollVector;

        public Zoom()
        {
            previousMouseState = Mouse.GetState();
        }

        public void Update(GameTime gameTime, MouseState currentMouseState)
        {
            this.currentMouseState = currentMouseState;

            if (currentMouseState.ScrollWheelValue != previousMouseState.ScrollWheelValue || isScrolling)
            {
                wantedScrollValue = currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;

                if (!isScrolling)
                {
                    ChangeScrollValue(4);

                    deltaTime = 0f;
                    scrollDelayTimer = 1.5f;
                    isScrolling = true;
                }
                else if (isScrolling)
                {
                    deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    scrollDelayTimer -= deltaTime;
                    ChangeScrollValue(2);

                    if (scrollDelayTimer <= 0f)
                    {
                        isScrolling = false;
                    }
                }

                previousMouseState = currentMouseState;
            }
        }

        private void ChangeScrollValue(int scrollPower)
        {
            if (scrollDifference != wantedScrollValue && currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue || isScrolling && scrollVector == ScrollVector.Up)
            {
                scrollDifference += scrollPower;
                scrollVector = ScrollVector.Up;

            }
            else if (scrollDifference != wantedScrollValue && currentMouseState.ScrollWheelValue < previousMouseState.ScrollWheelValue || isScrolling && scrollVector == ScrollVector.Down)
            {
                scrollDifference -= scrollPower;
                scrollVector = ScrollVector.Down;
            }

            scrollDifference = GameConfig.CreatorTool == true ? MathHelper.Clamp(scrollDifference, 0, 100) : MathHelper.Clamp(scrollDifference, 25, 100);
        }

        public int GetScrollValue()
        {
            return scrollDifference;
        }
    }
}
