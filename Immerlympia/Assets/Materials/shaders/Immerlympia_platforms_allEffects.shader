Shader "Immerlympia/platforms_2" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_Saturation("Saturation", Range(0,1)) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_RampOffset("Toon Ramp Offset", Range(-1, 1)) = 0
		_ScrollDir("Scroll Direction", Vector) = (0,0,0,0)
		[Header(Posterization)]
		_Posterization ("Posterization", Range(0,1)) = 0
		[IntRange] _PosterizeLevels ("Posterize Levels", Range(1,16)) = 4
        _Gamma ("Gamma", Range(0,2)) = 0.6
		_addOriginal ("add original color", Range(0,1)) = 0
		[Header(Disappearing)]
		// _OverrideColor("Override Color", Color) = (0, 0, 0, 0)
		[IntRange] _stencilRef("Stencil ref", Range(0,255)) = 1
		[IntRange] _stencilReadMask("Stencil read mask", Range(0,255)) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _stencilComp("Stencil compare", Int) = 0
		[Header(Dissolve)]
		_Alpha("Alpha", Range(0,1)) = 1
		_NoiseTex("Dissolve Noise", 2D) = "white"{} // Texture the dissolve is based on
        _DisAmount("Dissolve Amount", Range(0, 1)) = 0 // amount of dissolving going on
        _DisLineWidth("Dissolve Width", Range(0, 2)) = 0 // width of the line around the dissolve
        _DisLineColor("Dissolve Color", Color) = (1,1,1,1) // Color of the dissolve Line
		[Header(Hologram)]
		_ScanDirection ("Direction", Vector) = (0,1,0,0)
		_GlowDirection ("Direction", Vector) = (0,1,0,0)
		// Rim
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0.1, 10)) = 5.0
		// Scanline
		_ScanTiling ("Scan Tiling", Range(0.0, 10.0)) = 0.05
		_ScanSpeed ("Scan Speed", Range(-2.0, 2.0)) = 1.0
		// Glitch
		_GlitchSpeed ("Glitch Speed", Range(0, 50)) = 1.0
		_GlitchIntensity ("Glitch Intensity", Float) = 0
		// Glow
		_GlowTiling ("Glow Tiling", Range(0.01, 1.0)) = 0.05
		_GlowSpeed ("Glow Speed", Range(-10.0, 10.0)) = 1.0
		// [Header(xRay)]
		// _OutlineWidth("Outline width", Range(0,20)) = 0
		// _EdgeColor("XRay Edge Color", Color) = (0,0,0,0)
	}

	SubShader {
		
		Tags { 
			"Queue" = "Geometry+1"
			"RenderType" = "Opaque" } // wann wird es gerendert? opaque -> zuerst 
		LOD 200

		Blend SrcAlpha OneMinusSrcAlpha

		Stencil {
			Ref [_stencilRef]
			ReadMask [_stencilReadMask]
			Comp [_stencilComp] // default LEqual
		}

		CGPROGRAM
			#pragma surface surf ToonRamp keepalpha vertex:vert
			#pragma shader_feature _DISSOLVE
			// #pragma shader_feature _GLITCH
			
			// #ifdef _GLITCH
			//#pragma vertex:vert
			// #endif

			sampler2D _Ramp;

			// custom lighting function that uses a texture ramp based
			// on angle between light direction and normal

			#pragma lighting ToonRamp exclude_path:prepass

			float _RampOffset;
			float4 _OverrideColor;

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

			// #ifdef _GLITCH
			
			float _GlitchIntensity;
			float _GlitchSpeed;

			void vert (inout appdata_full v) {
				v.vertex.x += _GlitchIntensity * (step(0.1, sin(_Time.y * 2.0 + v.vertex.y * 1.0)) * step(0.1, sin(_Time.y*_GlitchSpeed * 0.5)));
				v.vertex.z += _GlitchIntensity * (step(0.1, sin(_Time.y * 2.0 + v.vertex.x * 1.0)) * step(0.1, sin(_Time.y*_GlitchSpeed * 0.5)));
			}

			// #endif

			sampler2D _MainTex;
			float4 _Color;
			float4 _ScrollDir;

			struct Input {
				float2 uv_MainTex : TEXCOORD0;
				float3 worldPos;
				float3 worldNormal;
				float3 viewDir;
				float3 screenPos;
			};

			float _Gamma;
			float _PosterizeLevels;
			float _Posterization;
			float _addOriginal;
			float _Saturation;
			float _Alpha;

			// Joyce dissolve
			sampler2D _NoiseTex;
			float _DisAmount;
			float _DisLineWidth;
			float4 _DisLineColor;

			float4 _ScanDirection;
			float4 _GlowDirection;
			// rim color
			float4 _RimColor;
			float _RimPower;
			// scan lines
			float _ScanTiling;
			float _ScanSpeed;
			// glow
			float _GlowTiling;
			float _GlowSpeed;


			void surf (Input IN, inout SurfaceOutput o) {
				IN.uv_MainTex += _ScrollDir * _Time.y;
				half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

				half dirScan = (dot(IN.worldPos, normalize(float4(_ScanDirection.xyz, 1.0))) + 1) / 2;
				half dirGlow = (dot(IN.worldPos, normalize(float4(_GlowDirection.xyz, 1.0))) + 1) / 2;

				half4 posterizedColor;
				posterizedColor.rgb = pow(c.rgb, _Gamma) * _PosterizeLevels;
				posterizedColor.rgb = floor(posterizedColor.rgb) / _PosterizeLevels;
				posterizedColor.rgb = pow(posterizedColor.rgb, 1.0 / _Gamma);
				posterizedColor.rgb += c.rgb * _addOriginal;

				c.rgb = lerp(c.rgb, posterizedColor.rgb, _Posterization);
				fixed3 lum = saturate(Luminance(c.rgb) + 0.01);
				c.rgb = lerp(lum, c.rgb, _Saturation);

				// Scanlines
				float scan = 0.0;
				//#ifdef _SCAN_ON
					scan = step(frac(dirScan * _ScanTiling + _Time.w * _ScanSpeed), 0.5) * 0.65;
					// scan = smoothstep(0.25, 0.75, frac(dirVertex * _ScanTiling + _Time.w * _ScanSpeed));
				//#endif

				// Glow
				float glow = 0.0;
				// #ifdef _GLOW_ON
					glow = frac(dirGlow * _GlowTiling - _Time.x * _GlowSpeed);
				// #endif

				// rim color
				half rim = 1.0-saturate(dot(IN.viewDir, IN.worldNormal));
				fixed4 rimColor = _RimColor * pow (rim, _RimPower);

				half4 n = tex2D(_NoiseTex, IN.worldPos.xy); // turn the noise texture into a value we can compare to. worldPos.xy projects from one side, xz from other side, yz from top
 
				if (n.r - _DisLineWidth < _DisAmount) { //if the noise value minus the width of the line is lower than the dissolve amount
					c = _DisLineColor ; // that part is the dissolve line color
				}
				if (n.r<_DisAmount) { // if the noise value is under the dissolve amount
					c.a = 0.0; // it's transparent, the alpha is set to 0
				}

				o.Albedo = c.rgb + rimColor + (glow * 0.33 * _RimColor);
				o.Alpha = c.a * _Alpha * scan;
			}

		ENDCG

	} 

	Fallback "Diffuse"
}