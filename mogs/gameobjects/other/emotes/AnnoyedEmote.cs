using Microsoft.Xna.Framework;

public class AnnoyedEmote : Emote
{
    public AnnoyedEmote(Cat cat)
    {
        Duration = 4;
        Frames = new Vector2[] { new Vector2(0, -1), new Vector2(0, -1), new Vector2(0, 1), new Vector2(0, 1) };
        FrameRate = 0.35f;

        SetSpriteRect(Sprite.EmojiData[Emoji.AnnoyedEmote]);
        Vector2 emotePos = new Vector2(cat.Position.X + 1, cat.Position.Y - 20);
        SetPos(emotePos);
    }
}