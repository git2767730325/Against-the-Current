// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:33482,y:33252,varname:node_1,prsc:2|diff-66-OUT,spec-38-OUT,gloss-46-OUT,normal-56-OUT,emission-73-OUT,lwrap-92-OUT,amdfl-148-OUT,clip-3188-OUT;n:type:ShaderForge.SFN_Tex2d,id:2,x:32329,y:32586,ptovrint:False,ptlb:Base,ptin:_Base,varname:node_6821,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3,x:33292,y:32770,ptovrint:False,ptlb:Ambient Oclussion,ptin:_AmbientOclussion,varname:node_71,prsc:2,ntxv:0,isnm:False|MIP-260-OUT;n:type:ShaderForge.SFN_Tex2d,id:5,x:31656,y:32885,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_7757,prsc:2,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Tex2d,id:7,x:32144,y:32811,ptovrint:False,ptlb:Specular,ptin:_Specular,varname:node_9829,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9,x:32144,y:32995,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_8343,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:11,x:32326,y:33311,ptovrint:False,ptlb:Reflection Mask,ptin:_ReflectionMask,varname:node_1726,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:13,x:32519,y:33621,ptovrint:False,ptlb:Trans Mask,ptin:_TransMask,varname:node_4467,prsc:2,tex:72f052122502345f1b365b37455a8e3d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Cubemap,id:16,x:32120,y:33318,ptovrint:False,ptlb:Reflection Map,ptin:_ReflectionMap,varname:node_6648,prsc:2,cube:a596436b21c6d484bb9b3b6385e3e666,pvfc:0;n:type:ShaderForge.SFN_Lerp,id:17,x:33527,y:32742,varname:node_17,prsc:2|A-3-A,B-20-OUT,T-21-OUT;n:type:ShaderForge.SFN_Vector1,id:18,x:32890,y:32641,varname:node_18,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:20,x:32890,y:32588,varname:node_20,prsc:2,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:21,x:33292,y:32960,ptovrint:False,ptlb:AO Burn,ptin:_AOBurn,varname:node_9602,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:22,x:33292,y:33057,ptovrint:False,ptlb:AO Level,ptin:_AOLevel,varname:node_2056,prsc:2,glob:False,v1:0.5;n:type:ShaderForge.SFN_SwitchProperty,id:24,x:33292,y:32608,ptovrint:False,ptlb:AO Enable,ptin:_AOEnable,varname:node_2796,prsc:2,on:True|A-20-OUT,B-220-OUT;n:type:ShaderForge.SFN_Multiply,id:25,x:32487,y:32603,varname:node_25,prsc:2|A-2-RGB,B-27-OUT;n:type:ShaderForge.SFN_Tex2d,id:26,x:32329,y:32392,ptovrint:False,ptlb:Mask Map,ptin:_MaskMap,varname:node_7782,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_OneMinus,id:27,x:32500,y:32392,varname:node_27,prsc:2|IN-26-A;n:type:ShaderForge.SFN_Color,id:29,x:32524,y:32213,ptovrint:False,ptlb:Color Tint,ptin:_ColorTint,varname:node_8328,prsc:2,glob:False,c1:0.02205884,c2:1,c3:0.5808825,c4:1;n:type:ShaderForge.SFN_Multiply,id:30,x:32697,y:32350,varname:node_30,prsc:2|A-29-RGB,B-26-A;n:type:ShaderForge.SFN_Add,id:31,x:32660,y:32541,varname:node_31,prsc:2|A-30-OUT,B-25-OUT;n:type:ShaderForge.SFN_Multiply,id:32,x:32660,y:32691,varname:node_32,prsc:2|A-24-OUT,B-168-OUT;n:type:ShaderForge.SFN_Multiply,id:38,x:32360,y:32811,varname:node_38,prsc:2|A-7-A,B-39-OUT,C-24-OUT,D-176-RGB;n:type:ShaderForge.SFN_ValueProperty,id:39,x:32360,y:32950,ptovrint:False,ptlb:Specular Level,ptin:_SpecularLevel,varname:node_5880,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:46,x:32583,y:33019,varname:node_46,prsc:2|A-47-OUT,B-48-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:47,x:32450,y:33155,ptovrint:False,ptlb:Custom Gloss,ptin:_CustomGloss,varname:node_7358,prsc:2,on:False|A-20-OUT,B-9-A;n:type:ShaderForge.SFN_Slider,id:48,x:32281,y:33039,ptovrint:False,ptlb:Shinniness,ptin:_Shinniness,varname:node_4193,prsc:2,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Vector3,id:54,x:31656,y:33057,varname:node_54,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_ValueProperty,id:55,x:31656,y:33177,ptovrint:False,ptlb:Normal Smooth,ptin:_NormalSmooth,varname:node_2524,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Lerp,id:56,x:31867,y:33029,varname:node_56,prsc:2|A-5-RGB,B-54-OUT,T-55-OUT;n:type:ShaderForge.SFN_Multiply,id:62,x:32508,y:33311,varname:node_62,prsc:2|A-16-RGB,B-11-A;n:type:ShaderForge.SFN_Color,id:63,x:32625,y:33191,ptovrint:False,ptlb:Reflection Tint,ptin:_ReflectionTint,varname:node_7806,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:64,x:32625,y:33393,ptovrint:False,ptlb:Reflection Power,ptin:_ReflectionPower,varname:node_5853,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:65,x:32782,y:33191,varname:node_65,prsc:2|A-63-RGB,B-62-OUT,C-64-OUT;n:type:ShaderForge.SFN_Blend,id:66,x:32855,y:32892,varname:node_66,prsc:2,blmd:5,clmp:True|SRC-32-OUT,DST-65-OUT;n:type:ShaderForge.SFN_ValueProperty,id:72,x:32766,y:33393,ptovrint:False,ptlb:Reflection Emission,ptin:_ReflectionEmission,varname:node_9457,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:73,x:32929,y:33271,varname:node_73,prsc:2|A-65-OUT,B-72-OUT;n:type:ShaderForge.SFN_ValueProperty,id:80,x:32519,y:33804,ptovrint:False,ptlb:Trans Power,ptin:_TransPower,varname:node_3777,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:81,x:32730,y:33614,varname:node_81,prsc:2|A-13-A,B-80-OUT,C-285-RGB;n:type:ShaderForge.SFN_SwitchProperty,id:92,x:32919,y:33566,ptovrint:False,ptlb:Translucency,ptin:_Translucency,varname:node_2205,prsc:2,on:False|A-18-OUT,B-81-OUT;n:type:ShaderForge.SFN_Multiply,id:137,x:33231,y:34452,varname:node_137,prsc:2|A-138-OUT,B-140-OUT,C-141-RGB;n:type:ShaderForge.SFN_Fresnel,id:138,x:32961,y:34331,varname:node_138,prsc:2|EXP-139-OUT;n:type:ShaderForge.SFN_ValueProperty,id:139,x:32785,y:34349,ptovrint:False,ptlb:Rim Fresnel,ptin:_RimFresnel,varname:node_1286,prsc:2,glob:False,v1:3;n:type:ShaderForge.SFN_ValueProperty,id:140,x:32961,y:34486,ptovrint:False,ptlb:Rim Power,ptin:_RimPower,varname:node_7177,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Color,id:141,x:32961,y:34578,ptovrint:False,ptlb:Rim Color,ptin:_RimColor,varname:node_599,prsc:2,glob:False,c1:0.9264706,c2:0.5858564,c3:0.5858564,c4:1;n:type:ShaderForge.SFN_Tex2d,id:142,x:32961,y:34761,ptovrint:False,ptlb:Rim Mask,ptin:_RimMask,varname:node_4907,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Blend,id:143,x:33231,y:34604,varname:node_143,prsc:2,blmd:0,clmp:True|SRC-145-OUT,DST-142-A;n:type:ShaderForge.SFN_Max,id:145,x:33407,y:34452,varname:node_145,prsc:2|A-18-OUT,B-137-OUT;n:type:ShaderForge.SFN_Multiply,id:146,x:33277,y:34836,varname:node_146,prsc:2|A-149-RGB,B-150-OUT;n:type:ShaderForge.SFN_Add,id:148,x:33560,y:34706,varname:node_148,prsc:2|A-143-OUT,B-146-OUT;n:type:ShaderForge.SFN_AmbientLight,id:149,x:32942,y:34945,varname:node_149,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:150,x:32942,y:35100,ptovrint:False,ptlb:Ambiental Power,ptin:_AmbientalPower,varname:node_931,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_SwitchProperty,id:168,x:32859,y:32774,ptovrint:False,ptlb:Color Mask,ptin:_ColorMask,varname:node_3804,prsc:2,on:False|A-2-RGB,B-31-OUT;n:type:ShaderForge.SFN_Color,id:176,x:32530,y:32852,ptovrint:False,ptlb:Spec Color,ptin:_SpecColor,varname:node_6665,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Min,id:197,x:33687,y:32899,varname:node_197,prsc:2|A-243-OUT,B-20-OUT;n:type:ShaderForge.SFN_Max,id:220,x:33887,y:32899,varname:node_220,prsc:2|A-197-OUT,B-18-OUT;n:type:ShaderForge.SFN_Blend,id:243,x:33504,y:32960,varname:node_243,prsc:2,blmd:10,clmp:True|SRC-17-OUT,DST-22-OUT;n:type:ShaderForge.SFN_Slider,id:260,x:33250,y:33157,ptovrint:False,ptlb:AO Detail,ptin:_AODetail,varname:node_4207,prsc:2,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Color,id:285,x:32680,y:33477,ptovrint:False,ptlb:Trans Color,ptin:_TransColor,varname:node_6346,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:9582,x:33472,y:33860,ptovrint:False,ptlb:Cutout multiplier,ptin:_Cutoutmultiplier,varname:node_9582,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:7294,x:33479,y:33970,ptovrint:False,ptlb:Alpha Map,ptin:_AlphaMap,varname:node_7294,prsc:2,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3188,x:33237,y:33633,varname:node_3188,prsc:2|A-9582-OUT,B-7294-A;proporder:2-168-29-26-176-39-7-48-47-9-5-55-24-3-21-22-260-63-64-16-11-72-92-80-285-13-141-140-139-142-150-9582-7294;pass:END;sub:END;*/

