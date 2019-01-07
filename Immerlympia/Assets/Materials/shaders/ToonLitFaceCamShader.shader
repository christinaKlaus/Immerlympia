Shader "Immerlympia/FaceCam" {
    Properties {
        _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        [IntRange] _PosterizeLevels ("Posterize Levels", Range(1,16)) = 4
        _Gamma ("Gamma", Float) = 0.6

    }
 
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        CGPROGRAM
        #pragma surface surf ToonRamp
        
        sampler2D _Ramp;
        
        // custom lighting function that uses a texture ramp based
        // on angle between light direction and normal
        #pragma lighting ToonRamp exclude_path:prepass
        #include "UnityCG.cginc"

        inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
        {
            fixed4 c;
            c.rgb = s.Albedo; 
            c.a = s.Alpha;
            return c;
        }
        
        sampler2D _MainTex;
        float4 _Color;
        
        struct Input {
            float2 uv_MainTex : TEXCOORD0;
        };

        float _PosterizeLevels;
        float _Gamma;

        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            c.rgb = pow(c.rgb, 0.4) * 6;
            c.rgb = floor(c.rgb) / 6;
            c.rgb = pow(c.rgb, 1.0 / 0.4);

            // c.rgb = pow(c.rgb, _Gamma);
            // c.rgb = c.rgb * _PosterizeLevels;
            // c.rgb = floor(c.rgb);
            // c.rgb = c.rgb / _PosterizeLevels;
            // c.rgb = pow(c.rgb, 1.0 / _Gamma);

            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
 
    }
 
    Fallback "Diffuse"
}