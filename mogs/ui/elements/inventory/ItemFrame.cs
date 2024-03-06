using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemFrame
{
    public Vector2 Position { get; private set; }
    public Vector2 IdlePosition { get; private set; }

    public void Set(Vector2 position)
    {
        IdlePosition = position;
        Position = IdlePosition;
    }

    public void Adjust(Vector2 adjustment)
    {
        Position += adjustment;
    }

    public void Reset()
    {
        Position = IdlePosition;
    }
}
