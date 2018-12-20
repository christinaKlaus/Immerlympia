// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "radialCountdown"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		[IntRange]_maxGameTime("maxGameTime", Range( 0 , 60)) = 60
		[IntRange]_currentGameTime("currentGameTime", Range( 0 , 60)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform half _maxGameTime;
		uniform half _currentGameTime;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float4 tex2DNode2 = tex2D( _Texture0, uv_Texture0 );
			float2 temp_output_20_0 = (float2( -1,-1 ) + (i.uv_texcoord - float2( 0,0 )) * (float2( 1,1 ) - float2( -1,-1 )) / (float2( 1,1 ) - float2( 0,0 )));
			o.Albedo = ( tex2DNode2 * step( atan2( (temp_output_20_0).y , (temp_output_20_0).x ) , (( UNITY_PI * -1.0 ) + (( (( _maxGameTime >= 0.0 && _maxGameTime <= _maxGameTime ) ? _currentGameTime :  _maxGameTime ) / _maxGameTime ) - 0.0) * (UNITY_PI - ( UNITY_PI * -1.0 )) / (1.0 - 0.0)) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16100
2047;26;1567;918;587.8378;122.8431;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;4;-1604.678,199.777;Half;False;Property;_maxGameTime;maxGameTime;1;1;[IntRange];Create;True;0;0;False;0;60;0;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-1551.676,586.381;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-1584.916,38.11464;Half;False;Property;_currentGameTime;currentGameTime;2;1;[IntRange];Create;True;0;0;False;0;0;0;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;-1263.699,580.5779;Float;True;5;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT2;1,1;False;3;FLOAT2;-1,-1;False;4;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;35;-1155.774,252.2827;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;32;-841.2089,363.7643;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-627.2356,229.2245;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-618.012,123.0555;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;22;-908.7737,759.985;Float;True;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;23;-911.9736,510.3854;Float;True;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ATan2OpNode;24;-545.574,564.7856;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;31;-422.4136,176.7829;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-4;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-908.3334,-193.7941;Float;True;Property;_Texture0;Texture 0;0;0;Create;True;0;0;False;0;39530c5371118cb419a0e620d61191ce;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.StepOpNode;29;-181.3448,264.8167;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-592.5728,-180.5973;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;45.58121,165.9831;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;44;409.0467,168.8884;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;radialCountdown;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;10;0
WireConnection;35;0;4;0
WireConnection;35;2;4;0
WireConnection;35;3;5;0
WireConnection;35;4;4;0
WireConnection;33;0;32;0
WireConnection;6;0;35;0
WireConnection;6;1;4;0
WireConnection;22;0;20;0
WireConnection;23;0;20;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;31;0;6;0
WireConnection;31;3;33;0
WireConnection;31;4;32;0
WireConnection;29;0;24;0
WireConnection;29;1;31;0
WireConnection;2;0;1;0
WireConnection;27;0;2;0
WireConnection;27;1;29;0
WireConnection;44;0;27;0
ASEEND*/
//CHKSM=49FEC729B2B40A04527A22CCAC8B6847E362FF89