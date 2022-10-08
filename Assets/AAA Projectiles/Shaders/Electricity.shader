// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33025,y:32662,varname:node_4795,prsc:2|emission-4162-OUT,alpha-3842-OUT;n:type:ShaderForge.SFN_Tex2d,id:8543,x:30610,y:32857,varname:node_3095,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-4083-OUT,TEX-6864-TEX;n:type:ShaderForge.SFN_Slider,id:6745,x:30104,y:32650,ptovrint:False,ptlb:Dissolve amount,ptin:_Dissolveamount,varname:node_3858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.332,max:1;n:type:ShaderForge.SFN_Add,id:3403,x:30791,y:32650,varname:node_3403,prsc:2|A-4576-OUT,B-8543-R;n:type:ShaderForge.SFN_RemapRange,id:4576,x:30610,y:32650,varname:node_4576,prsc:2,frmn:0,frmx:1,tomn:-0.65,tomx:0.65|IN-5252-OUT;n:type:ShaderForge.SFN_OneMinus,id:5252,x:30436,y:32650,varname:node_5252,prsc:2|IN-6745-OUT;n:type:ShaderForge.SFN_Clamp01,id:9277,x:31396,y:32650,varname:node_9277,prsc:2|IN-9180-OUT;n:type:ShaderForge.SFN_Append,id:1295,x:31812,y:32650,varname:node_1295,prsc:2|A-5356-OUT,B-4824-OUT;n:type:ShaderForge.SFN_Vector1,id:4824,x:31601,y:32788,varname:node_4824,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2d,id:8029,x:31985,y:32650,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_8891,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:cffc28da632aab1458403329eaf7f668,ntxv:0,isnm:False|UVIN-1295-OUT;n:type:ShaderForge.SFN_OneMinus,id:5356,x:31601,y:32650,varname:node_5356,prsc:2|IN-9277-OUT;n:type:ShaderForge.SFN_Append,id:8111,x:30076,y:32857,varname:node_8111,prsc:2|A-298-X,B-298-Y;n:type:ShaderForge.SFN_Time,id:8033,x:30076,y:33009,varname:node_8033,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7462,x:30255,y:32857,varname:node_7462,prsc:2|A-8111-OUT,B-8033-T;n:type:ShaderForge.SFN_TexCoord,id:6967,x:30255,y:33009,varname:node_6967,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:4083,x:30436,y:32857,varname:node_4083,prsc:2|A-7462-OUT,B-6967-UVOUT;n:type:ShaderForge.SFN_Add,id:8113,x:30791,y:32857,varname:node_8113,prsc:2|A-4576-OUT,B-4110-R;n:type:ShaderForge.SFN_Multiply,id:5041,x:30987,y:32650,varname:node_5041,prsc:2|A-3403-OUT,B-8113-OUT;n:type:ShaderForge.SFN_Append,id:6296,x:30068,y:33166,varname:node_6296,prsc:2|A-298-Z,B-298-W;n:type:ShaderForge.SFN_Multiply,id:4841,x:30255,y:33166,varname:node_4841,prsc:2|A-6296-OUT,B-8033-T;n:type:ShaderForge.SFN_Add,id:2410,x:30436,y:33166,varname:node_2410,prsc:2|A-6967-UVOUT,B-4841-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:6864,x:30436,y:33009,ptovrint:False,ptlb:Main Texture,ptin:_MainTexture,varname:node_6645,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:4110,x:30613,y:33166,varname:node_5059,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-2410-OUT,TEX-6864-TEX;n:type:ShaderForge.SFN_Multiply,id:4162,x:32453,y:32768,varname:node_4162,prsc:2|A-5343-OUT,B-8253-RGB,C-6607-RGB,D-3491-OUT,E-6642-OUT;n:type:ShaderForge.SFN_VertexColor,id:8253,x:31985,y:32838,varname:node_8253,prsc:2;n:type:ShaderForge.SFN_Color,id:6607,x:31985,y:33001,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_2840,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3897059,c2:0.4191683,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:6642,x:31985,y:33310,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_1748,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:6;n:type:ShaderForge.SFN_Fresnel,id:281,x:31574,y:33203,varname:node_281,prsc:2|EXP-8780-Z;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:9180,x:31188,y:32650,varname:node_9180,prsc:2|IN-5041-OUT,IMIN-7182-OUT,IMAX-8999-OUT,OMIN-8780-X,OMAX-8780-Y;n:type:ShaderForge.SFN_Vector1,id:7182,x:30987,y:32773,varname:node_7182,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:8999,x:30987,y:32822,varname:node_8999,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector4Property,id:8780,x:30987,y:32903,ptovrint:False,ptlb:Remap XY/Fresnel ZW,ptin:_RemapXYFresnelZW,varname:node_1833,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:-10,v2:10,v3:2,v4:2;n:type:ShaderForge.SFN_Vector4Property,id:298,x:29842,y:32973,ptovrint:False,ptlb:Speed,ptin:_Speed,varname:node_2936,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.189,v2:0.225,v3:-0.2,v4:-0.05;n:type:ShaderForge.SFN_Clamp01,id:5343,x:32148,y:32650,varname:node_5343,prsc:2|IN-8029-R;n:type:ShaderForge.SFN_Multiply,id:8549,x:31803,y:33156,varname:node_8549,prsc:2|A-8780-W,B-281-OUT;n:type:ShaderForge.SFN_Clamp01,id:3491,x:31985,y:33156,varname:node_3491,prsc:2|IN-8549-OUT;n:type:ShaderForge.SFN_Multiply,id:9956,x:32453,y:32928,varname:node_9956,prsc:2|A-5343-OUT,B-8253-A,C-6607-A,D-3491-OUT,E-5365-OUT;n:type:ShaderForge.SFN_Slider,id:5365,x:31828,y:33413,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_7937,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_DepthBlend,id:3243,x:32474,y:33207,varname:node_3243,prsc:2|DIST-3129-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3129,x:32308,y:33207,ptovrint:False,ptlb:Depth,ptin:_Depth,varname:node_3129,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.15;n:type:ShaderForge.SFN_SwitchProperty,id:3842,x:32828,y:33028,ptovrint:False,ptlb:Use depth?,ptin:_Usedepth,varname:node_3842,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-9956-OUT,B-9373-OUT;n:type:ShaderForge.SFN_Multiply,id:9373,x:32639,y:33087,varname:node_9373,prsc:2|A-9956-OUT,B-3243-OUT;proporder:6864-6745-8029-6607-6642-8780-298-5365-3842-3129;pass:END;sub:END;*/

