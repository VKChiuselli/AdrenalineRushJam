Shader "Custom/Dissolve" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _SliceGuide("Slice Guide (RGB)", 2D) = "white" {}
        _Amount("Amount", Range(0,1)) = 0
		_EmissionMap("Emission", 2D) = "white" {}

    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off
        CGPROGRAM
        #pragma surface surf Lambert addshadow
        #pragma target 3.0
 
        fixed4 _Color;
        sampler2D _MainTex;
        sampler2D _SliceGuide;
		sampler2D _EmissionMap;


        float _Amount;
        float _EmissionAmount;
 
        struct Input {
            float2 uv_MainTex;
        };
 
 
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 e = tex2D(_EmissionMap, IN.uv_MainTex) ;



            half test = tex2D(_SliceGuide, IN.uv_MainTex).rgb - _Amount;
            clip(test);
 
			o.Emission = e.rgb;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}