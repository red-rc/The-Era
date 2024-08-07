using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public static class ResourceManager
{
    private static ContentManager Content;

    public static void Initialize(ContentManager content)
    {
        Content = content;
    }

    public static Texture2D LoadTexture(string textureName)
    {
        return Content.Load<Texture2D>(textureName);
    }
    public static SpriteFont LoadFont(string FontName)
    {
        return Content.Load<SpriteFont>(FontName);
    }
}
