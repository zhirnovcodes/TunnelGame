Shader "Zhirnov/LitTunnel"
{
    Properties
    {
        _Tess ("Tessellation", Range(1, 8)) = 1
        _Color ("Color", Color) = (1,1,1,1)
        _BlendColor ("Blend Color", Color) = (1,1,1,1)
        _Up ("Up", Vector) = (0,1,0,0)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _HeightMap ("Height Map", 2D) = "bump" {}
        _MetalicMap ("Metalic Map", 2D) = "bump" {}

        _HeightPower ("Height Power", Range(0.01,5)) = 1
        _NoiseFrequency ("Noise Frequency", Range(0, 10)) = 1
        _FadeEdge ("Fade Edge", Range(0, 0.5)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        
        #include "UnityCG.cginc"

        #pragma surface surf Standard fullforwardshadows vertex:vert tessellate:tess

        #pragma target 3.0
        
        #include "Libs/Bezier.cginc"
        #include "Libs/NoiseSimplex.cginc"

        sampler2D _MainTex;
        sampler2D _NormalMap;
        sampler2D _MetalicMap;
        sampler2D _HeightMap;

        half _Tess;
        half _HeightPower;
        half _Glossiness;
        half _Metallic;
        float _NoiseFrequency;
        float _FadeEdge;
        fixed4 _Color;
        fixed4 _BlendColor;
        float3 _Up;
        float4 _HeightMap_ST;

        float tess()
        {
            return _Tess;
		}

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;
            float4 color : COLOR;
            float2 texcoord    : TEXCOORD0;
            float2 uv    : TEXCOORD1;
        };

        struct MeshData
        {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 tangent : TANGENT;
            float2 texcoord : TEXCOORD0;
            float2 uv : TEXCOORD1;
            float4 color : COLOR;
        };
        
            
        float getNoise(float3 noiseCoord){
            return clamp(snoise( ( noiseCoord.xyz ) * _NoiseFrequency ), -1, 1) + 1 / 2;
		}
        
        float invLerp(float from, float to, float value){
            return (value - from) / (to - from);
        }

        #define PI 3.14159265359
        
        void vert (inout MeshData m)
        {
            ObjectPosNormal vPosN = //_PointsCount >= 4 ? changeObjectPositionNormal(m.vertex, m.normal, m.texcoord.y, normalize(_Up)) : 
                changeObjectPositionNormal3(m.vertex, m.normal, m.texcoord.y, normalize(_Up));

            float4 bP = vPosN.position;
            float3 bN = vPosN.normal;
            float4 bT = vPosN.tangent;


            //float yOffset = tex2Dlod(_HeightMap, float4(0, m.texcoord.y, 0, 0)).r * _HeightPower;
            //heightOffset = float4(bN * yOffset, 0);
            //bP += heightOffset;
            //float4 offset = UnpackNormal( tex2Dlod( _HeightMap, float4(IN.uv_MainTex, 0, 0)) ).r * bN * _HeightPower;
            //bP += offset;
            
            /*
            float4 noColor = float4(0,0,0,1);
            float4 wColor = float4(1,1,1,1);
            if (m.texcoord.y > _RoundEdge && m.texcoord.y < 1 - _RoundEdge)
            {
                m.color = noColor;
			}
            else if (m.texcoord.y <= _RoundEdge)
            {
                m.color = lerp(wColor, noColor, m.texcoord.y/_RoundEdge);
			}
            else if (m.texcoord.y >= 1 - _RoundEdge)
            {
                m.color = lerp(wColor, noColor, (1 - m.texcoord.y)/_RoundEdge);
			}
            */
            float2 newTexcoord = TRANSFORM_TEX(m.texcoord, _HeightMap);
            
            //float3 normal = UnpackNormal( tex2Dlod( _NormalMap, float4( newTexcoord,0,0) ) );
            float cornerSmoothVariant = 0.1;
            float noiseHeight = getNoise( float3(newTexcoord, 0) );
            if ( newTexcoord.x <= cornerSmoothVariant )
            {
                float noiseMax = getNoise( float3(1, newTexcoord.y, 0) );
                float smoothT = invLerp(0, cornerSmoothVariant, newTexcoord.x);
                noiseHeight = lerp(noiseMax, noiseHeight, smoothT);
			}

            //float4 offset = max(tex2Dlod(_HeightMap, float4(newTexcoord,0,0)).r * _HeightPower * noiseHeight, 0) * float4(bN, 0);
            //bP += offset;
            float roundEdgeFade = saturate(invLerp(1 - _FadeEdge, 1, m.texcoord.y));
            roundEdgeFade *= 2;

            m.vertex = bP;
            m.normal = bN;
            m.tangent = bT;
            m.uv = m.texcoord;
            m.color = float4(0,0,0,roundEdgeFade);
		}

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = lerp(tex2D (_MainTex, IN.uv_MainTex) * _Color, _BlendColor, IN.color.a);
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal( tex2D( _NormalMap, IN.uv_MainTex ) );
            o.Metallic = UnpackNormal( tex2D( _MetalicMap, IN.uv_MainTex ) ) * _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
