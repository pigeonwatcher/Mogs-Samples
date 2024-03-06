public partial class GameObject
{
    public virtual void SetActive(bool isActive)
    {
        Active = isActive;
    }

    public virtual void SetEnabled(bool isEnabled)
    {
        Enabled = isEnabled;
    }
}
