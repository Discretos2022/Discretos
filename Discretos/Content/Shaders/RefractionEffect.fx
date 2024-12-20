//
// 2d Refraction related shaders
//
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

//-----------------------------------------------------------------------------
//
// Requisite variable and texture samplers
//
//-----------------------------------------------------------------------------

// This vector should be in motion in order to achieve the desired effect.
float2 DisplacementMotionVector;
float SampleWavelength;
float Frequency; // .51  .3
float RefractiveIndex;
// one more little test here.
float2 RefractionVector;
float RefractionVectorRange;

Texture2D Texture : register(t0);
sampler TextureSampler : register(s0)
{
    Texture = (Texture);
};

Texture2D DisplacementTexture;
sampler2D DisplacementSampler = sampler_state
{
    magfilter = linear; 
    minfilter = linear;
    AddressU = Wrap;
    AddressV = Wrap;
    Texture = <DisplacementTexture>;
};

//-----------------------------------------------------------------------------
//
// Requisite functions.
//
//-----------------------------------------------------------------------------


// function reflect map pixel grabber my version
float4 FuncMyRefractMap(float4 color, float2 texCoord)
{
    // the SampleWavelength grabs a smaller or larger area of texture coordinates.
    // the motion moves the uvs over the refraction map to give the feeling of motion.
    // the Frequency reduces the manitude of the range the coordinates are grabed from.
    texCoord += (tex2D(DisplacementSampler, texCoord * SampleWavelength + DisplacementMotionVector).xy - float2(0.5f, 0.5f)) * Frequency;
    return tex2D(TextureSampler, texCoord) * color;    
}

// function reflect map pixel grabber the ported version.
float4 FuncGuysRefractMap(float4 color, float2 texCoord)
{
    texCoord += tex2D(DisplacementSampler, texCoord * SampleWavelength + DisplacementMotionVector) * 0.2 - 0.15;
    return tex2D(TextureSampler, texCoord) * color;
}

// function reflect map pixel grabber hlsl snells
float4 FuncHlslRefractMap(float2 ray, float4 color, float2 texCoord)
{
    float2 normalValue = abs(normalize(tex2D(DisplacementSampler, texCoord * SampleWavelength + DisplacementMotionVector).rg));
    texCoord = texCoord + refract(ray, normalValue, RefractiveIndex); //* Amplitude;
    // Look up into the main textur's pixel and return.
    return tex2D(TextureSampler, texCoord) * color;
}

// function monochrome
float4 FuncMonoChrome(float4 col)
{
    col.rgb = (col.r + col.g + col.b) / 3.0f;
    return col;
}

// function outline
float4 FuncDiagnalAvg(float4 col, float2 texCoord)
{
    col -= tex2D(TextureSampler, texCoord.xy - 0.003) * 2.7f;
    col += tex2D(TextureSampler, texCoord.xy + 0.003) * 2.7f;
    return col;
}

// function outline
float4 FuncDiagnalAvgMonochrome(float4 col, float2 texCoord)
{
    col -= tex2D(TextureSampler, texCoord.xy - 0.003) * 2.7f;
    col += tex2D(TextureSampler, texCoord.xy + 0.003) * 2.7f;
    col.rgb = (col.r + col.g + col.b) / 3.0f;
    return col;
}

// function highlight golden ratio steped.
float4 FuncHighlight(float4 col, float2 texCoord)
{
    // try to brighten on blue.
    float2 temp = texCoord;
    float dist = 0.01f;
    float gldr = 1.6f;
    temp += float2(dist * gldr, 0.00f);
    float4 col01 = tex2D(TextureSampler, temp);
    temp = texCoord;
    temp += float2(0.00f, dist * gldr * 2.0f);
    float4 col03 = tex2D(TextureSampler, temp);
    temp = texCoord;
    temp += float2(-dist * gldr * 3.0f, 0.00f);
    float4 col02 = tex2D(TextureSampler, temp);
    temp = texCoord;
    temp += float2(0.00f, -dist * gldr * 4.0f);
    float4 col04 = tex2D(TextureSampler, temp);

    float4 tempCol = ((col01 + col02 + col03 + col04) - col) / 2.0f;
    tempCol.bg = col.bg;

    col.rgb = saturate(col.rgb * tempCol.rgb);

    return col;
}



