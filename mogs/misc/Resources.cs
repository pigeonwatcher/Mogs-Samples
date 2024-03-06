using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

public partial class Resources
{
    /// <summary>
    /// Central resource class where other classes can fetch textures and fonts.
    /// </summary>

    private readonly GraphicsDeviceManager graphics;
    private readonly ContentManager content;

    public static StaticSpriteFont Font1 { get; private set; }
    public static StaticSpriteFont Font2 { get; private set; }
    public static Texture2D EntityAtlas { get; private set; }
    public static Texture2D EmojiAtlas { get; private set; }
    public static Texture2D ItemAtlas { get; private set; }
    public static Texture2D UIAtlas { get; private set; }
    public static Texture2D AccessoryAtlas { get; private set; }
    public static Texture2D MapAtlas { get; private set; }
    public static Texture2D HomeBackgroundAtlas { get; private set; }
    public static Texture2D CatBackground { get; private set; }
    public static GreyScreen GreyScreen { get; private set; }
    public static Effect PaletteEffect { get; private set; }
    public static GraphicsDevice GraphicsDevice { get; private set; }

    private const string pngDirectoryPath = "fonts/";

    public Resources(GraphicsDeviceManager graphics, ContentManager content)
    {
        this.graphics = graphics;
        this.content = content;
    }

    public void Initialise()
    {
        GraphicsDevice = graphics.GraphicsDevice;

        Texture2D greyTexture = new Texture2D(Resources.GraphicsDevice, 1, 1);
        greyTexture.SetData(new[] { new Color(0, 0, 0, 120) });
        GreyScreen = new GreyScreen(greyTexture);
    }

    public void LoadGeneralContent()
    {
        EntityAtlas = content.Load<Texture2D>("sprites/cat/cat_atlas");
        EmojiAtlas = content.Load<Texture2D>("sprites/emoji/emoji_atlas");
        ItemAtlas = content.Load<Texture2D>("sprites/item/item_atlas");
        UIAtlas = content.Load<Texture2D>("sprites/ui/ui_atlas");
        AccessoryAtlas = content.Load<Texture2D>("sprites/cosmetic/accessory_atlas");
        MapAtlas = content.Load<Texture2D>("sprites/map/map_atlas");
        HomeBackgroundAtlas = content.Load<Texture2D>("sprites/background/home_background");
        CatBackground = content.Load<Texture2D>("sprites/background/cat_background");

        PaletteEffect = content.Load<Effect>("shaders/catcolors");

        Font1 = LoadFont("tinypix");
        Font2 = LoadFont("title");
    }

    private StaticSpriteFont LoadFont(string fontName)
    {
#if DESKTOP
        var fntData = File.ReadAllText($"Assets/fonts/{fontName}.fnt");
        var font = StaticSpriteFont.FromBMFont(fntData,
            fileName => File.OpenRead("Assets/fonts/" + fileName),
            graphics.GraphicsDevice);

        return font;
#elif ANDROID 
        string fontFileName = fontName + ".fnt";

        string fntData;
        using (Stream fontStream = context.Assets.Open(pngDirectoryPath + fontFileName))
        using (StreamReader reader = new StreamReader(fontStream))
        {
            fntData = reader.ReadToEnd();
        }

        return StaticSpriteFont.FromBMFont(fntData,
            fileName => context.Assets.Open(pngDirectoryPath + fileName),
            graphics.GraphicsDevice);
#endif
    }
}

public enum Atlas
{
    Cat,
    Emoji,
    Item,
    UI,
}
