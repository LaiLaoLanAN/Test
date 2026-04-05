Shader "Custom/ColorEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GrayIntensity ("Gray Intensity", Range(0, 1)) = 0.0
        _Contrast ("Contrast", Range(0, 2)) = 1.0 // 0=全灰，1=正常，>1=高对比度
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
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

            sampler2D _MainTex;
            float _GrayIntensity;
            float _Contrast;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 灰度计算
                float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
                float3 gray = luminance.xxx;
                
                // 应用灰度
                col.rgb = lerp(col.rgb, gray, _GrayIntensity);
                
                // 应用对比度（中值0.5作为基准）
                col.rgb = (col.rgb - 0.5) * _Contrast + 0.5;
                
                return saturate(col); // 确保颜色值在0-1范围内
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}