//-----------------------------------------------------------------------------
//
// Requisite Shaders.
//
//-----------------------------------------------------------------------------



//__________________________________________________
// (Tech)  refraction map shader mine
//__________________________________________________

float4 PsRefractionMap(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 col = FuncMyRefractMap(color, texCoord.rg);
    return col;
}

//__________________________________________________
// (Tech) refraction map shader guys
//__________________________________________________

float4 PsRefraction2(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 col = FuncGuysRefractMap(color, texCoord.rg);
    return col;
}

//__________________________________________________
// (Tech) RefractDiagnalAverageMonochrome
//__________________________________________________

float4 PsDiagnalAverageMonochromeShader(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 col = FuncMyRefractMap(color, texCoord.rg);
    col = FuncDiagnalAvgMonochrome(col, texCoord.rg);
    return col;
} 

//__________________________________________________
// (Tech)  the golden ratio spiral highlight
//__________________________________________________

float4 PsRefractGoldenRatioHighlight(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{ 
    float4 col = FuncMyRefractMap(color, texCoord.rg);
    col =FuncHighlight(col, texCoord.rg);
    return col;
}

//__________________________________________________
// (Tech) RefractMonoCromeClipDarkness
//__________________________________________________

float4 PsRefractMonoCromeClipDarkness(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 col = FuncMyRefractMap(color, texCoord.rg);
    col = FuncMonoChrome(col);
    clip(col.r - 0.4f);
    return col;
}

//__________________________________________________
// (Tech) RefractAntiRefractionArea  
// this prevents a area around the point from being refracted
//__________________________________________________

float4 PsRefractAntiRefractionArea(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    /*if (position.x > 100 && position.x < 600)
    {
        // Determine distance to the anti Refract position.
        float dist = saturate(distance(position.xy, RefractionVector) / RefractionVectorRange);
        float2 warpedCoords = texCoord + (tex2D(DisplacementSampler, texCoord * SampleWavelength + DisplacementMotionVector).xy - float2(0.5f, 0.5f)) * Frequency;
        float2 lerpedCoords = (warpedCoords - texCoord) * (dist * dist) + texCoord;
        float4 col = tex2D(TextureSampler, saturate(lerpedCoords)) * color;
        // Visually highlight effect range.
        //col.b += (1.0f - dist) * (1.0f - dist);
        //col.r += (dist) * (dist);
        
        return col;
        
    }
    
    //return tex2D(TextureSampler, texCoord);*/
    
    // Determine distance to the anti Refract position.
    float dist = saturate(distance(position.xy, RefractionVector) / RefractionVectorRange);
    float2 warpedCoords = texCoord + (tex2D(DisplacementSampler, texCoord * SampleWavelength + DisplacementMotionVector).xy - float2(0.5f, 0.5f)) * Frequency;
    float2 lerpedCoords = (warpedCoords - texCoord) * (dist * dist) + texCoord;
    float4 col = tex2D(TextureSampler, saturate(lerpedCoords)) * color;
    // Visually highlight effect range.
    //col.b += (1.0f - dist) * (1.0f - dist);
    //col.r += (dist) * (dist);
        
    return col;
    
}

//__________________________________________________
// (Tech) LimitedRefractionArea  
// this only creates a warped inverse refraction area around the point
//__________________________________________________
 
float4 PsLimitedRefractionArea(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Determine distance to the anti Refract position.
    float dist = saturate(distance(position.xy, RefractionVector) / RefractionVectorRange);
    float warpedCoords = (tex2D(DisplacementSampler, texCoord * SampleWavelength + DisplacementMotionVector).xy - float2(0.5f, 0.5f)) * Frequency;
    float2 lerpedCoords = (warpedCoords) * (1.0f - dist) * (1.0f - dist) + texCoord;
    float4 col = tex2D(TextureSampler, saturate(lerpedCoords)) * color;
    // Visually highlight effect range.
    //col.r += (1.0f - dist) * (1.0f - dist);
    //col.b += (dist) * (dist);
    return col;
}

//__________________________________________________
// (Tech) TwoPassTechnique  uses mine modified.
// this is for the double pass test shader.
//__________________________________________________

float4 PsSecondaryShader(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // the frequency grabs a smaller area of texture coordinates the motion moves the uvs over the refraction map.
    float2 coordRange = texCoord * SampleWavelength + DisplacementMotionVector;
    // brightspots and color change well add this last.
    float2 brightspots = abs(float2(sin(coordRange.x), cos(coordRange.y)));
    // we get the texels from the refraction map and shrink grab range.
    texCoord += (tex2D(DisplacementSampler, coordRange).rg - float2(0.5f, 0.5f)) * Frequency;
    // Look up into the main texture.
    float4 col = tex2D(TextureSampler, saturate(texCoord)) * color * float4(brightspots.x, brightspots.y, 1.0f, 0.5f);

    return col;
}

//__________________________________________________
// Unfortunately i cant seem to figure out how to do the 2d one.
// the 3d one seems pretty simple.
// but as it is this ones busted.
//
// (Tech) refraction hlsl call using snells law.
//__________________________________________________

float4 PsRefractionSnells(float4 position : SV_Position, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 ray = float2(0.5f, .5f); //  float2(0.707106f, 0.707106f);
    float4 col = FuncHlslRefractMap(ray, color, texCoord.rg);
    return col;
}



//-----------------------------------------------------------------------------
//
// Requisite Techniques.
//
//-----------------------------------------------------------------------------


technique Refraction2
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefraction2(); // ps_2_0  doesn't error either.
    }
}

