// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33425,y:32673,varname:node_4795,prsc:2|emission-8427-OUT,alpha-3663-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32236,y:32609,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5425-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32236,y:32797,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32236,y:32955,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:6178,x:32594,y:32797,varname:node_6178,prsc:2|A-3942-OUT,B-8960-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2461,x:32711,y:32865,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_2461,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_TexCoord,id:4036,x:30992,y:32862,varname:node_4036,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_Tex2d,id:3339,x:31337,y:32609,ptovrint:False,ptlb:Noise,ptin:_Noise,varname:node_3339,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3348-OUT;n:type:ShaderForge.SFN_Vector4Property,id:3172,x:30605,y:32609,ptovrint:False,ptlb:Noise speed U/V ,ptin:_NoisespeedUV,varname:node_3172,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Append,id:876,x:30790,y:32609,varname:node_876,prsc:2|A-3172-X,B-3172-Y;n:type:ShaderForge.SFN_Multiply,id:3113,x:30978,y:32609,varname:node_3113,prsc:2|A-7371-T,B-876-OUT;n:type:ShaderForge.SFN_Add,id:3348,x:31162,y:32609,varname:node_3348,prsc:2|A-3113-OUT,B-4036-UVOUT;n:type:ShaderForge.SFN_Time,id:7371,x:30790,y:32454,varname:node_7371,prsc:2;n:type:ShaderForge.SFN_DepthBlend,id:432,x:32424,y:33100,varname:node_432,prsc:2|DIST-6344-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6344,x:32236,y:33134,ptovrint:False,ptlb:Depth power,ptin:_Depthpower,varname:node_2141,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:4444,x:32865,y:32947,ptovrint:False,ptlb:Use depth?,ptin:_Usedepth,varname:node_2520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-8960-OUT,B-6652-OUT;n:type:ShaderForge.SFN_Multiply,id:8427,x:32865,y:32799,varname:node_8427,prsc:2|A-6178-OUT,B-2461-OUT;n:type:ShaderForge.SFN_Multiply,id:3942,x:32424,y:32797,varname:node_3942,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB;n:type:ShaderForge.SFN_Multiply,id:6652,x:32694,y:32990,varname:node_6652,prsc:2|A-8960-OUT,B-432-OUT;n:type:ShaderForge.SFN_Tex2d,id:3221,x:31337,y:32432,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_3221,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:5425,x:32073,y:32609,varname:node_5425,prsc:2|A-1298-OUT,B-4036-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:3739,x:31712,y:32609,varname:node_3739,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9866-OUT;n:type:ShaderForge.SFN_Multiply,id:9866,x:31531,y:32609,varname:node_9866,prsc:2|A-3221-RGB,B-3339-RGB;n:type:ShaderForge.SFN_Multiply,id:1298,x:31898,y:32609,varname:node_1298,prsc:2|A-3739-OUT,B-4036-UVOUT,C-1520-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1520,x:31712,y:32781,ptovrint:False,ptlb:Distortion power,ptin:_Distortionpower,varname:node_1520,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:8960,x:32424,y:32948,varname:node_8960,prsc:2|A-6074-A,B-2053-A,C-797-A;n:type:ShaderForge.SFN_Slider,id:7027,x:32708,y:33148,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7027,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:3663,x:33045,y:32947,varname:node_3663,prsc:2|A-4444-OUT,B-7027-OUT;proporder:6074-3339-797-2461-3172-4444-6344-3221-1520-7027;pass:END;sub:END;*/

Shader "ErbGameArt/Particles/Blend_DistortTexture" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Noise ("Noise", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _Emission ("Emission", Float ) = 2
        _NoisespeedUV ("Noise speed U/V", Vector) = (0,0,0,0)
        [MaterialToggle] _Usedepth ("Use depth?", Float ) = 0
        _Depthpower ("Depth power", Float ) = 1
        _Mask ("Mask", 2D) = "white" {}
        _Distortionpower ("Distortion power", Float ) = 0
        _Opacity ("Opacity", Range(0, 1)) = 1
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
			"PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            //#pragma multi_compile_fwdbase
            //#pragma multi_compile_fog
            //#pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _Emission;
            uniform sampler2D _Noise; uniform float4 _Noise_ST;
            uniform float4 _NoisespeedUV;
            uniform float _Depthpower;
            uniform fixed _Usedepth;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform float _Distortionpower;
            uniform float _Opacity;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float2 node_3348 = ((_Time.g*float2(_NoisespeedUV.r,_NoisespeedUV.g))+i.uv0);
                float4 _Noise_var = tex2D(_Noise,TRANSFORM_TEX(node_3348, _Noise));
                float2 node_5425 = (((_Mask_var.rgb*_Noise_var.rgb).rg*i.uv0*_Distortionpower)+i.uv0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5425, _MainTex));
                float node_8960 = (_MainTex_var.a*i.vertexColor.a*_TintColor.a);
                float3 emissive = (((_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb)*node_8960)*_Emission);
                fixed4 finalRGBA = fixed4(emissive,(lerp( node_8960, (node_8960*saturate((sceneZ-partZ)/_Depthpower)), _Usedepth )*_Opacity));
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
}
