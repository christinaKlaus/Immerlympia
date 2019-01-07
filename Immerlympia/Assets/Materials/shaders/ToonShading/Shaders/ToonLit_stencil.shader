Shader "Toon/Lit_stencil" {
	Properties {
		
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_Saturation("Saturation", Range(0,1)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_RampOffset("Toon Ramp Offset", Range(-1, 1)) = 0
		_ScrollDir("Scroll Direction", Vector) = (0,0,0,0)
		_Posterization ("Posterization", Range(0,1)) = 0
		[IntRange] _PosterizeLevels ("Posterize Levels", Range(1,16)) = 4
        _Gamma ("Gamma", Range(0,2)) = 0.6
		_addOriginal ("add original color", Range(0,1)) = 0
	}

	SubShader {
		Tags { 
			"Queue" = "Geometry"
			"RenderType" = "Opaque" } // wann wird es gerendert? opaque -> zuerst 
		LOD 200
		
		// Stencil
		// {
		// 	Ref [_RefValue]
		// 	Comp [_StencilComp]
		// 	Pass [_StencilPassOp]
		// 	ZFail [_StencilZFailOp]
		// }

		// ZWrite On
		// ZTest LEqual

		CGPROGRAM
			#pragma surface surf ToonRamp

			sampler2D _Ramp;

			// custom lighting function that uses a texture ramp based
			// on angle between light direction and normal

			#pragma lighting ToonRamp exclude_path:prepass

			float _RampOffset;

			inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
			{
				#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
				#endif
	
				half d = dot (s.Normal, lightDir) * 0.5 + 0.5;
				half3 ramp = tex2D (_Ramp, float2(d,d)).rgb + _RampOffset;
	
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
				c.a = 0;
				return c;
			}


			sampler2D _MainTex;
			float4 _Color;
			float4 _ScrollDir;

			struct Input {
				float2 uv_MainTex : TEXCOORD0;
			};

			float _Gamma;
			float _PosterizeLevels;
			float _Posterization;
			float _addOriginal;
			float _Saturation;

			void surf (Input IN, inout SurfaceOutput o) {
				IN.uv_MainTex += _ScrollDir * _Time.y;
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

				half4 posterizedColor;
				posterizedColor.rgb = pow(c.rgb, _Gamma) * _PosterizeLevels;
				posterizedColor.rgb = floor(posterizedColor.rgb) / _PosterizeLevels;
				posterizedColor.rgb = pow(posterizedColor.rgb, 1.0 / _Gamma);
				posterizedColor.rgb += c.rgb * _addOriginal;

				c.rgb = lerp(c.rgb, posterizedColor.rgb, _Posterization);
				fixed3 lum = saturate(Luminance(c.rgb) + 0.01);
				o.Albedo = lerp(lum, c.rgb, _Saturation);
				o.Alpha = c.a;
			}

		ENDCG

	} 

	Fallback "Diffuse"
}
