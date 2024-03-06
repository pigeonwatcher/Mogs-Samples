using Microsoft.Xna.Framework;

public class HungerSystem : IEntitySystem, IUpdateSystem
{
    public bool Active { get; private set; }

    private PlayerCat playerCat;
    private float elapsedTime = 0;

    private const float hungerDecreaseInterval = 1f * 60;

    public void SetActive(bool isActive)
    {
        Active = isActive;
    }

    public void RegisterEntity(Entity entity)
    {
        if (entity is PlayerCat playerCat)
        {
            this.playerCat = playerCat;
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entity is PlayerCat)
        {
            playerCat = null;
        }
    }

    public void Update(GameTime gameTime)
    {
        if (!Active || playerCat == null) return;

        elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (elapsedTime > hungerDecreaseInterval)
        {
            elapsedTime = 0;
            playerCat.Hungry(); // Increase hunger.
        }
    }
}