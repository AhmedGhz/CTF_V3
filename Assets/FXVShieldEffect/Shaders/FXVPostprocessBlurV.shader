// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FXV/FXVPostprocessBlurV" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 300
	
		Blend Off
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

            uniform float4 _MainTex_TexelSize;

            sampler2D _MainTex;

            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f i) : SV_Target
            {
            	fixed4 finalColor = fixed4(0.0, 0.0, 0.0, 0.0);

				float texelSize = _MainTex_TexelSize.y;
				
				finalColor += tex2D(_MainTex, i.uv)*0.227027;

				finalColor += tex2D(_MainTex, i.uv+float2(0.0,1.0)*texelSize)*0.1945946;
				finalColor += tex2D(_MainTex, i.uv-float2(0.0,1.0)*texelSize)*0.1945946;

				finalColor += tex2D(_MainTex, i.uv+float2(0.0,2.0)*texelSize)*0.1216216;
				finalColor += tex2D(_MainTex, i.uv-float2(0.0,2.0)*texelSize)*0.1216216;

				finalColor += tex2D(_MainTex, i.uv+float2(0.0,3.0)*texelSize)*0.054054;
				finalColor += tex2D(_MainTex, i.uv-float2(0.0,3.0)*texelSize)*0.054054;

				finalColor += tex2D(_MainTex, i.uv+float2(0.0,4.0)*texelSize)*0.016216;
				finalColor += tex2D(_MainTex, i.uv-float2(0.0,4.0)*texelSize)*0.016216;

                return finalColor;
            }
            ENDCG
        }
	}
	FallBack "Diffuse"
}
