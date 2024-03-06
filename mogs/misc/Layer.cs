public static class Layer
{
    /// <summary>
    /// Layer class used to set the layer depth for GameObjects when drawn. 
    /// </summary>

    public const float FRONT = 0;
    public const float BACK = 1;

    public const float PLAY_LAYER = 0.8f;
    public const float ACCESSORY_LAYER = 0.7f;
    public const float EMOJI_LAYER = 0.6f;

    public const float UI = 0.50f;
    public const float UI_TOOLBAR = 0.49f;
    public const float UI_FOODINV = 0.48f;
    public const float UI_COFFEEINV = 0.47f;
    public const float UI_ACTIVEITEM = 0.42f;
    public const float UI_POPUP = 0.41f;

    public const float UI_CHILDOFFSET = -0.001f;
    public const float UI_GREYSCREENOFFSET = 0.001f;
}
