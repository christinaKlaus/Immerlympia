// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Immerlympia/XRay"
{
	Properties
	{
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
		[IntRange] _RefValue("Ref Value", Range(0,5)) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 6 //default NotEqual
		// [Enum(UnityEngine.Rendering.StencilOp)] _StencilPassOp ("Stencil Operation", Float) = 2 
		// [Enum(UnityEngine.Rendering.StencilOp)] _StencilFailOp ("Stencil Fail", Float) = 0 
		// [Enum(UnityEngine.Rendering.StencilOp)] _StencilZFailOp ("Stencil ZFail", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Blend", Float) = 1 //default One
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Blend", Float) = 1 //default One
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 8 //default Always
		[Toggle] _ZWrite ("Zwrite", Float) = 0
		
	}

	SubShader
	{
		Stencil
		{
			Ref 0
			Comp NotEqual
		}

		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"XRay" = "ColoredOutline"
		}

		ZWrite Off
		ZTest Always
		Blend One One

		Pass
		{
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

			fixed4 frag (v2f i) : SV_Target
			{
				float NdotV = 1 - dot(i.normal, i.viewDir) * 0.7;
				return _EdgeColor * NdotV;
			}

			ENDCG
		}
	}
}
