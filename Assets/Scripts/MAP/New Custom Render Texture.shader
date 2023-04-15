Shader "URP/SolidTransparent"
{
    Properties
    { 
        _MainTex ("Main Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
       
        Tags { "RenderType" = "Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline" }
   
        Pass
        {
            Name  "FrontPass"
            Tags {"LightMode" = "SRPDefaultUnlit"}
            ZWrite On
            ColorMask 0
        }


        Pass
        {
            Name  "TransparentPass"
            Tags {"LightMode" = "UniversalForward"}
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma prefer_hlslcc gles   //  GLES 2.0 호환
            #pragma exclude_renderers d3d11_9x  // dx9.0 호환 제거
            
            #pragma vertex vert
            #pragma fragment frag


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            }; 
            

            sampler2D _MainTex;

            // SRP Batcher
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
            CBUFFER_END


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
               
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 mainTexUV = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                float4 col = tex2D(_MainTex, mainTexUV) * _Color;
 
                return col;
            }
            
            ENDHLSL
        }
    }
}
