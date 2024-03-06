using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class SceneFade : SceneTransition
{
    public Action TransitionCompleted;

    private Texture2D fadeTexture;
    private Rectangle screenSize;
    private float fadeAlpha;
    private SceneState currentState;

    private const float fadeDuration = 250;

    public enum SceneState
    {
        Idle,
        FadeIn,
        FadeOut,
    }

    public SceneFade(GraphicsDevice graphics)
    {
        fadeTexture = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
        fadeTexture.SetData(new[] { Color.Black });
        Vector2 fadePos = Grid.Position(Anchor.TopLeft, Pivot.TopLeft, Grid.ScreenWidth, Grid.ScreenHeight);
        screenSize = new Rectangle((int)fadePos.X, (int)fadePos.Y, Grid.ScreenWidth, Grid.ScreenHeight);
        currentState = SceneState.Idle;
    }

    public void StartTransitionIn()
    {
        fadeAlpha = 1f;
        currentState = SceneState.FadeIn;
    }

    public void StartTransitionOut()
    {
        fadeAlpha = 0f;
        currentState = SceneState.FadeOut;
    }

    public void Update(GameTime gameTime)
    {
        switch (currentState)
        {
            case SceneState.FadeIn:
                FadeIn(gameTime);
                break;
            case SceneState.FadeOut:
                FadeOut(gameTime);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (currentState != SceneState.Idle)
        {
            spriteBatch.Draw(fadeTexture, screenSize, null, Color.Black * fadeAlpha, 0, Vector2.Zero, SpriteEffects.None, Layer.FRONT);
        }
    }

    private void FadeIn(GameTime gameTime)
    {
        float alphaChangePerFrame = (float)gameTime.ElapsedGameTime.TotalMilliseconds / fadeDuration;
        fadeAlpha -= alphaChangePerFrame;

        if (fadeAlpha <= 0)
        {
            fadeAlpha = 0f;
            currentState = SceneState.Idle;
            TransitionCompleted?.Invoke();
        }
    }

    private void FadeOut(GameTime gameTime)
    {
        float alphaChangePerFrame = (float)gameTime.ElapsedGameTime.TotalMilliseconds / fadeDuration;
        fadeAlpha += alphaChangePerFrame;

        if (fadeAlpha >= 1)
        {
            fadeAlpha = 1f;
            currentState = SceneState.Idle;
            TransitionCompleted?.Invoke();
        }
    }
}

