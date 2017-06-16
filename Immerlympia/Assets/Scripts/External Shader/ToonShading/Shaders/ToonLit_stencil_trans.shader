Shader "Toon/Lit_stencil_transparent" {
	Properties {
		
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_TransTex("Transparency (Greyscale)", 2D) = "white" {}
		_EmitTex("Emission (RGB)", 2D) = "black" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_ScrollDir("Scroll Direction", Vector) = (0,0,0,0)

	}

	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True"  "RenderType" = "Transparent"} // wann wird es gerendert? opaque -> zuerst 
		LOD 200

		Pass {
			ZWrite On
			ColorMask 0
		}
		
		Stencil
		{
			Ref 1
			Comp Always
			Pass Replace
			ZFail Keep
		}


		CGPROGRAM
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


			sampler2D _MainTex;
			sampler2D _EmitTex;
			sampler2D _TransTex;
			float4 _Color;
			float4 _ScrollDir;

			struct Input {
				float2 uv_MainTex : TEXCOORD0;
			};

			void surf (Input IN, inout SurfaceOutput o) {
				IN.uv_MainTex += _ScrollDir * _Time.y;
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				half4 alpha = tex2D(_TransTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				o.Alpha = alpha.r;
				o.Emission = tex2D(_EmitTex, IN.uv_MainTex).rgb;
			}

		ENDCG

	} 

	Fallback "Diffuse"
}
