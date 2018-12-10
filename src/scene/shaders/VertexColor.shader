/*
A simple vertex shader that allows vertex coloring.
*/
Shader "Custom/VertexColor" {
	SubShader
	{
		Pass
		{
			Cull Off
		}
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
			#pragma surface surf Lambert vertex:vert
			#pragma target 3.0

			struct Input
			{
				float4 vertColor;
			};

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.vertColor = v.color;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				o.Albedo = vec4(0, 0, 0, 0);
			}
		ENDCG
	}
	FallBack "Diffuse"
}