Shader "DLNK/SuperShaderCutout" {
    Properties {
        _Base ("Base", 2D) = "white" {}
        [MaterialToggle] _ColorMask ("Color Mask", Float ) = 1
        _ColorTint ("Color Tint", Color) = (0.02205884,1,0.5808825,1)
        _MaskMap ("Mask Map", 2D) = "white" {}
        _SpecColor ("Spec Color", Color) = (0.5,0.5,0.5,1)
        _SpecularLevel ("Specular Level", Float ) = 1
        _Specular ("Specular", 2D) = "white" {}
        _Shinniness ("Shinniness", Range(0, 1)) = 0.5
        [MaterialToggle] _CustomGloss ("Custom Gloss", Float ) = 1
        _Gloss ("Gloss", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        _NormalSmooth ("Normal Smooth", Float ) = 0
        [MaterialToggle] _AOEnable ("AO Enable", Float ) = 1
        _AmbientOclussion ("Ambient Oclussion", 2D) = "white" {}
        _AOBurn ("AO Burn", Float ) = 0
        _AOLevel ("AO Level", Float ) = 0.5
        _AODetail ("AO Detail", Range(0, 10)) = 0
        _ReflectionTint ("Reflection Tint", Color) = (1,1,1,1)
        _ReflectionPower ("Reflection Power", Float ) = 1
        _ReflectionMap ("Reflection Map", Cube) = "_Skybox" {}
        _ReflectionMask ("Reflection Mask", 2D) = "white" {}
        _ReflectionEmission ("Reflection Emission", Float ) = 0
        [MaterialToggle] _Translucency ("Translucency", Float ) = 0
        _TransPower ("Trans Power", Float ) = 1
        _TransColor ("Trans Color", Color) = (0.5,0.5,0.5,1)
        _TransMask ("Trans Mask", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (0.9264706,0.5858564,0.5858564,1)
        _RimPower ("Rim Power", Float ) = 1
        _RimFresnel ("Rim Fresnel", Float ) = 3
        _RimMask ("Rim Mask", 2D) = "white" {}
        _AmbientalPower ("Ambiental Power", Float ) = 1
        _Cutoutmultiplier ("Cutout multiplier", Float ) = 1
        _AlphaMap ("Alpha Map", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float4 _LightColor0;
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _AmbientOclussion; uniform float4 _AmbientOclussion_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Gloss; uniform float4 _Gloss_ST;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform sampler2D _TransMask; uniform float4 _TransMask_ST;
            uniform samplerCUBE _ReflectionMap;
            uniform float _AOBurn;
            uniform float _AOLevel;
            uniform fixed _AOEnable;
            uniform sampler2D _MaskMap; uniform float4 _MaskMap_ST;
            uniform float4 _ColorTint;
            uniform float _SpecularLevel;
            uniform fixed _CustomGloss;
            uniform float _Shinniness;
            uniform float _NormalSmooth;
            uniform float4 _ReflectionTint;
            uniform float _ReflectionPower;
            uniform float _ReflectionEmission;
            uniform float _TransPower;
            uniform fixed _Translucency;
            uniform float _RimFresnel;
            uniform float _RimPower;
            uniform float4 _RimColor;
            uniform sampler2D _RimMask; uniform float4 _RimMask_ST;
            uniform float _AmbientalPower;
            uniform fixed _ColorMask;
            uniform float4 _SpecColor;
            uniform float _AODetail;
            uniform float4 _TransColor;
            uniform float _Cutoutmultiplier;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = lerp(_Normal_var.rgb,float3(0,0,1),_NormalSmooth);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip( BinaryDither3x3((_Cutoutmultiplier*_AlphaMap_var.a) - 1.5, sceneUVs) );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float node_20 = 1.0;
                float4 _Gloss_var = tex2D(_Gloss,TRANSFORM_TEX(i.uv0, _Gloss));
                float gloss = (lerp( node_20, _Gloss_var.a, _CustomGloss )*_Shinniness);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float4 _AmbientOclussion_var = tex2Dlod(_AmbientOclussion,float4(TRANSFORM_TEX(i.uv0, _AmbientOclussion),0.0,_AODetail));
                float node_18 = 0.0;
                float _AOEnable_var = lerp( node_20, max(min(saturate(( _AOLevel > 0.5 ? (1.0-(1.0-2.0*(_AOLevel-0.5))*(1.0-lerp(_AmbientOclussion_var.a,node_20,_AOBurn))) : (2.0*_AOLevel*lerp(_AmbientOclussion_var.a,node_20,_AOBurn)) )),node_20),node_18), _AOEnable );
                float3 specularColor = (_Specular_var.a*_SpecularLevel*_AOEnable_var*_SpecColor.rgb);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float4 _TransMask_var = tex2D(_TransMask,TRANSFORM_TEX(i.uv0, _TransMask));
                float3 w = lerp( node_18, (_TransMask_var.a*_TransPower*_TransColor.rgb), _Translucency )*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = forwardLight * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _RimMask_var = tex2D(_RimMask,TRANSFORM_TEX(i.uv0, _RimMask));
                indirectDiffuse += (saturate(min(max(node_18,(pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimFresnel)*_RimPower*_RimColor.rgb)),_RimMask_var.a))+(UNITY_LIGHTMODEL_AMBIENT.rgb*_AmbientalPower)); // Diffuse Ambient Light
                float4 _Base_var = tex2D(_Base,TRANSFORM_TEX(i.uv0, _Base));
                float4 _MaskMap_var = tex2D(_MaskMap,TRANSFORM_TEX(i.uv0, _MaskMap));
                float4 _ReflectionMask_var = tex2D(_ReflectionMask,TRANSFORM_TEX(i.uv0, _ReflectionMask));
                float3 node_65 = (_ReflectionTint.rgb*(texCUBE(_ReflectionMap,viewReflectDirection).rgb*_ReflectionMask_var.a)*_ReflectionPower);
                float3 diffuse = (directDiffuse + indirectDiffuse) * saturate(max((_AOEnable_var*lerp( _Base_var.rgb, ((_ColorTint.rgb*_MaskMap_var.a)+(_Base_var.rgb*(1.0 - _MaskMap_var.a))), _ColorMask )),node_65));
////// Emissive:
                float3 emissive = (node_65*_ReflectionEmission);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float4 _LightColor0;
            uniform sampler2D _Base; uniform float4 _Base_ST;
            uniform sampler2D _AmbientOclussion; uniform float4 _AmbientOclussion_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;
            uniform sampler2D _Specular; uniform float4 _Specular_ST;
            uniform sampler2D _Gloss; uniform float4 _Gloss_ST;
            uniform sampler2D _ReflectionMask; uniform float4 _ReflectionMask_ST;
            uniform sampler2D _TransMask; uniform float4 _TransMask_ST;
            uniform samplerCUBE _ReflectionMap;
            uniform float _AOBurn;
            uniform float _AOLevel;
            uniform fixed _AOEnable;
            uniform sampler2D _MaskMap; uniform float4 _MaskMap_ST;
            uniform float4 _ColorTint;
            uniform float _SpecularLevel;
            uniform fixed _CustomGloss;
            uniform float _Shinniness;
            uniform float _NormalSmooth;
            uniform float4 _ReflectionTint;
            uniform float _ReflectionPower;
            uniform float _ReflectionEmission;
            uniform float _TransPower;
            uniform fixed _Translucency;
            uniform fixed _ColorMask;
            uniform float4 _SpecColor;
            uniform float _AODetail;
            uniform float4 _TransColor;
            uniform float _Cutoutmultiplier;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = lerp(_Normal_var.rgb,float3(0,0,1),_NormalSmooth);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip( BinaryDither3x3((_Cutoutmultiplier*_AlphaMap_var.a) - 1.5, sceneUVs) );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float node_20 = 1.0;
                float4 _Gloss_var = tex2D(_Gloss,TRANSFORM_TEX(i.uv0, _Gloss));
                float gloss = (lerp( node_20, _Gloss_var.a, _CustomGloss )*_Shinniness);
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _Specular_var = tex2D(_Specular,TRANSFORM_TEX(i.uv0, _Specular));
                float4 _AmbientOclussion_var = tex2Dlod(_AmbientOclussion,float4(TRANSFORM_TEX(i.uv0, _AmbientOclussion),0.0,_AODetail));
                float node_18 = 0.0;
                float _AOEnable_var = lerp( node_20, max(min(saturate(( _AOLevel > 0.5 ? (1.0-(1.0-2.0*(_AOLevel-0.5))*(1.0-lerp(_AmbientOclussion_var.a,node_20,_AOBurn))) : (2.0*_AOLevel*lerp(_AmbientOclussion_var.a,node_20,_AOBurn)) )),node_20),node_18), _AOEnable );
                float3 specularColor = (_Specular_var.a*_SpecularLevel*_AOEnable_var*_SpecColor.rgb);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow);
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float4 _TransMask_var = tex2D(_TransMask,TRANSFORM_TEX(i.uv0, _TransMask));
                float3 w = lerp( node_18, (_TransMask_var.a*_TransPower*_TransColor.rgb), _Translucency )*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 directDiffuse = forwardLight * attenColor;
                float4 _Base_var = tex2D(_Base,TRANSFORM_TEX(i.uv0, _Base));
                float4 _MaskMap_var = tex2D(_MaskMap,TRANSFORM_TEX(i.uv0, _MaskMap));
                float4 _ReflectionMask_var = tex2D(_ReflectionMask,TRANSFORM_TEX(i.uv0, _ReflectionMask));
                float3 node_65 = (_ReflectionTint.rgb*(texCUBE(_ReflectionMap,viewReflectDirection).rgb*_ReflectionMask_var.a)*_ReflectionPower);
                float3 diffuse = directDiffuse * saturate(max((_AOEnable_var*lerp( _Base_var.rgb, ((_ColorTint.rgb*_MaskMap_var.a)+(_Base_var.rgb*(1.0 - _MaskMap_var.a))), _ColorMask )),node_65));
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCollector"
            Tags {
                "LightMode"="ShadowCollector"
            }
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCOLLECTOR
            #define SHADOW_COLLECTOR_PASS
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcollector
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float _Cutoutmultiplier;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_COLLECTOR;
                float2 uv0 : TEXCOORD5;
                float4 screenPos : TEXCOORD6;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                TRANSFER_SHADOW_COLLECTOR(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip( BinaryDither3x3((_Cutoutmultiplier*_AlphaMap_var.a) - 1.5, sceneUVs) );
                SHADOW_COLLECTOR_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Cull Off
            Offset 1, 1
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            // Dithering function, to use with scene UVs (screen pixel coords)
            // 3x3 Bayer matrix, based on https://en.wikipedia.org/wiki/Ordered_dithering
            float BinaryDither3x3( float value, float2 sceneUVs ) {
                float3x3 mtx = float3x3(
                    float3( 3,  7,  4 )/10.0,
                    float3( 6,  1,  9 )/10.0,
                    float3( 2,  8,  5 )/10.0
                );
                float2 px = floor(_ScreenParams.xy * sceneUVs);
                int xSmp = fmod(px.x,3);
                int ySmp = fmod(px.y,3);
                float3 xVec = 1-saturate(abs(float3(0,1,2) - xSmp));
                float3 yVec = 1-saturate(abs(float3(0,1,2) - ySmp));
                float3 pxMult = float3( dot(mtx[0],yVec), dot(mtx[1],yVec), dot(mtx[2],yVec) );
                return round(value + dot(pxMult, xVec));
            }
            uniform float _Cutoutmultiplier;
            uniform sampler2D _AlphaMap; uniform float4 _AlphaMap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = o.pos;
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5;
/////// Vectors:
                float4 _AlphaMap_var = tex2D(_AlphaMap,TRANSFORM_TEX(i.uv0, _AlphaMap));
                clip( BinaryDither3x3((_Cutoutmultiplier*_AlphaMap_var.a) - 1.5, sceneUVs) );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
