#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0 /// 3
	#define PS_SHADERMODEL ps_3_0 /// 3
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1 // 4
	#define PS_SHADERMODEL ps_4_0_level_9_1 // 4
#endif

Texture2D SpriteTexture;

float2 LightPosition[100];


float NumOfLight;

float x;
float y;
float p;
float o;
float b;

float ResolutionX;
float ResolutionY;

float4 AmbientLight;

float diviseur;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD;
};

//float4 lights;



float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 col = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	//float4 avgColor = (col.r + col.g + col.b) / 3;

	/// effet ombre
	col.rgba = col.rgba * (input.TextureCoordinates.y * 1 - 0.4f);


	return col;


	/// pixelisation ////////////////////////////////////////////////////////////////////////
	/*float pixels = 64.0f;
	float pixelation = 0.1f;
	float mx = input.TextureCoordinates.x * pixels;
	float my = input.TextureCoordinates.y * pixels;

	float x = round(mx / pixelation) * pixelation;
	float y = round(my / pixelation) * pixelation;
	float2 coord = float2(x / pixels, y / pixels);

	return tex2D(SpriteTextureSampler, coord);*/
	////////////////////////////////////////////////////////////////////////////////////////


}

float4 RDT(VertexShaderOutput input) : COLOR
{
	float4 col = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	return col;

}


float4 Light1(VertexShaderOutput input) : COLOR
{
	
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	float4 lights = float4(x, y, p, o);

	float4 lightColor = float4(1, 1, 1, 100);


	//Convert the TextureCoordinate into pixel coordinates
	float2 pixelLocation = float2(1280, 720) * input.TextureCoordinates;

	//The final light level after calculations are complete
	float4 result = 0;

	float currentResult;


	//r and g are the x and y coordinates of the point light
	//b is the radius of the light
	//a is the opacity or brightness of the light									
	currentResult = lights.b - distance(pixelLocation, float2(lights.r, lights.g));
	currentResult /= lights.b;
	currentResult *= lights.a;
	if (currentResult > 0) result += lightColor * currentResult;


	//Don't allow light level to go below .2
	//if (result < b) result = b;
	if (result.r < b) result.r = b;
	if (result.g < b) result.g = b;
	if (result.b < b) result.b = b;

	return result * color;


}


float4 Light2(VertexShaderOutput input) : COLOR
{

	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	float4 lights = float4(x, y, p, o);

	float4 lightColor = float4(1.5f, 1, 0, 10);


	//Convert the TextureCoordinate into pixel coordinates
	float2 pixelLocation = float2(1280, 720) * input.TextureCoordinates;

	//The final light level after calculations are complete
	float4 result = 0;

	float currentResult;


	//r and g are the x and y coordinates of the point light
	//b is the radius of the light
	//a is the opacity or brightness of the light									 ///
	currentResult = lights.b - distance(pixelLocation, float2(lights.r, lights.g)) / 10;
	currentResult /= lights.b;
	currentResult *= lights.a;
	if (currentResult > 0) result += lightColor * currentResult;


	//Don't allow light level to go below .2
	//if (result < b) result = b;
	if (result.r < b) result.r = b;
	if (result.g < b) result.g = b;
	if (result.b < b) result.b = b;

	return result * color;


}


float4 Light3(VertexShaderOutput input) : COLOR
{

	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	float4 lights = float4(x, y, p, o);

	float4 lightColor = float4(1, 1, 1, 100);


	//Convert the TextureCoordinate into pixel coordinates
	float2 pixelLocation = float2(ResolutionX, ResolutionY) * input.TextureCoordinates;

	//The final light level after calculations are complete
	float4 result = 0;

	float Pixeldistance;
	float currentResult;


	//r and g are the x and y coordinates of the point light
	//b is the radius of the light
	//a is the opacity or brightness of the light	

	Pixeldistance = distance(pixelLocation, float2(lights.r, lights.g));

	if(Pixeldistance < 50) Pixeldistance = 50;
	
	currentResult = 1 / Pixeldistance;
	currentResult *= lights.b;
	currentResult /= lights.a;

	if (currentResult > 0) result += lightColor * currentResult;


	//Don't allow light level to go below .2
	//if (result < b) result = b;
	if (result.r < b) result.r = b;
	if (result.g < b) result.g = b;
	if (result.b < b) result.b = b;


	

	return result * color;


}


