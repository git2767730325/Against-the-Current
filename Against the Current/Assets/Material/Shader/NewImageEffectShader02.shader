Shader "Test/NewImageEffectShader02"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Range("rotateSpeedRange",float)=1
        _RangeS("hdrRange",float)=1
        [HDR]_Color("hdrColor",Color)=(1,1,1,1)
        _rotateDir("_rotateDir",float)=1
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags {"Queue" = "Transparent" }
        // No culling or depth
        
            Cull Back ZWrite On ZTest LEqual
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Range;
            float4 _Color;
            float _RangeS;
            //float _Rangea=1;
            float _rotateDir;
            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 col = tex2D(_MainTex, i.uv);
                float angle=_Time.y*_Range*_rotateDir;
                float2 uv=(0,0);
                i.uv-=float2(0.5,0.5);
                if(length(i.uv)>0.5)
                    return fixed4(0,0,0,0);    
                uv.x=i.uv.x*cos(angle)+i.uv.y*sin(angle);
                uv.y=i.uv.y*cos(angle)-i.uv.x*sin(angle);
                uv+=float2(0.5,0.5);
                fixed4 col = tex2D(_MainTex, uv)*_Color*_Range*_RangeS;
                if(col.r+col.g+col.b<1)
                    col.a=0;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}
