// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FXV/FXVPostprocessAdd" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BlurTex ("Albedo (RGB)", 2D) = "white" {}
		_ColorMultiplier ("ColorMultiplier", Range(0,5)) = 1.0
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 300

		ZWrite Off

		Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;

                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;

                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;
			sampler2D _BlurTex;

            float _ColorMultiplier;

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
                return  tex2D(_MainTex, i.uv) + tex2D(_BlurTex, i.uv) * _ColorMultiplier;
            }
            ENDCG
        }
	}
	FallBack "Diffuse"
}