/// RayTracer RTX
float4 Light4(VertexShaderOutput input) : COLOR
{

	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	float4 lights = float4(x, y, p, o);

	float4 lightColor = float4(1, 1, 1, 100);


	//Convert the TextureCoordinate into pixel coordinates
	float2 pixelLocation = float2(ResolutionX, ResolutionY) * input.TextureCoordinates;

	//The final light level after calculations are complete
	float4 result = 0;

	float Pixeldistance;
	float currentResult;


	result.r = AmbientLight.r;
	result.g = AmbientLight.g;
	result.b = AmbientLight.b;

	[unroll]
	for (int i = 0; i < NumOfLight; i++)
	{

		//r and g are the x and y coordinates of the point light
		//b is the radius of the light
		//a is the opacity or brightness of the light	

		Pixeldistance = distance(pixelLocation, float2(LightPosition[i].r, LightPosition[i].g));

		//if (Pixeldistance < 50) Pixeldistance = 50;

		if (Pixeldistance < 3000) // 120
		{
			currentResult = diviseur / Pixeldistance;
			currentResult *= lights.b;
			currentResult /= lights.a;

			if (currentResult > 0) result += lightColor * currentResult;


			//Don't allow light level to go below .2
			//if (result < b) result = b;
			
			if (result.r > 1.4f) result.r = 1.4f;
			if (result.g > 1.4f) result.g = 1.4f;
			if (result.b > 1.4f) result.b = 1.4f;

		}

	}

	if (result.r < b) result.r = b;
	if (result.g < b) result.g = b;
	if (result.b < b) result.b = b;
	

	

	
	result.a = AmbientLight.a;


	return result * color;


}



sampler2D s0;
Texture2D lightMask;
sampler2D lightSampler = sampler_state { Texture = <lightMask>; };

float sature;

//float2 ambianteLightR;
//float2 ambianteLightG;
//float2 ambianteLightB;


float4 LightMask(VertexShaderOutput input, float2 coords: TEXCOORD0, float4 pos: SV_Position, float4 col: COLOR0) : COLOR
{

	float4 color = tex2D(s0, coords);
	float4 lightColor = tex2D(lightSampler, coords);
	
    lightColor.a = 1;
	
    return color * lightColor;


}


Texture2D hullMask;
sampler2D hullSampler = sampler_state { Texture = <hullMask>; };

Texture2D colorMask;
sampler2D colorSampler = sampler_state { Texture = <colorMask>; };

bool DEBUG;


float4 LightAndHullMask(VertexShaderOutput input, float2 coords : TEXCOORD0, float4 pos : SV_Position, float4 col : COLOR0) : COLOR
{
	
    float4 ambiant = tex2D(lightSampler, float2(0, 0));

    float4 pixel = tex2D(s0, coords);
    float4 light = tex2D(lightSampler, coords);
    float4 lightColor = tex2D(colorSampler, coords);
    float4 hull = tex2D(hullSampler, coords);
	

    /*float1 average;
	
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            average += tex2D(hullSampler, pos.xy + float2(i, j)).a;
        }
    }*/
	

    if (hull.a != 0)
    {
		
        ambiant.a = 1;
		
        if (DEBUG)
            return hull; // * (average / 25.0f);
            
		//return float4(0, 0, 0, 1);
		
        return pixel * ambiant;

    }

    light.a = 1;
    return pixel * light;


}


technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL RDT();
	}

	pass P1
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}

	pass P2
	{
		PixelShader = compile PS_SHADERMODEL Light1();
	}

	pass P3
	{
		PixelShader = compile PS_SHADERMODEL Light3();
	}

	pass P4
	{
		PixelShader = compile PS_SHADERMODEL Light4();
	}

	pass P5
	{
		PixelShader = compile PS_SHADERMODEL LightMask();
	}

    pass P6
    {
        PixelShader = compile PS_SHADERMODEL LightAndHullMask();
    }

};