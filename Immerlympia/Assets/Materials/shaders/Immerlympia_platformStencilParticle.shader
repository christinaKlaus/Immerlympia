Shader "Immerlympia/platformStencil"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        [IntRange] _stencilRef("Stencil ref", Range(0,255)) = 1
		[IntRange] _stencilWriteMask("Stencil write mask", Range(0,255)) = 1
		[Enum(UnityEngine.Rendering.StencilOp)] _stencilPass("Stencil Pass", Int) = 2
		[Enum(UnityEngine.Rendering.CompareFunction)] _stencilComp("Stencil compare", Int) = 8

    }
    SubShader
    {
        Tags { 
            "RenderQueue" = "Geometry-1"
            "RenderType"="Opaque" }
        LOD 100

        Stencil {
            Ref [_stencilRef]
            WriteMask [_stencilWriteMask]
            Comp [_stencilComp]
            Pass [_stencilPass]
        }

        Blend Zero One
        ZWrite Off

        Pass{
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag


			struct appdata{
				float4 vertex : POSITION;
			};

			struct v2f{
				float4 position : SV_POSITION;
			};

			v2f vert(appdata v){
				v2f o;
				//calculate the position in clip space to render the object
				o.position = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				//Return the color the Object is rendered in
				return 0;
			}

			ENDCG
		}
    }
}
