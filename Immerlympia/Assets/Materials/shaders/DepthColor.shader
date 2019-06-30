Shader "Unlit/DepthColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _gradientTexture ("Gradient Texture", 2D) = "white" {}
        _minDist ("Minimum Distance", Float) = 0
        _maxDist ("Maximum Distance", Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float _maxDist, _minDist;
            sampler2D _CameraDepthTexture;

            float Remap (float value, float from1, float to1, float from2, float to2) {
                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }

            // OldRange = (OldMax - OldMin)  
            // NewRange = (NewMax - NewMin)  
            // NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin

            sampler2D _gradientTexture;

            fixed4 frag (v2f i) : SV_Target
            {
                // // sample the texture
                // fixed4 col = tex2D(_CameraDepthTexture, i.uv);
                // // apply fog
                // return col;

                float oldRange = _maxDist - _minDist;
                float newRange = 1;
                float newValue = (((distance(i.worldPos, _WorldSpaceCameraPos) - _minDist) * newRange) / oldRange) + 0;


                return tex2D(_gradientTexture, float2(newValue, 0));
                // return fixed4(lerp(fixed4(0,0,0,1), fixed4(1,1,1,1), newValue));
            }
            ENDCG
        }
    }
}