Shader "ErbGameArt/Particles/Blend_Electricity" {
    Properties {
        _MainTexture ("Main Texture", 2D) = "white" {}
        _Dissolveamount ("Dissolve amount", Range(0, 1)) = 0.332
        _Mask ("Mask", 2D) = "white" {}
        _Color ("Color", Color) = (0.3897059,0.4191683,1,1)
        _Emission ("Emission", Float ) = 6
        _RemapXYFresnelZW ("Remap XY/Fresnel ZW", Vector) = (-10,10,2,2)
        _Speed ("Speed", Vector) = (0.189,0.225,-0.2,-0.05)
        _Opacity ("Opacity", Range(0, 1)) = 1
        [MaterialToggle] _Usedepth ("Use depth?", Float ) = 0
        _Depth ("Depth", Float ) = 0.15
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
            uniform float _Dissolveamount;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _MainTexture; uniform float4 _MainTexture_ST;
            uniform float4 _Color;
            uniform float _Emission;
            uniform float4 _RemapXYFresnelZW;
            uniform float4 _Speed;
            uniform float _Opacity;
            uniform float _Depth;
            uniform fixed _Usedepth;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD3;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float node_4576 = ((1.0 - _Dissolveamount)*1.3+-0.65);
                float2 node_4083 = ((float2(_Speed.r,_Speed.g)*_Time.g)+i.uv0);
                float4 node_3095 = tex2D(_MainTexture,TRANSFORM_TEX(node_4083, _MainTexture));
                float2 node_2410 = (i.uv0+(float2(_Speed.b,_Speed.a)*_Time.g));
                float4 node_5059 = tex2D(_MainTexture,TRANSFORM_TEX(node_2410, _MainTexture));
                float2 node_1295 = float2((1.0 - saturate((_RemapXYFresnelZW.r + ( (((node_4576+node_3095.r)*(node_4576+node_5059.r)) - 0.0) * (_RemapXYFresnelZW.g - _RemapXYFresnelZW.r) ) / (1.0 - 0.0)))),0.0);
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(node_1295, _Mask));
                float node_5343 = saturate(_Mask_var.r);
                float node_3491 = saturate((_RemapXYFresnelZW.a*pow(1.0-max(0,dot(normalDirection, viewDirection)),_RemapXYFresnelZW.b)));
                float3 emissive = (node_5343*i.vertexColor.rgb*_Color.rgb*node_3491*_Emission);
                float node_9956 = (node_5343*i.vertexColor.a*_Color.a*node_3491*_Opacity);
                fixed4 finalRGBA = fixed4(emissive,lerp( node_9956, (node_9956*saturate((sceneZ-partZ)/_Depth)), _Usedepth ));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
}
