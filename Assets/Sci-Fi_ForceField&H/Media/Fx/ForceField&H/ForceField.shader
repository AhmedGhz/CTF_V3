Shader "Custom/ForceField" {
	Properties {
		_Color ("Color", Color) = (0.7, 1, 1, 0) 
		_MainTex ("MainTex", 2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_Emission ("Emission", Range(0.0,10.0)) = 0.5
		_FallOff("FallOff", Range(0.0,10.0)) = 5.0
		_FallOff2("FallOff2", Range(0.0,10.0)) = 5.0		
		_MeshOffset("MeshOffset", Float) = 0.1
		_BrightnessCollision("BrightnessPointCollision", Range(0.0,1.0)) = 0.5
		_MaxDistance ("SizePointCollision", float) = 1  
		           				
		_Position ("Collision", Vector) = (-1, -1, -1, -1)
		_EffectTime ("Effect Time (ms)", float) = 0		        
	}
     
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 2000
		Cull Off
       	
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha
		#pragma target 3.0
		
		sampler2D _MainTex;
		sampler2D _NormalMap;	
		float4 _Color; 
		float4 _Position;
		float _MaxDistance;
		float _EffectTime; 
		half _Emission; 
		half _FallOff; 
		half _FallOff2; 
		half _MeshOffset;  
		half _BrightnessCollision;                           
          
       	struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float3 viewDir;
			float customDist;
		};
		       
		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			v.vertex.xyz += v.normal*_MeshOffset;
			o.customDist = distance(_Position.xyz, v.vertex.xyz);
		}
		
		void surf (Input IN, inout SurfaceOutput o) {
			float2 uv_n = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));		
			float3 c = tex2D (_MainTex, IN.uv_MainTex+uv_n) * _Color;
			
			half falloff = saturate(dot(o.Normal, normalize(IN.viewDir)));
			falloff = saturate(pow((1.0f - falloff),_FallOff)*pow(falloff,_FallOff2));
		
			half alpha;
			if(_EffectTime > 0){
				if(IN.customDist < _MaxDistance){
					alpha = saturate(_EffectTime - (IN.customDist / _MaxDistance))*_BrightnessCollision + _Color.a * falloff;
					if(alpha < _Color.a * falloff){
						alpha = _Color.a * falloff;
					}
				}
				else{
					alpha = _Color.a * falloff;
				}
			}
			else{
				alpha = _Color.a * falloff;
			}

			o.Albedo = c.rgb;
			o.Emission = c.rgb*_Emission; 
	        o.Alpha = alpha;
		}		
		ENDCG
     } 
     Fallback "Transparent/Diffuse"
 }