// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hovl/Particles/Blend_Tornado"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[Toggle]_UpCutoff("Up Cutoff", Float) = 0
		[HDR]_Color("Color", Color) = (1,0.6,0,1)
		_SpeedXYFresnelEmission("Speed XY + Fresnel + Emission", Vector) = (-0.3,-0.7,2,2)
		[Toggle]_Fresnel("Fresnel", Float) = 0
		[HDR]_Fresnelcolor("Fresnel color", Color) = (1,1,1,1)
		_Numberofwaves("Number of waves", Float) = 1
		_WavesspeedsizeXYTwistspeedsizeZW("Waves speed-size XY Twist speed-size ZW", Vector) = (-1,0.2,4,0.6)
		_TwistWaves("Twist Waves", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex3coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow vertex:vertexDataFunc 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 uv_tex3coord;
			float3 worldPos;
			float3 worldNormal;
			half ASEVFace : VFACE;
		};

		uniform float _Numberofwaves;
		uniform float4 _WavesspeedsizeXYTwistspeedsizeZW;
		uniform float _TwistWaves;
		uniform float _Fresnel;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _SpeedXYFresnelEmission;
		uniform float4 _MainTex_ST;
		uniform float4 _Fresnelcolor;
		uniform float _UpCutoff;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float V162 = v.texcoord.xyz.y;
			float3 ase_vertex3Pos = v.vertex.xyz;
			float mulTime136 = _Time.y * _WavesspeedsizeXYTwistspeedsizeZW.x;
			float mulTime194 = _Time.y * _WavesspeedsizeXYTwistspeedsizeZW.z;
			float3 appendResult215 = (float3(( sin( ( ( ase_vertex3Pos.y + mulTime194 ) * _TwistWaves ) ) * V162 ) , 0.0 , ( V162 * sin( ( _TwistWaves * ( mulTime194 + ase_vertex3Pos.y + ( UNITY_PI / 2.0 ) ) ) ) )));
			v.vertex.xyz += ( ( ase_vertexNormal * ( V162 * sin( ( _Numberofwaves * ( ase_vertex3Pos.y + mulTime136 ) * UNITY_PI ) ) ) * _WavesspeedsizeXYTwistspeedsizeZW.y ) + ( _WavesspeedsizeXYTwistspeedsizeZW.w * appendResult215 ) );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult105 = (float2(_SpeedXYFresnelEmission.x , _SpeedXYFresnelEmission.y));
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 panner102 = ( 1.0 * _Time.y * appendResult105 + ( uv_MainTex + i.uv_tex3coord.z ));
			float4 tex2DNode13 = tex2D( _MainTex, panner102 );
			float3 temp_output_111_0 = (( _Color * i.vertexColor * tex2DNode13 * _SpeedXYFresnelEmission.w )).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV92 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode92 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV92, _SpeedXYFresnelEmission.z ) );
			float3 clampResult116 = clamp( ( fresnelNode92 * fresnelNode92 * (_Fresnelcolor).rgb ) , float3( 0,0,0 ) , float3( 1,1,1 ) );
			float3 switchResult126 = (((i.ASEVFace>0)?(clampResult116):(float3( 0,0,0 ))));
			o.Emission = (( _Fresnel )?( ( temp_output_111_0 + switchResult126 ) ):( temp_output_111_0 ));
			o.Alpha = 1;
			float V162 = i.uv_tex3coord.y;
			float clampResult131 = clamp( pow( V162 , 20.0 ) , 0.0 , 1.0 );
			float clampResult129 = clamp( ( i.vertexColor.a - clampResult131 ) , 0.0 , 1.0 );
			clip( ( tex2DNode13.r + (-0.5 + ((( _UpCutoff )?( clampResult129 ):( i.vertexColor.a )) - 0.0) * (0.5 - -0.5) / (1.0 - 0.0)) ) - _Cutoff );
		}

		ENDCG
	}
}
/*ASEBEGIN
Version=18900
0;8;1918;1011;2340.574;60.85034;2.016886;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;262;-1502.736,403.2581;Inherit;False;0;-1;3;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;259;-1064.534,1005.566;Float;False;Property;_WavesspeedsizeXYTwistspeedsizeZW;Waves speed-size XY Twist speed-size ZW;8;0;Create;True;0;0;0;False;0;False;-1,0.2,4,0.6;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;217;-863.0305,1549.201;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;104;-1408.028,208.5537;Float;False;Property;_SpeedXYFresnelEmission;Speed XY + Fresnel + Emission;4;0;Create;True;0;0;0;False;0;False;-0.3,-0.7,2,2;0,0,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;103;-1577.523,-0.06123567;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;162;-1207.909,426.1062;Float;False;V;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;218;-672.2368,1543.652;Inherit;False;2;0;FLOAT;2;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;194;-728.8644,1295.759;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;182;-983.6356,1256.438;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;212;-500.4071,1170.434;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;264;-580.9677,1357.006;Inherit;False;Property;_TwistWaves;Twist Waves;9;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;115;-1077.109,552.7286;Float;False;Property;_Fresnelcolor;Fresnel color;6;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;128;-905.5161,216.7327;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;-1103.619,190.869;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;136;-731.1426,870.1433;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;263;-1202.631,30.66148;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;223;-509.3324,1470.52;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;118;-802.3303,551.567;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;131;-751.0924,261.7297;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;107;-653.1189,-79.71218;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;134;-507.9478,831.4379;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;140;-533.2584,752.8935;Float;False;Property;_Numberofwaves;Number of waves;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;139;-499.0641,933.8414;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;102;-945.4998,106.0993;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;92;-830.849,388.0453;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-366.7826,1430.802;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;266;-372.6909,1188.116;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-308.364,807.8345;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;214;-237.6411,1170.258;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;-682.632,-250.0441;Float;False;Property;_Color;Color;3;1;[HDR];Create;True;0;0;0;False;0;False;1,0.6,0,1;1,0.3087967,0,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SinOpNode;224;-229.9132,1494.939;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-551.061,375.0023;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;13;-768.4219,77.11012;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;109;-510.9174,319.1026;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;130;-401.0951,255.7524;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;240;-362.1234,1319.39;Inherit;False;162;V;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;116;-404.1171,374.7019;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;-102.6207,1441.083;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;129;-249.6181,254.19;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-169.4303,694.9666;Inherit;False;162;V;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;-400.9251,-65.87315;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;141;-155.6895,807.0349;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;213;-102.143,1171.872;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;111;-213.7003,-28.46989;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;127;43.85934,248.665;Float;False;Property;_UpCutoff;Up Cutoff;2;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;145;72.00549,535.1841;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;260;-3.214251,1011.204;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;82.69418,684.108;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;126;-244.9501,375.5855;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;215;70.57986,1257.814;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;123;259.8736,267.1765;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.5;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;293.0871,1094.002;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;110;44.84188,103.5352;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;352.6382,678.576;Inherit;False;3;3;0;FLOAT3;1,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;227;583.0101,791.52;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;113;239.1762,27.6867;Float;False;Property;_Fresnel;Fresnel;5;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;122;482.011,204.1862;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;100;817.7408,222.7795;Float;False;True;-1;2;;0;0;Standard;Hovl/Particles/Blend_Tornado;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;162;0;262;2
WireConnection;218;0;217;0
WireConnection;194;0;259;3
WireConnection;212;0;182;2
WireConnection;212;1;194;0
WireConnection;128;0;162;0
WireConnection;105;0;104;1
WireConnection;105;1;104;2
WireConnection;136;0;259;1
WireConnection;263;0;103;0
WireConnection;263;1;262;3
WireConnection;223;0;194;0
WireConnection;223;1;182;2
WireConnection;223;2;218;0
WireConnection;118;0;115;0
WireConnection;131;0;128;0
WireConnection;134;0;182;2
WireConnection;134;1;136;0
WireConnection;102;0;263;0
WireConnection;102;2;105;0
WireConnection;92;3;104;3
WireConnection;265;0;264;0
WireConnection;265;1;223;0
WireConnection;266;0;212;0
WireConnection;266;1;264;0
WireConnection;138;0;140;0
WireConnection;138;1;134;0
WireConnection;138;2;139;0
WireConnection;214;0;266;0
WireConnection;224;0;265;0
WireConnection;114;0;92;0
WireConnection;114;1;92;0
WireConnection;114;2;118;0
WireConnection;13;1;102;0
WireConnection;109;0;104;4
WireConnection;130;0;107;4
WireConnection;130;1;131;0
WireConnection;116;0;114;0
WireConnection;225;0;240;0
WireConnection;225;1;224;0
WireConnection;129;0;130;0
WireConnection;106;0;31;0
WireConnection;106;1;107;0
WireConnection;106;2;13;0
WireConnection;106;3;109;0
WireConnection;141;0;138;0
WireConnection;213;0;214;0
WireConnection;213;1;240;0
WireConnection;111;0;106;0
WireConnection;127;0;107;4
WireConnection;127;1;129;0
WireConnection;260;0;259;2
WireConnection;142;0;163;0
WireConnection;142;1;141;0
WireConnection;126;0;116;0
WireConnection;215;0;213;0
WireConnection;215;2;225;0
WireConnection;123;0;127;0
WireConnection;203;0;259;4
WireConnection;203;1;215;0
WireConnection;110;0;111;0
WireConnection;110;1;126;0
WireConnection;146;0;145;0
WireConnection;146;1;142;0
WireConnection;146;2;260;0
WireConnection;227;0;146;0
WireConnection;227;1;203;0
WireConnection;113;0;111;0
WireConnection;113;1;110;0
WireConnection;122;0;13;1
WireConnection;122;1;123;0
WireConnection;100;2;113;0
WireConnection;100;10;122;0
WireConnection;100;11;227;0
ASEEND*/
//CHKSM=22EB2FC5F855C283EBCDBC7C38BF28C566583C61