// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/Built-in/ReadFromAtlasTiled"
{
	Properties
	{
		_Min("Min", Vector) = (0,0,0,0)
		_Max("Max", Vector) = (0,0,0,0)
		_TileSize("TileSize", Vector) = (2,2,0,0)
		_Atlas("Atlas", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Atlas;
		float4 _Atlas_TexelSize;
		uniform float2 _Min;
		uniform float4 _Atlas_ST;
		uniform float2 _TileSize;
		uniform float2 _Max;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult57 = (float2(_Atlas_TexelSize.x , _Atlas_TexelSize.y));
			float2 ScaledMin27 = ( appendResult57 * _Min );
			float2 uv_Atlas = i.uv_texcoord * _Atlas_ST.xy + _Atlas_ST.zw;
			float2 ScaledMax26 = ( _Max * appendResult57 );
			float2 Size39 = ( ScaledMax26 - ScaledMin27 );
			float2 TiledVar55 = ( fmod( uv_Atlas , ( float2( 1,1 ) / _TileSize ) ) * Size39 );
			o.Emission = tex2D( _Atlas, ( ScaledMin27 + ( TiledVar55 * _TileSize ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"

}
/*ASEBEGIN
Version=18901
1245;73;674;926;1629.059;61.94818;1;True;False
Node;AmplifyShaderEditor.TexelSizeNode;19;-1788.193,255.8997;Inherit;False;30;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;14;-1533.996,122.2001;Float;False;Property;_Max;Max;1;0;Create;True;0;0;0;False;0;False;0,0;512,512;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;13;-1506.596,424.2998;Float;False;Property;_Min;Min;0;0;Create;True;0;0;0;False;0;False;0,0;255,255;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DynamicAppendNode;57;-1520.159,284.0518;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1205.595,362.0998;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1223.595,212.6999;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-1034.497,321.9005;Float;False;ScaledMin;-1;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-1040.897,185.3005;Float;False;ScaledMax;-1;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;50;-1123.448,-14.44986;Float;False;Property;_TileSize;TileSize;2;0;Create;True;0;0;0;False;0;False;2,2;2,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;56;-1137.555,-154.6484;Float;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;-789.0002,171.3005;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;54;-866.0472,-100.3501;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1025.2,-266.6001;Inherit;False;0;30;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-664.9017,127.0011;Float;False;Size;0;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FmodOpNode;49;-683.4481,-184.4499;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-534.299,-93.79826;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-381.1538,-36.34888;Float;False;TiledVar;1;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-402.9997,-218.998;Inherit;False;27;ScaledMin;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-224.9486,51.95014;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-191.198,-148.7983;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;30;-58.50118,-225.9996;Inherit;True;Property;_Atlas;Atlas;3;0;Create;True;0;0;0;False;0;False;-1;None;c1efbea8a190455497f6910632f58675;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;251.5002,-363.0998;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AmplifyShaderPack/Built-in/ReadFromAtlasTiled;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;57;0;19;1
WireConnection;57;1;19;2
WireConnection;22;0;57;0
WireConnection;22;1;13;0
WireConnection;23;0;14;0
WireConnection;23;1;57;0
WireConnection;27;0;22;0
WireConnection;26;0;23;0
WireConnection;38;0;26;0
WireConnection;38;1;27;0
WireConnection;54;0;56;0
WireConnection;54;1;50;0
WireConnection;39;0;38;0
WireConnection;49;0;12;0
WireConnection;49;1;54;0
WireConnection;42;0;49;0
WireConnection;42;1;39;0
WireConnection;55;0;42;0
WireConnection;53;0;55;0
WireConnection;53;1;50;0
WireConnection;40;0;41;0
WireConnection;40;1;53;0
WireConnection;30;1;40;0
WireConnection;0;2;30;0
ASEEND*/
//CHKSM=18D14CD6C42C9D68999BC97FD41EF5E3C516E1C0