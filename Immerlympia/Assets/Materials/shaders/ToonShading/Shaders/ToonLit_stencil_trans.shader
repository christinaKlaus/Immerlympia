Shader "Toon/Lit_stencil_transparent" {
	Properties {
		
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TransOffset ("Transparency Offset", Range(-1,1)) = 0
		_TransTex("Transparency (Greyscale)", 2D) = "white" {}
		_EmitTex("Emission (RGB)", 2D) = "black" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_ScrollDir("Scroll Direction", Vector) = (0,0,0,0)

	}

	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True"  "RenderType" = "Transparent"} // wann wird es gerendert? opaque -> zuerst 
		LOD 200

		// Pass {
		// 	ZWrite On
		// 	ColorMask 0
		// }
		
		// Stencil
		// {
		// 	Ref 1
		// 	Comp Always
		// 	Pass Replace
		// 	ZFail Keep
		// }

		Pass {
			Tags {
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}

			ZWrite Off
        	Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#include "UnityCG.cginc"
            #pragma vertex vert
			#pragma fragment frag

			sampler2D _TransTex;
			sampler2D _MainTex;
            float4 _TransTex_ST;
			float _TransOffset;

			struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
         
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _TransTex);
                return o;
            }

			fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				col.a = tex2D(_TransTex, i.uv) + _TransOffset;
                return col;
            }

			ENDCG
		}

		CGPROGRAM
			#pragma target 3.0
			#pragma surface surf Lambert alpha

			sampler2D _Ramp;
			
			// custom lighting function that uses a texture ramp based
			// on angle between light direction and normal
			

			#pragma lighting ToonRamp exclude_path:prepass

			inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
			{
				#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
				#endif
	
				half d = dot (s.Normal, lightDir) * 0.5 + 0.5;
				half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
				c.a = 0;
				return c;
			}

			sampler2D _EmitTex;
			sampler2D _TransTex;
			float4 _Color;
			float4 _ScrollDir;
			float _TransOffset;
			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex : TEXCOORD0;
				fixed facing : VFACE;
				INTERNAL_DATA
			};

			void surf (Input IN, inout SurfaceOutput o) {
				//IN.uv_MainTex = TRANSFORM_TEX(IN.uv_MainTex, _MainTex);
				IN.uv_MainTex += _ScrollDir * _Time.y;
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				half4 alpha = tex2D(_TransTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				o.Alpha = alpha.r;
				o.Emission = tex2D(_EmitTex, IN.uv_MainTex).rgb;

				WorldNormalVector(IN, o.Normal);
				o.Normal *= 1 - (step(IN.facing, 0.5) * 2);
			}

		ENDCG

	} 

	Fallback "Diffuse"
}
