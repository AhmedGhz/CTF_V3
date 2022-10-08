// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AmplifyShaderPack/Built-in/CustomRTRainInit"
{
    Properties
    {
        _InitialColor("InitialColor", Color) = (1,1,1,1)

    }

    SubShader
    {
		LOD 0

        
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Off
		ColorMask RGBA
		ZWrite Off
		ZTest Always
		
		
        Pass
        {
			Name "Custom RT Init"
            CGPROGRAM
            
            #include "UnityCustomRenderTexture.cginc"

            #pragma vertex ASEInitCustomRenderTextureVertexShader
            #pragma fragment frag
            #pragma target 3.0
			

			struct ase_appdata_init_customrendertexture
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				
			};

			// User facing vertex to fragment structure for initialization materials
			struct ase_v2f_init_customrendertexture
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float3 direction : TEXCOORD1;
				
			};

			uniform float4 _InitialColor;


			ase_v2f_init_customrendertexture ASEInitCustomRenderTextureVertexShader (ase_appdata_init_customrendertexture v )
			{
				ase_v2f_init_customrendertexture o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = float3(v.texcoord.xy, CustomRenderTexture3DTexcoordW);
				o.direction = CustomRenderTextureComputeCubeDirection(v.texcoord.xy);
				return o;
			}

            float4 frag(ase_v2f_init_customrendertexture IN ) : COLOR
            {
                float4 finalColor;
				
                finalColor = _InitialColor;
				return finalColor;
            }
            ENDCG
        }
    }

	
}
/*ASEBEGIN
Version=18901
1270;73;649;926;292.5;196;1;True;False
Node;AmplifyShaderEditor.ColorNode;3;-269,-4.5;Float;False;Property;_InitialColor;InitialColor;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;2;AmplifyShaderPack/Built-in/CustomRTRainInit;6ce779933eb99f049b78d6163735e06f;True;Custom RT Init;0;0;Custom RT Init;1;False;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;2;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;7;False;-1;True;False;0;False;-1;0;False;-1;True;0;True;2;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=398FA48182B0C56B6E4E5525ACE7537B27108574