/*
A shader written for UI layer linear healthbar with a linear color gradient.
*/
Shader "Custom/RadialHealthBar"
{
	Properties
	{
		_MainTex("MainTexture", 2D) = "white" {}
		_Color1("Color 1", Color) = (1,0,0,1)
		_Color2("Color 2", Color) = (0,1,0,1)
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100
		ColorMask RGB
		Cull Back
		Pass
		{
		CGPROGRAM
			#pragma shader_feature _SCAN_ON
			#pragma shader_feature _GLOW_ON
			#pragma shader_feature _GLITCH_ON
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color1;
			float4 _Color2;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}


			fixed4 frag(v2f i) : SV_Target
			{
				float x = i.uv.x;
				float4 baseColor = lerp(_Color1, _Color2, x);
				fixed4 texColor = tex2D(_MainTex, i.uv);
				fixed4 col = texColor * baseColor;
				return col;
			}
		ENDCG
		}
	}
}