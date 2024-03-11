using Mogs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InteractiveInventory : UIElement
{
    private readonly Texture2D UIAtlas;
    private readonly Texture2D ItemAtlas;

    public delegate void ItemTouched(InteractiveItem item, IInputService input = null);
    public delegate void ItemReleased(InteractiveItem item, IInputService input = null);

    public InteractiveItem ActiveItem { get; private set; }

    private ItemTouched itemTouched;
    private ItemReleased itemReleased;

    private int parentWidth;
    private Vector2 parentPosition;
    private Rectangle itemFrameSprite;
    private List<ItemFrame> itemFramePositions = new List<ItemFrame>();
    private List<InteractiveItem> interactiveItems = new List<InteractiveItem>();

    private int itemsPerRow = 3;
    private int itemsPerSlide = 6;
    private int itemOffsetX = 2;
    private int itemOffsetY = 2;
    private int parentMargin = 4;

    private const float itemLayerOffset = Layer.UI_CHILDOFFSET * 2;
    private const float itemFrameLayerOffset = Layer.UI_CHILDOFFSET;

    public InteractiveInventory(Texture2D uiAtlas, Texture2D itemAtlas)
    {
        UIAtlas = uiAtlas;
        ItemAtlas = itemAtlas;

        itemFrameSprite = Sprite.UIData[UI.ItemFrame];
    }

    public void Config(ItemTouched itemTouched = null, ItemReleased itemReleased = null, int itemsPerRow = 0, int itemsPerSlide = 0)
    {
        this.itemTouched = itemTouched;
        this.itemReleased = itemReleased;
        this.itemsPerRow = itemsPerRow;
        this.itemsPerSlide = itemsPerSlide;
    }

    public void SetParent(Rectangle parent)
    {
        parentWidth = parent.Width;
        parentPosition = parent.Location.ToVector2();
    }

    public void Refresh(IEnumerable<ItemID> items)
    {
        interactiveItems.Clear();

        for(int i = 0; i < itemsPerSlide; i++) 
        {
            Vector2 itemFramePos = ItemFramePosition(i);
            ItemFrame itemFrame = new ItemFrame();
            itemFrame.Set(itemFramePos);
            itemFramePositions.Add(itemFrame);
        }

        int itemsCount = items.Count() > itemsPerSlide ? itemsPerSlide : items.Count();

        for(int i = 0; i < itemsCount; i++) 
        {
            Rectangle sprite = Sprite.ItemData[items.ElementAt(i)];
            Vector2 itemPos = new Vector2(itemFramePositions[i].Position.X + itemOffsetX, itemFramePositions[i].Position.Y + itemOffsetY);
            InteractiveItem item = new InteractiveItem(items.ElementAt(i), i, sprite, itemPos);
            interactiveItems.Add(item);
        }
    }

    public void SetActiveItem(InteractiveItem item)
    {
        ActiveItem = item;

        if(item != null) 
        { 
            Game1.IsItemDragging = true;
        }
        else 
        { 
            Game1.IsItemDragging = false;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for(int i = 0; i < itemFramePositions.Count; i++) 
        {
            spriteBatch.Draw(UIAtlas, itemFramePositions[i].Position, itemFrameSprite, ColorUtils.LightOffWhite, 0, Vector2.Zero, 1, SpriteEffects.None, LayerDepth + (itemLayerOffset - itemFrameLayerOffset));
        }

        for (int i = 0; i < interactiveItems.Count; i++)
        {
            if (interactiveItems[i] == ActiveItem)
            {
                interactiveItems[i].Draw(spriteBatch, ItemAtlas, Layer.UI_ACTIVEITEM);
                continue;
            }

            interactiveItems[i].Draw(spriteBatch, ItemAtlas, LayerDepth + itemLayerOffset);
        }
    }

    public override void OnTouch(IInputService input)
    {
        foreach (InteractiveItem item in interactiveItems)
        {
            if (item.Bounds.Contains((Point)input.InitialTouchPosition))
            {
                SetActiveItem(item);
            }
        }

        if (ActiveItem != null)
        {
            itemTouched?.Invoke(ActiveItem, input);
        }
    }

    public override void OnRelease(IInputService input)
    {
        if(ActiveItem != null)
        {
            itemReleased?.Invoke(ActiveItem, input);

            ActiveItem.Reset();
            SetActiveItem(null);
        }
    }

    public override void ResetPos()
    {
        foreach (InteractiveItem item in interactiveItems)
        {
            item.Reset();
        }

        foreach (ItemFrame item in itemFramePositions)
        {
            item.Reset();
        }

        base.ResetPos();
    }

    public override void AdjustPos(Vector2 adjustPosition)
    {
        foreach (InteractiveItem item in interactiveItems)
        {
            item.Adjust(adjustPosition);
        }

        foreach (ItemFrame item in itemFramePositions)
        {
            item.Adjust(adjustPosition);
        }

        base.AdjustPos(adjustPosition);
    }

    private Vector2 ItemFramePosition(int itemIndex)
    {
        int row = itemIndex / itemsPerRow;
        int rowOffset = (itemFrameSprite.Width + parentMargin) * row;
        int rowPos = itemIndex % itemsPerRow;

        int evenWidthSpacing = Grid.EvenWidthSpacing(itemFrameSprite.Width, itemsPerRow, parentWidth, parentMargin);

        Vector2 position = new Vector2(parentPosition.X + parentMargin + (evenWidthSpacing * rowPos), parentPosition.Y + parentMargin + rowOffset);

        return position;
    }
}