technique RefractionMap
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractionMap();
    }
}

technique RefractDiagnalAverageMonochrome
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsDiagnalAverageMonochromeShader();
    }
}

technique RefractGoldenRatioHighlight
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractGoldenRatioHighlight();
    }
}

technique RefractMonoCromeClipDarkness
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractMonoCromeClipDarkness();
    }
}

technique RefractAntiRefractionArea
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractAntiRefractionArea();
    }
}

technique LimitedRefractionArea
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsLimitedRefractionArea();
    }
}

technique RefractionSnells
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractionSnells();
    }
}
technique TwoPassTechnique
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractionMap();
    }
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL
        PsSecondaryShader();
    }
}



//////********************************************************************************************************************



Texture2D SpriteTexture;
float pixelisation;

float MousePosX;
//float MousePosY;

sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR
{
    float pixels = 64.0f;
    float pixelation = pixelisation;
    float mx = input.TextureCoordinates.x * pixels;
    float my = input.TextureCoordinates.y * pixels;

    float x = round(mx / pixelation) * pixelation;
    float y = round(my / pixelation) * pixelation;
    float2 coord = float2(x / pixels, y / pixels);

    //float4 col = tex2D(TextureSampler, texCoord) * color;
    //col = FuncMonoChrome(col);
    //clip(col.r - 0.4f);

    return tex2D(SpriteTextureSampler, coord);
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};






float4 Shadowing(VertexShaderOutput input, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR
{
    float4 col = tex2D(TextureSampler, texCoord);

    float num = MousePosX;

    //if(MousePosX <= 1000)
    //col.rgb = (col.r + col.g + col.b) / (MousePosX/2);
    //else
        //col.rgb = (color.r + color.g + color.b);

    //clip(col.g - 0);

    saturate(col.rgb * num);

    return tex2D(SpriteTextureSampler, texCoord) * MousePosX;
}

technique Shadow
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL Shadowing();
    }
};











technique MyPassTechnique
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL
        PsRefractionMap();
    }
	pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL
        PsLimitedRefractionArea();
    }
	pass Pass2
    {
        PixelShader = compile PS_SHADERMODEL
        PsDiagnalAverageMonochromeShader();
    }
}
