Shader "Storky/Island Sky"
{
    // Gökyüzü renkleri
    Properties
    {
        _HorizonColor("Horizon", Color)=(.50,.66,.73,1)
        _ZenithColor("Zenith", Color)=(.22,.48,.72,1)
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Materyal verisi
            CBUFFER_START(UnityPerMaterial)
                half4 _HorizonColor;
                half4 _ZenithColor;
            CBUFFER_END

            // Köşe girdileri ve piksel verisi
            struct Attributes {float4 positionOS:POSITION;};
            struct Varyings {float4 positionCS:SV_POSITION;float3 direction:TEXCOORD0;};
            Varyings Vert(Attributes v){Varyings o;o.positionCS=TransformObjectToHClip(v.positionOS.xyz);o.direction=v.positionOS.xyz;return o;}

            // Gökyüzü rengini hesaplama
            half4 Frag(Varyings i):SV_Target
            {
                float3 direction=normalize(i.direction);
                half t=pow(saturate(direction.y),.65);
                half3 color=lerp(_HorizonColor.rgb,_ZenithColor.rgb,t);
                half disc=smoothstep(.9995,.99985,dot(direction,GetMainLight().direction));
                return half4(lerp(color,half3(1,.96,.82),disc),1);
            }
            ENDHLSL
        }
    }
}
