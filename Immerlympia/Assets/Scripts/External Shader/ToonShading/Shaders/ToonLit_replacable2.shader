Shader "Toon/Lit_replacable2" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_EdgeColor("XRay Edge Color", Color) = (0,0,0,0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissionTex("Emission (RGB)", 2D) = "black" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_RampOffset("Toon Ramp Offset", Range(-1, 1)) = 0
		_OutlineWidth ("Outline Width", Range(0, 0.25)) = 0.1
		_OutlineColor ("Outline Color", Color) = (0,0,0,0)
	}

	// CGINCLUDE
	// #include "UnityCG.cginc"

	// struct appdata
	// {
	// 	float4 vertex : POSITION;
	// 	float3 normal : NORMAL;
	// };

	// struct v2f
	// {
	// 	float4 pos : SV_POSITION;
	// 	float3 normal : NORMAL;
	// };

	// float _OutlineWidth;

	// v2f vert(appdata v)
	// {
	// 	// v.vertex.xyz *= _OutlineWidth;
	// 	v2f o;
	// 	v.vertex.w *= _OutlineWidth;
	// 	o.pos = UnityObjectToClipPos(v.vertex);
	// 	o.normal = v.normal;
	// 	return o;
	// }

	// ENDCG

	SubShader {
		Pass //Render outline
		{
			Tags {
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}
			// ZWrite Off
			// CGPROGRAM
			// 	#pragma vertex vert
			// 	#pragma fragment frag

			// 	float4 _OutlineColor;

			// 	half4 frag(v2f i) : COLOR 
			// 	{
			// 		return _OutlineColor;
			// 	}
			// ENDCG
			
			Zwrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f 
			{
				float4 pos : SV_POSITION;
			};

			float _OutlineWidth;
			float4 _OutlineColor;

			float4 vert(appdata_base v) : SV_POSITION
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
				normal.x *= UNITY_MATRIX_P[0][0];
				normal.y *= UNITY_MATRIX_P[1][1];
				o.pos.xy += normal.xy * _OutlineWidth;
				return o.pos;
			}

			half4 frag(v2f i) : COLOR 
			{
				return _OutlineColor;
			}

			ENDCG

		}

		Tags {	"Queue" = "Geometry-1"
				"RenderType" = "Opaque"
				"XRay" = "ColoredOutline" 
		} // wann wird es gerendert? opaque -> zuerst 
		LOD 200
		
		CGPROGRAM
				#pragma surface surf ToonRamp
				#pragma lighting ToonRamp exclude_path:prepass 

				sampler2D _Ramp;

				// custom lighting function that uses a texture ramp based
				// on angle between light direction and normal

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
				sampler2D _EmissionTex;

				struct Input {
					float2 uv_MainTex : TEXCOORD0;
				};


				void surf (Input IN, inout SurfaceOutput o) {
					half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
					o.Albedo = c.rgb;
					o.Alpha = c.a;
					o.Emission = tex2D(_EmissionTex, IN.uv_MainTex) * _Color;
				}

			ENDCG
		
	} 

	Fallback "Diffuse"
}
