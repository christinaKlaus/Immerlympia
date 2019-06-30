// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ZBuffer"
{
    Properties
    {
        _gradientTexture ("Gradient Texture", 2D) = "white" {}
        _minDist ("Minimum Distance", Float) = 0
        _maxDist ("Maximum Distance", Float) = 10
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenuv : TEXCOORD1;
            };
           
            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenuv = ComputeScreenPos(o.pos);
                return o;
            }
           
            sampler2D _CameraDepthTexture, _gradientTexture;
            float _minDist, _maxDist;
 
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenuv.xy / i.screenuv.w;
                float depth = 1 - Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));

                float oldRange = _maxDist - _minDist;
                float newRange = 1;
                float newValue = (((depth - _minDist) * newRange) / oldRange) + 0;
                
                return tex2D(_gradientTexture, float2(1 - newValue, 0));
            }
            ENDCG
        }
    }
}