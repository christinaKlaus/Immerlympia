// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Immerlympia/platformsTentacles" {
	Properties {
		_Color ("Main Color", Color) = (0.5, 0.5, 0.5, 1)
		[HDR] _Emission("Emission Color", Color) = (0, 0, 0)
		_Saturation("Saturation", Range(0,1)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_RampOffset("Toon Ramp Offset", Range(-1, 1)) = 0
		_ScrollDir("Scroll Direction", Vector) = (0, 0, 0, 0)
		[Header(Posterization)]
		_Posterization ("Posterization", Range(0,1)) = 0
		[IntRange] _PosterizeLevels ("Posterize Levels", Range(1, 16)) = 4
        _Gamma ("Gamma", Range(0,2)) = 0.6
		_addOriginal ("add original color", Range(0,1)) = 0
		[Header(Hologram)]
		_HoloMaster("Holo Master Lerp", Range(0,1)) = 0
		_AlphaStep("Alpha Step", Range(0,1)) = 1
		_AlphaTex("Alpha Texture", 2D) = "white" {}
		_Alpha_ST("Alpha Tiling/Offset", Vector) = (1, 1, 0, 0)
		_HoloOffset("Holo Offset", Range(-1,1)) = -1
		_HoloDir("Holo Direction", Vector) = (0, -1, 0, 0)
		_HoloTex("Holo Texture", 2D) = "white" {}
		_Holo_ST("Holo Tiling/Offset", Vector) = (1, 1, 0, 0)
		_GlitchControl("Glitch int/spd/x/y", Vector) = (0, 0, 1, 0)
		_GlitchRotation("Glitch rotation", Range(0, 360)) = 0
		_RimColor ("Rim Color", Color) = (1, 1, 1, 1)
		_RimPower ("Rim Power", Range(-10, 10)) = 0
		[Header(Tentacle Control)]
		[IntRange] _TentacleMoveDebug("Tentacle movement debug", Range(0, 1)) = 0
		_XMoveStrength("X movement strength", Range(0, 3)) = 0.3
		_ZMoveStrength("Z movement strength", Range(0, 3)) = 0.3
		_MinMoveHeight("Min movement height", Float) = 0.5
		_MaxMoveHeight("Max movement height", Float) = 1
		
	}

	SubShader {
		Tags { 
			"Queue" = "Geometry+1"
			"RenderType" = "Opaque" } // wann wird es gerendert? opaque -> zuerst 
		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
			#pragma surface surf ToonRamp keepalpha vertex:vert fullforwardshadows
			// #pragma shader_feature HOLO_ON
			
			sampler2D _Ramp;
			float _RampOffset;

			// custom lighting function that uses a texture ramp based
			// on angle between light direction and normal

			#pragma lighting ToonRamp exclude_path:prepass

			inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
			{
				#ifndef USING_DIRECTIONAL_LIGHT
				lightDir = normalize(lightDir);
				#endif
	
				half d = dot (s.Normal, lightDir) * 0.5 + 0.5;
				half3 ramp = tex2D (_Ramp, float2(d,d)).rgb + _RampOffset;
	
				half4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
				c.a = s.Alpha;
				return c;
			}

			sampler2D _MainTex;
			float4 _Color;
			float4 _ScrollDir;

			struct Input {
				float2 uv_MainTex : TEXCOORD0;
				float3 worldPos;
				float3 worldNormal;
				float3 viewDir;
				float4 screenPos;
				INTERNAL_DATA
			};

			float4 _GlitchControl;
			float _MaxMoveHeight;
			float _MinMoveHeight;
			float _XMoveStrength;
			float _ZMoveStrength;

			float Remap(float orig, float Mi, float Ma){
				return (abs(orig) - Mi ) / (Ma - Mi);
			}

			void vert (inout appdata_full v, out Input o) {
				float glitch = _GlitchControl.x * (step(0.2, sin(_Time.y * 2.0 + mul(unity_WorldToObject, v.vertex.x) * 1.0)) * step(0.2, sin(frac(_Time.y)*_GlitchControl.y * 0.5)));
				v.vertex.x += glitch * _GlitchControl.z * (-1 + (2 * step(0, frac(_Time.y))));
				v.vertex.y += glitch * _GlitchControl.w * (-1 + (2 * step(0, frac(_Time.y))));

				v.vertex.x += saturate(Remap(v.vertex.y, _MinMoveHeight, _MaxMoveHeight)) * (sin(v.vertex.z) * sin(_Time.z) * _XMoveStrength);
				v.vertex.z += saturate(Remap(v.vertex.y, _MinMoveHeight, _MaxMoveHeight)) * (cos(v.vertex.x) * cos(_Time.z) * _ZMoveStrength);

				// v.vertex.x -= clamp(v.normal.x * glitch * _GlitchControl.z * (-1 + (2 * step(0, frac(_Time.y)))), 0, 100);
				// v.vertex.z -= clamp(v.normal.z * glitch * _GlitchControl.w * (-1 + (2 * step(0, frac(_Time.y)))), 0, 100);

				UNITY_INITIALIZE_OUTPUT(Input, o);
			}

			float _Gamma;
			float _PosterizeLevels;
			float _Posterization;
			float _addOriginal;
			float _Saturation;
			fixed4 _Emission;

			float _AlphaStep;
			float4 _Alpha_ST;
			sampler2D _AlphaTex;
			sampler2D _HoloTex;
			float3 _HoloDir;
			float4 _Holo_ST;
			float _HoloOffset;
			float _HoloMaster;

			float4 _RimColor;
			float _RimPower;

			float _TentacleMoveDebug;

			// float4 _GlitchControl;
			float _GlitchRotation;

			void surf (Input IN, inout SurfaceOutput o) {
				//float2 STUV = TRANSFORM_TEX(IN.uv_MainTex, _MainTex);

				float2 holoTil = float2(_Holo_ST.x, _Holo_ST.y);
				float2 holoOffs = float2(_Holo_ST.z, _Holo_ST.w);

				float2 alphaTil = float2(_Alpha_ST.x, _Alpha_ST.y);
				float2 alphaOffs = float2(_Alpha_ST.z, _Alpha_ST.w);

				half4 c = tex2D(_MainTex, frac(IN.uv_MainTex + _ScrollDir * _Time.y)) * _Color;
				
				float2 screenUV = IN.screenPos.xy / IN.screenPos.w;

				half4 ac = tex2D(_AlphaTex, frac(((screenUV + alphaOffs) * alphaTil)));
				
				// float si = sin ( _GlitchRotation );
                // float co = cos ( _GlitchRotation );
                // float2x2 rotationMatrix = (float2x2( co, -si, si, co) * 0.5) + 0.5;
				// float glitch = _GlitchControl.x * (smoothstep(0.1, 0.15, sin(frac(_Time.y) * 2.0 + screenUV.y)) * smoothstep(0.2, 0.35, sin(frac(_Time.y) * _GlitchControl.y * 0.3)));
				
				// screenUV = mul(screenUV, (rotationMatrix * 2 - 1) * (1 - glitch));
				// screenUV.x += glitch * _GlitchControl.z;
				// screenUV.y += glitch * _GlitchControl.w;

				// makes sure the texture always scrolls at the same speed when the tiling changes
				half4 h = tex2D(_HoloTex, frac(((screenUV + holoOffs) * holoTil) + (_HoloDir * holoTil) * _Time.x));
				

				half4 posterizedColor;
				posterizedColor.rgb = pow(c.rgb, _Gamma) * _PosterizeLevels;
				posterizedColor.rgb = floor(posterizedColor.rgb) / _PosterizeLevels;
				posterizedColor.rgb = pow(posterizedColor.rgb, 1.0 / _Gamma);
				posterizedColor.rgb += c.rgb * _addOriginal;

				half rim = 1.0-saturate(dot(IN.viewDir, IN.worldNormal));
				fixed4 rimColor = _RimColor * pow (rim, _RimPower);

				c.rgb = lerp(c.rgb, posterizedColor.rgb, _Posterization);
				fixed3 lum = saturate(Luminance(c.rgb) + 0.01);
				c.rgb = lerp(lum, c.rgb, _Saturation);

				o.Emission = lerp(_Emission, rimColor + (step(ac + 0.3, _AlphaStep) * rimColor), _HoloMaster);
				o.Albedo = lerp(c.rgb, float4(1,1,1,0) * Remap(IN.worldPos.y, _MinMoveHeight, _MaxMoveHeight), _TentacleMoveDebug);
				o.Alpha = lerp(c.a, c.a * step(1 - _AlphaStep, ac) * (1 - (h.r + _HoloOffset)), _HoloMaster) * step(1 - _AlphaStep, clamp(ac, 0.001, 0.999)); //step(y,x) -> (x >= y) ? 1 : 0
			}

		ENDCG

	} 

	Fallback "Diffuse"
}
