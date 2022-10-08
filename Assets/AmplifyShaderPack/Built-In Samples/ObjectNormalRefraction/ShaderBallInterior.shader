// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/Built-in/ShaderBallInterior"
{
	Properties
	{
		_RubberDiffuse("RubberDiffuse", 2D) = "white" {}
		_RubberSpecular("RubberSpecular", 2D) = "white" {}
		_RubberNormal("RubberNormal", 2D) = "bump" {}
		_Smoothness("Smoothness", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			half ASEVFace : VFACE;
		};

		uniform sampler2D _RubberNormal;
		uniform float4 _RubberNormal_ST;
		uniform sampler2D _RubberDiffuse;
		uniform float4 _RubberDiffuse_ST;
		uniform sampler2D _RubberSpecular;
		uniform float4 _RubberSpecular_ST;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_RubberNormal = i.uv_texcoord * _RubberNormal_ST.xy + _RubberNormal_ST.zw;
			float3 tex2DNode3 = UnpackNormal( tex2D( _RubberNormal, uv_RubberNormal ) );
			float3 appendResult9 = (float3(tex2DNode3.xy , ( tex2DNode3.b * i.ASEVFace )));
			o.Normal = appendResult9;
			float2 uv_RubberDiffuse = i.uv_texcoord * _RubberDiffuse_ST.xy + _RubberDiffuse_ST.zw;
			o.Albedo = tex2D( _RubberDiffuse, uv_RubberDiffuse ).rgb;
			float2 uv_RubberSpecular = i.uv_texcoord * _RubberSpecular_ST.xy + _RubberSpecular_ST.zw;
			o.Specular = tex2D( _RubberSpecular, uv_RubberSpecular ).rgb;
			o.Smoothness = _Smoothness;
			o.Occlusion = 0.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"

}
/*ASEBEGIN
Version=18901
1252;73;667;926;1408.602;793.7581;1.8591;True;False
Node;AmplifyShaderEditor.SamplerNode;3;-1122.051,-70.15266;Inherit;True;Property;_RubberNormal;RubberNormal;2;0;Create;True;0;0;0;False;0;False;-1;066f29fd0fc3d0341b96857dcf2cede3;cd843373d1f443fc9d46351a814d44cf;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FaceVariableNode;7;-800,64;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-640,0;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-480,-64;Inherit;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;2;-640,128;Inherit;True;Property;_RubberSpecular;RubberSpecular;1;0;Create;True;0;0;0;False;0;False;-1;0688235f1fee46b4581bcc1cf189cf3a;664833c525b1432b8e496452d1027b33;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-528,320;Float;False;Property;_Smoothness;Smoothness;3;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-496,400;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-640,-256;Inherit;True;Property;_RubberDiffuse;RubberDiffuse;0;0;Create;True;0;0;0;False;0;False;-1;ff9da6b9c3ae4e8439ce851c4452c3a0;0f2b32c905e3472db19df9a0a6fb3413;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-192,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;AmplifyShaderPack/Built-in/ShaderBallInterior;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;3;3
WireConnection;8;1;7;0
WireConnection;9;0;3;0
WireConnection;9;2;8;0
WireConnection;0;0;1;0
WireConnection;0;1;9;0
WireConnection;0;3;2;0
WireConnection;0;4;4;0
WireConnection;0;5;6;0
ASEEND*/
//CHKSM=BB1E2C382AA7769D6C2A3EA1CA269C6C2D03ABA3