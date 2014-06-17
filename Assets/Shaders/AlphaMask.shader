Shader "Unlit/AlphaMask" {
	Properties {
		_MainTex ("Base (Alpha only)", 2D) = "white" {}
		_ClipTex ("Clip (RGB)", 2D) = "white" {}
		_xScale ("x Scale", Float) = 1
		_yScale ("y Scale", Float) = 1
		_xOffset ("x Offset", Float) = 0
		_yOffset ("y Offset", Float) = 0
	}
	
	SubShader {
		Tags {
			"Queue" = "Transparent"
		}
		
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _ClipTex;
			float _xScale;
			float _yScale;
			float _xOffset;
			float _yOffset;
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv1 : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};
			
			float4 _MainTex_ST;
			float4 _ClipTex_ST;
			
			v2f vert(appdata_base v) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv1 = TRANSFORM_TEX(v.texcoord, _ClipTex);
				o.uv2 = float2(
					v.texcoord.x * _xScale + _xOffset,
					v.texcoord.y * _yScale + _yOffset
				);
				return o;
			}
			
			half4 frag(v2f i) : COLOR  {
				half4 main = tex2D(_MainTex, i.uv1);
				half4 clip_ = tex2D(_ClipTex, i.uv2);
				clip_.a *= main.a;
				return clip_;
			}
			
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
