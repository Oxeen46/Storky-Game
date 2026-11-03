Shader "Storky/Stylized Ocean"
{
    Properties
    {
        _BaseColor("Sea", Color) = (0.07,0.30,0.40,1)
        _CrestColor("Ripples", Color) = (0.17,0.43,0.50,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }
        Pass
        {
            Name "Ocean"
            Tags { "LightMode"="UniversalForward" }
            ZWrite On
            Cull Back
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _CrestColor;
            CBUFFER_END
            struct Attributes { float4 positionOS:POSITION; };
            struct Varyings { float4 positionCS:SV_POSITION; float3 positionWS:TEXCOORD0; half fog:TEXCOORD1; };
            Varyings Vert(Attributes input)
            {
                Varyings o;
                o.positionWS=TransformObjectToWorld(input.positionOS.xyz);
                o.positionCS=TransformWorldToHClip(o.positionWS);
                o.fog=ComputeFogFactor(o.positionCS.z);
                return o;
            }
            half4 Frag(Varyings input):SV_Target
            {
                float2 p=input.positionWS.xz;
                float t=_Time.y;
                half a=sin(p.x*.33+p.y*.19+t*.75);
                half b=sin(p.x*-.17+p.y*.41-t*.55);
                float3 view=GetWorldSpaceNormalizeViewDir(input.positionWS);
                float distanceToEye=distance(_WorldSpaceCameraPos,input.positionWS);
                half detail=1-smoothstep(80,430,distanceToEye);
                half3 normal=normalize(half3((a+b)*.09*detail,1,(a-b)*.07*detail));
                half fresnel=pow(1-saturate(dot(normal,view)),4);
                Light sun=GetMainLight();
                half spec=pow(saturate(dot(normal,normalize(sun.direction+view))),80)*.22*detail;
                half3 color=lerp(_BaseColor.rgb,_CrestColor.rgb,(a*b*.10+.12)*detail+fresnel*.40);
                color+=spec*sun.color;
                // Evaluate fog per pixel: four distant quad vertices would fog the entire sea.
                half fog=ComputeFogFactor(TransformWorldToHClip(input.positionWS).z);
                return half4(MixFog(color,fog),1);
            }
            ENDHLSL
        }
    }
}
