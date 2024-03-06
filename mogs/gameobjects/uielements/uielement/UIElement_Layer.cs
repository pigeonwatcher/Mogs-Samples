public partial class UIElement : GameObject
{
    public override void SetLayer(float layer)
    {
        base.SetLayer(layer);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetLayer(LayerDepth + Layer.UI_CHILDOFFSET);
        }
    }

    public override void AdjustLayer(float adjustment)
    {
        base.AdjustLayer(adjustment);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].AdjustLayer(adjustment);
        }
    }
}
