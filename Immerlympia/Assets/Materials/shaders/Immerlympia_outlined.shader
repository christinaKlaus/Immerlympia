Shader "Immerlympia/outlined" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		[Header(xRay)]
		_EdgeColor("XRay Edge Color", Color) = (0,0,0,0)
		[Range(0,1)] _EdgeOffset("XRay Edge Offset", Vector) = (0,1,0,0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_EmissionTint ("Emission Tint", Color) = (1,1,1,1)
		_EmissionIntensity("Emission intensity", Float) = 1
		_EmissionTex("Emission (RGB)", 2D) = "black" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		_RampOffset("Toon Ramp Offset", Range(-1, 1)) = 0
		[Header(Outline)]
		_OutlineWidth ("Outline Width", Range(0, 0.25)) = 0.1
		_OutlineColor ("Outline Color", Color) = (0,0,0,0)
	}

	SubShader {

		Pass // XRay pass
		{
			Tags {
				"Queue" = "Opaque"
				"RenderType" = "Opaque"
			}

			ZTest GEqual
			ZWrite Off
			Blend One Zero

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				return o;
			}

			float4 _EdgeColor;
			float4 _EdgeOffset;

			float4 frag (v2f i) : SV_Target
			{
				//step(y,x) -> (x >= y) ? 1 : 0
				float NdotV = 1 - dot(i.normal, i.viewDir) * 0.7;
				float smoothEdge = smoothstep(_EdgeOffset.x, _EdgeOffset.y, NdotV);
				fixed4 color = _EdgeColor * step(smoothEdge, _EdgeOffset.z); 
				// fixed4 color = _EdgeColor * smoothstep(_EdgeOffset.x, _EdgeOffset.y, NdotV);

				return color;
			}

			ENDCG
		}

		Pass //Render outline
		{
			Tags {
				"Queue" = "Transparent"
				"RenderType" = "Transparent"
			}

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
		
		Tags {
				"Queue" = "Geometry-1"
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

			float _EmissionIntensity;
			sampler2D _MainTex;
			float4 _EmissionTint, _Color;
			sampler2D _EmissionTex;

			struct Input {
				float2 uv_MainTex : TEXCOORD0;
			};


			void surf (Input IN, inout SurfaceOutput o) {
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Emission = _EmissionTint * tex2D(_EmissionTex, IN.uv_MainTex) * _EmissionIntensity;
			}

		ENDCG
		
	} 

	Fallback "Diffuse"
}
