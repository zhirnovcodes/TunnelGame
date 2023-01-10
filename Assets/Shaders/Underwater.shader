Shader "Zhirnov/Underwater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseSpeed ("Noise Speed", Vector) = (0,0,0)
        _Offset ("Offset", Float) = 0.005
        _NoiseScale ("Noise Scale", Range(0, 10)) = 1
        _NoiseFrequency ("Noise Frequency", Range(0, 10)) = 1
        _DepthStart ("Depth Start", float) = 1
        _DepthDistance ("Depth Distance", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Libs/NoiseSimplex.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 scrPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float _Offset;
            float3 _NoiseSpeed;
            float _NoiseScale;
            float _NoiseFrequency;
            float _DepthStart;
            float _DepthDistance;

            #define PI 3.14159265359

            fixed4 frag (v2f i) : COLOR
            {
                float depth = Linear01Depth(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r) * _ProjectionParams.z;
                depth = saturate((depth - _DepthStart)/_DepthDistance);

                float3 scrPos = float3(i.scrPos.xy, 0) * _NoiseFrequency;
                //scrPos.z += _Time.x * _NoiseSpeed;
                float3 nPos = scrPos + _Time.x * _NoiseSpeed;
                float noise = _NoiseScale * (snoise(nPos) + 1) / 2;
                float noisePi = noise * PI * 2;
                float4 noiseToDir = normalize(float4(cos(noisePi), sin(noisePi), 0, 0 ));

                fixed4 col = tex2D(_MainTex, float4((i.scrPos + noiseToDir * _Offset * depth).xyz, 0));

                return col;
                //return fixed4(noise,noise,noise,1);
            }
            ENDCG
        }
    }
}
