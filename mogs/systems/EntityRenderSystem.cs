using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

public class EntityRenderSystem : IEntitySystem
{
    public bool IsActive { get; set; }

    private VertexBuffer vertexBuffer;

    private int activeEntities = 0;
    private EntityRenderData[] renderData = new EntityRenderData[MaxEntities];

    private static Vector4[] OriginalPalette = new[] {
       new Color(152, 251, 152).ToVector4(), // Primary Color.
       new Color(173, 216, 230).ToVector4()  // Secondary Color.
    };

    private const int MaxEntities = 20;

    public void SetActive(bool isActive)
    {
        IsActive = isActive;

        if(IsActive && vertexBuffer != null) 
        {
            Resources.GraphicsDevice.SetVertexBuffer(vertexBuffer);
        }
    }

    public void Initialise()
    {
        vertexBuffer = new VertexBuffer(Resources.GraphicsDevice, VertexPositionTexture.VertexDeclaration, 6, BufferUsage.WriteOnly);
        Resources.GraphicsDevice.SetVertexBuffer(vertexBuffer);

        Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 1), Vector3.Zero, Vector3.Up);
        Matrix projection = Matrix.CreateOrthographicOffCenter(0, Resources.GraphicsDevice.Viewport.Width, Resources.GraphicsDevice.Viewport.Height, 0, 1, 0);

        Resources.PaletteEffect.Parameters["View"].SetValue(view);
        Resources.PaletteEffect.Parameters["Projection"].SetValue(projection);
        Resources.PaletteEffect.Parameters["SpriteTexture"].SetValue(Resources.EntityAtlas);
        Resources.PaletteEffect.Parameters["OriginalColor1"].SetValue(OriginalPalette[0]);
        Resources.PaletteEffect.Parameters["OriginalColor2"].SetValue(OriginalPalette[1]);
    }

    public void RegisterEntity(Entity entity)
    {
        if(activeEntities < renderData.Length)
        {
            renderData[activeEntities] = new EntityRenderData(entity);
            activeEntities++;
        }
        else 
        {
            Debug.WriteLine("Max amount of entities reached!");
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        int index = Array.FindIndex(renderData, 0, activeEntities, (renderData) => renderData.Entity == entity);

        if (index != -1)
        {
            renderData[index].Dispose();

            // Shift elements
            for (int i = index; i < activeEntities - 1; i++)
            {
                renderData[i] = renderData[i + 1];
            }
            activeEntities--;
            return;
        }
    }

    public void Render()
    {
        Resources.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        Resources.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        for (int i = 0; i < activeEntities; i++)
        {
            if (renderData[i].Entity == null || !renderData[i].Entity.Active) { continue; }

            vertexBuffer.SetData(renderData[i].Vertices);

            Resources.PaletteEffect.Parameters["World"].SetValue(renderData[i].Entity.MatrixPos);
            Resources.PaletteEffect.Parameters["NewColor1"].SetValue(renderData[i].Entity.Palette[0]);
            Resources.PaletteEffect.Parameters["NewColor2"].SetValue(renderData[i].Entity.Palette[1]);

            foreach (EffectPass pass in Resources.PaletteEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Resources.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }
    }
}

public struct EntityRenderData
{
    public Entity Entity { get; } = null;  
    public VertexPositionTexture[] Vertices { get; } = new VertexPositionTexture[6];

    public EntityRenderData(Entity entity)
    {
        Entity = entity;
        entity.VisualChanged += UpdateVertices;

        UpdateVertices(entity.SpriteRect);
    }

    public void Dispose()
    {
        if (Entity != null)
        {
            Entity.VisualChanged -= UpdateVertices;
        }
    }

    private void UpdateVertices(Rectangle spriteRect)
    {
        Texture2D atlas = Resources.EntityAtlas;
        float layer = Entity.LayerDepth;

        float left = (float)spriteRect.X / atlas.Width;
        float top = (float)spriteRect.Y / atlas.Height;
        float right = (float)(spriteRect.X + spriteRect.Width) / atlas.Width;
        float bottom = (float)(spriteRect.Y + spriteRect.Height) / atlas.Height;

        Vertices[0] = new VertexPositionTexture(new Vector3(0, 0, layer), new Vector2(left, top));
        Vertices[1] = new VertexPositionTexture(new Vector3(0, spriteRect.Height, layer), new Vector2(left, bottom));
        Vertices[2] = new VertexPositionTexture(new Vector3(spriteRect.Width, 0, layer), new Vector2(right, top));
        Vertices[3] = new VertexPositionTexture(new Vector3(spriteRect.Width, 0, layer), new Vector2(right, top));
        Vertices[4] = new VertexPositionTexture(new Vector3(0, spriteRect.Height, layer), new Vector2(left, bottom));
        Vertices[5] = new VertexPositionTexture(new Vector3(spriteRect.Width, spriteRect.Height, layer), new Vector2(right, bottom));
    }
}

public struct VertexPositionTexture
{
    public Vector3 Position;
    public Color Color;
    public Vector2 TextureCoordinate;

    public VertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
    {
        Position = position;
        Color = Color.White;
        TextureCoordinate = textureCoordinate;
    }

    public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
    (
        new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
        new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0),
        new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
    );
}