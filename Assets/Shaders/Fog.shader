Shader "Zhirnov/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _ColorNoise ("ColorNoise", Color) = (1,1,1,1)
        _FogPower ("Fog Power", Range(0, 1)) = 1
        _DepthStart ("Depth Start", float) = 1
        _DepthDistance ("Depth Distance", float) = 1
        _NoiseFrequency ("Noise Frequency", Range(0, 10)) = 1
        _NoisePower ("Noise Power", Range(0, 1)) = 1
        _NoiseSpeed ("Noise Speed", Vector) = (0,0,0)
        _RoundEdge ("_Round Edge", Range(0, 10)) = 0
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

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float4 _Color;
            float4 _ColorNoise;
            float3 _NoiseSpeed;
            float _DepthStart;
            float _DepthDistance;
            float _NoiseFrequency;
            float _NoisePower;
            float _FogPower;
            float _RoundEdge;
            
            float getNoise(float3 noiseCoord){
                return saturate(clamp(snoise( ( noiseCoord.xyz ) * _NoiseFrequency ), -1, 1) + 1 / 2);
		    }
            
            float4 slerp(float4 p0, float4 p1, float t)
            {
              float dotp = dot(normalize(p0), normalize(p1));
              if ((dotp > 0.9999) || (dotp<-0.9999))
              {
                if (t<=0.5)
                  return p0;
                return p1;
              }
              float theta = acos(dotp * 3.14159/180.0);
              float4 P = ((p0*sin((1-t)*theta) + p1*sin(t*theta)) / sin(theta));
              P.w = 1;
              return P;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                float4 projCoord = UNITY_PROJ_COORD(i.scrPos);
                float depth = Linear01Depth(tex2Dproj(_CameraDepthTexture, projCoord).r) * _ProjectionParams.z;
                depth = depth + ( (0.5 - projCoord.x) * (0.5 - projCoord.x) * 2 * _RoundEdge);
                depth = saturate((depth - _DepthStart)/_DepthDistance);

                float noise = getNoise(float3(float3(i.scrPos.xy, depth) + _NoiseSpeed * _Time.x)) * _NoisePower;
                
                float4 fogCol = lerp(_Color, _ColorNoise, noise);
                float nA = fogCol.a;
                //fogCol *= depth;

                float4 col = tex2Dproj(_MainTex, i.scrPos);
                return lerp(col, fogCol, saturate((depth + noise) * _FogPower * nA) );
                //return fixed4( UNITY_PROJ_COORD(i.scrPos).xyz, 1);//
            }
            ENDCG
        }
    }
}
