#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

float3 LightDirection;
float3 LightColor = 1.0;
float3 AmbientColor = 0.35;

sampler2D TextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

sampler NormalSampler : register(s1)
{
	Texture = (NormalTexture);
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR
{
	//color.rgb = (color.r + color.g + color.b);

	//color.r = 1;
	//color.g = 1;
	//color.b = 1;

	//return tex2D(SpriteTextureSampler, input.TextureCoordinates) * color;  // input.TextureCoordinates


	//Look up the texture value
	 float4 tex = tex2D(TextureSampler, texCoord);

	 //Look up the normalmap value
	 float4 normal = 2 * tex2D(NormalSampler, texCoord) - 1.0;

	 // Compute lighting.
	 float lightAmount = max(dot(normal.xyz, LightDirection), 0.0);
	 color.rgb *= AmbientColor + (lightAmount * LightColor);

	 return tex * color;

}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};