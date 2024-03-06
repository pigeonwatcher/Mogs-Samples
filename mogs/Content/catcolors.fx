#if OPENGL
    #define SV_POSITION POSITION
    #define VS_SHADERMODEL vs_3_0
    #define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL vs_4_0_level_9_1
    #define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

matrix World;
matrix View;
matrix Projection;

float4 OriginalColor1;
float4 OriginalColor2;
float4 NewColor1;
float4 NewColor2;

struct VertexShaderInput
{
    float4 Position : POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput MainVS(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 pixelColor = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    clip(pixelColor.a - 0.5);
    pixelColor *= input.Color;

    if (pixelColor.r == OriginalColor1.r && pixelColor.g == OriginalColor1.g && pixelColor.b == OriginalColor1.b)
        pixelColor.rgb = NewColor1.rgb;
    else if (pixelColor.r == OriginalColor2.r && pixelColor.g == OriginalColor2.g && pixelColor.b == OriginalColor2.b)
        pixelColor.rgb = NewColor2.rgb;

    return pixelColor;
}

technique SpriteDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
}
