Shader "Custom/Hologram" {
	Properties {
		_Color ("Color", Color) = (0.7, 1, 1, 0) 
		_MainTex ("MainTex", 2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_Emission ("Emission", Range(0.0,10.0)) = 0.5
//		_FallOff("FallOff", Range(0.0,10.0)) = 5.0
//		_FallOff2("FallOff2", Range(0.0,10.0)) = 5.0		
//		_MeshOffset("MeshOffset", Float) = 0.1  
		_CamDistance("CamDistance", Float) = 0.0                
	}
     
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 2000
		Cull Off
       	
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha
		#pragma target 3.0
		
		float4 _MainTex_ST;
		sampler2D _MainTex;
		sampler2D _NormalMap;	
		float4 _Color; 
		half _Emission; 
//		half _FallOff; 
//		half _FallOff2; 
//		half _MeshOffset; 
		float _CamDistance;                      
          
       	struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
//			float3 viewDir;
			float4 screenPos;
		};
		       
		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
//			v.vertex.xyz += v.normal*_MeshOffset;
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			float2 uv_n = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));			
			float2 screenUV = (IN.screenPos.xy / IN.screenPos.w)*_MainTex_ST.xy*_CamDistance;							
          	float3 c = tex2D (_MainTex, screenUV+_MainTex_ST.zw+uv_n);			
			
//			half falloff = saturate(dot(o.Normal, normalize(IN.viewDir)));
//			falloff = saturate(pow((1.0f - falloff),_FallOff)*pow(falloff,_FallOff2));
		
			o.Albedo = c.rgb * _Color.rgb;
			o.Emission = c.rgb * _Color.rgb * _Emission; 
	        o.Alpha = _Color.a * c.r;//* falloff;
		}		
		ENDCG
     } 
     Fallback "Transparent/Diffuse"
 }