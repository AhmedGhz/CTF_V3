// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "mobile"
{
	Properties
	{
		_City_color_map("City_color_map", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _City_color_map;
		uniform float4 _City_color_map_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_City_color_map = i.uv_texcoord * _City_color_map_ST.xy + _City_color_map_ST.zw;
			o.Albedo = tex2D( _City_color_map, uv_City_color_map ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
380;158;1365;796;757.6196;100.2136;1.150063;True;True
Node;AmplifyShaderEditor.CommentaryNode;1;-1598.232,240.3478;Inherit;False;1047.541;403.52;Scale depth from start to end;8;13;11;10;9;8;7;6;5;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2;-1585.041,695.4626;Inherit;False;297.1897;243;Correction for near plane clipping;1;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ProjectionParams;4;-1512.341,744.7626;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-1559.256,523.0427;Float;False;Property;_StartDitheringFade;Start Dithering Fade;1;0;Create;True;0;0;0;False;0;False;0;-2.41;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-1273.765,526.5748;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1092.792,484.0474;Float;False;Property;_EndDitheringFade;End Dithering Fade;2;0;Create;True;0;0;0;False;0;False;1;4.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SurfaceDepthNode;8;-1532.673,338.4599;Inherit;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;9;-1082.377,402.6942;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;-873.2921,343.148;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;11;-875.9918,497.9472;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DitheringNode;12;-495.256,484.9526;Inherit;False;1;False;4;0;FLOAT;0;False;1;SAMPLER2D;;False;2;FLOAT4;0,0,0,0;False;3;SAMPLERSTATE;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-689.4919,399.6475;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;3;-239.2561,404.9528;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;14;-358.5286,-37.93787;Inherit;True;Property;_City_color_map;City_color_map;0;0;Create;True;0;0;0;False;0;False;-1;1c12f6025daf9df4ea9e1f46933c3cda;1c12f6025daf9df4ea9e1f46933c3cda;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;138,-46;Float;False;True;-1;4;ASEMaterialInspector;0;0;Standard;mobile;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;5;0
WireConnection;6;1;4;2
WireConnection;9;0;6;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;11;0;7;0
WireConnection;11;1;6;0
WireConnection;13;0;10;0
WireConnection;13;1;11;0
WireConnection;3;0;13;0
WireConnection;3;1;12;0
WireConnection;0;0;14;0
WireConnection;0;10;3;0
ASEEND*/
//CHKSM=158FA3C054D5CF88162F1DE77F4AFB2CD3F08A17