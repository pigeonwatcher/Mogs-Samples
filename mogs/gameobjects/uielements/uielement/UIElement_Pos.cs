using Microsoft.Xna.Framework;

public partial class UIElement
{
    public override void MovePos(Vector2 movePos)
    {
        base.MovePos(movePos);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].MovePos(movePos);
        }
    }

    public override void AdjustPos(Vector2 adjustPos)
    {
        base.AdjustPos(adjustPos);

        for (int i = 0; i < children.Count; i++)
        {
            children[i].AdjustPos(adjustPos);
        }
    }

    public override void ResetPos()
    {
        base.ResetPos();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].ResetPos();
        }
    }
}
