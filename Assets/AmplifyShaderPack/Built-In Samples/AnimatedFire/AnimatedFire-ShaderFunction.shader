// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/Built-in/AnimatedFire"
{
	Properties
	{
		_FireTexture("Fire Texture", 2D) = "white" {}
		_BurnMask("Burn Mask", 2D) = "white" {}
		_FireIntensity("Fire Intensity", Float) = 2
		_Albedo("Albedo", 2D) = "white" {}
		_Normals("Normals", 2D) = "bump" {}
		_Specular("Specular", 2D) = "white" {}
		_Smoothness("Smoothness", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normals;
		uniform sampler2D _Albedo;
		uniform sampler2D _BurnMask;
		uniform float4 _BurnMask_ST;
		uniform sampler2D _FireTexture;
		uniform float _FireIntensity;
		uniform sampler2D _Specular;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			o.Normal = UnpackNormal( tex2D( _Normals, i.uv_texcoord ) );
			o.Albedo = tex2D( _Albedo, i.uv_texcoord ).rgb;
			float2 uv_BurnMask = i.uv_texcoord * _BurnMask_ST.xy + _BurnMask_ST.zw;
			float2 panner2_g3 = ( _Time.x * float2( -1,0 ) + i.uv_texcoord);
			o.Emission = ( ( tex2D( _BurnMask, uv_BurnMask ) * tex2D( _FireTexture, panner2_g3 ) ) * ( _FireIntensity * ( _SinTime.w + 1.5 ) ) ).rgb;
			o.Specular = tex2D( _Specular, i.uv_texcoord ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"

}
/*ASEBEGIN
Version=18901
1237;73;682;926;1247.175;354.2552;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1413.839,183.3731;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-855.2775,119.1449;Float;False;Property;_FireIntensity;Fire Intensity;3;0;Create;True;0;0;0;False;0;False;2;10.76;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;13;-535.8399,-92.72685;Inherit;True;Property;_Normals;Normals;5;0;Create;True;0;0;0;False;0;False;-1;None;abfd39fa1d6a42ba9b322e4301333932;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-535.7401,-292.4268;Inherit;True;Property;_Albedo;Albedo;4;0;Create;True;0;0;0;False;0;False;-1;None;85e3723e62d44f758723754190c67911;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-9.840205,290.5776;Float;False;Property;_Smoothness;Smoothness;7;0;Create;True;0;0;0;False;0;False;1;0.64;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-520.8406,351.0728;Inherit;True;Property;_Specular;Specular;6;0;Create;True;0;0;0;False;0;False;-1;None;f4ef9b0dca354d7aaf185497d6b77ea5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;22;-542.5759,156.6447;Inherit;False;Burn Effect;0;;3;05e3549071b9ccc4295de9c50416327f;0;2;12;FLOAT;0;False;14;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;291.1997,-174.2;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;AmplifyShaderPack/Built-in/AnimatedFire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;1;4;0
WireConnection;17;1;4;0
WireConnection;16;1;4;0
WireConnection;22;12;19;0
WireConnection;22;14;4;0
WireConnection;0;0;17;0
WireConnection;0;1;13;0
WireConnection;0;2;22;0
WireConnection;0;3;16;0
WireConnection;0;4;15;0
ASEEND*/
//CHKSM=3AAC5237B0DC85695FE58DD898D34B8D101F6D7E