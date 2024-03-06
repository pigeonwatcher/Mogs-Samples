using ECS;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CatInteractionSystem : IEntitySystem, ITouchSystem
{
    public bool IsActive { get; private set; }

    private Cat[] cats = new Cat[2];
    private bool[] isTapped = new bool[2]; 

    private int swipeThreshold = 30; 
    private Point? swipePosition = null;

    public event Action<Cat> CatTapped;

    private const int PLAYER_CAT_INDEX = 0;
    private const int CAT_PAL_INDEX = 1;

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void RegisterEntity(Entity entity)
    {
        if (entity is Cat cat)
        {
            int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

            cats[index] = cat;
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entity is Cat cat)
        {
            int index = cat is PlayerCat ? PLAYER_CAT_INDEX : CAT_PAL_INDEX;

            cats[index] = null;
        }
    }

    public void OnTouch(IInputService input)
    {
        if (!IsActive) { return; }

        if (swipePosition == null)
        {
            swipePosition = input.InitialTouchPosition;

            for (int i = 0; i < cats.Length; i++)
            {
                if (cats[i] != null && cats[i].Bounds.Contains(input.TouchPosition)) 
                {
                    isTapped[i] = true;
                }
            }
        }

        if (Math.Abs(input.TouchPosition.X - swipePosition.Value.X) > swipeThreshold || 
        Math.Abs(input.TouchPosition.Y - swipePosition.Value.Y) > swipeThreshold)
        {
            swipePosition = input.TouchPosition;

            for (int i = 0; i < isTapped.Length; i++)
            {
                isTapped[i] = false;
            }

            for (int i = 0; i < cats.Length; i++)
            {
                if (cats[i] != null && cats[i].Bounds.Contains(input.TouchPosition) && !Game1.IsItemDragging)
                {
                    cats[i].Pet();
                }
            }
        }
    }

    public void OnRelease(IInputService input)
    {
        if (!IsActive) { return; }

        swipePosition = null;

        for (int i = 0; i < isTapped.Length; i++)
        {
            if (isTapped[i])
            {
                CatTapped?.Invoke(cats[i]);

                isTapped[i] = false;
            }
        }
    }
}
