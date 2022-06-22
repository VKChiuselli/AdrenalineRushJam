// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Texture3D Preview" {
	Properties{
		_Volume("Texture", 3D) = "" {}
		_Period("Time", float) = 1
	}
		SubShader{
			Tags { "Queue" = "Transparent" "RenderType" = "Opaque" }
			Pass {
				Cull Off Lighting Off
				ZWrite Off
				Blend SrcAlpha One
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma exclude_renderers flash gles
				#include "UnityCG.cginc"
				float _Period;

				sampler3D _Volume;
				float4 _Volume_ST;
				struct vs_input {
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};
				struct ps_input {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};
				ps_input vert(vs_input v)
				{
					ps_input o;
					o.uv = TRANSFORM_TEX(v.texcoord, _Volume);
					o.pos = UnityObjectToClipPos(v.vertex);
					return o;
				}
				float4 frag(ps_input i) : COLOR
				{
					float t = frac(_Time.y / _Period);
					float3 uvw = float3(i.uv.x, i.uv.y, t);
					half4 color = tex3D(_Volume, uvw);
					return color;
				}
				ENDCG
			}
		}
			Fallback "VertexLit"
}