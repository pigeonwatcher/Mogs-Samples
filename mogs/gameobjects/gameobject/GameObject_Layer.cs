public partial class GameObject
{
    public virtual void SetLayer(float layer)
    {
        LayerDepth = layer;
    }

    public virtual void AdjustLayer(float adjustment)
    {
        LayerDepth += adjustment;
    }
}
