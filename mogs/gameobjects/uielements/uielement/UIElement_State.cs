public partial class UIElement : GameObject
{
    public override void SetActive(bool isActive)
    {
        base.SetActive(isActive);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetActive(isActive);
        }
    }

    public override void SetEnabled(bool isEnabled)
    {
        base.SetEnabled(isEnabled);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetEnabled(isEnabled);
        }
    }
}
