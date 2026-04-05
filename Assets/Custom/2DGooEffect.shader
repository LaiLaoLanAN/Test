Shader "Custom/2DGooEffect"
{
    Properties
    {
        _MainTex ("主纹理", 2D) = "white" {}
        _GooColor ("粘液颜色", Color) = (0.3, 0.8, 0.2, 1)
        _EdgeWidth ("边缘宽度", Range(0.01, 1)) = 0.1
        _FlowSpeed ("流动速度", Range(0, 2)) = 0.5
        _FlowIntensity ("流动强度", Range(0, 1)) = 0.3
        _NoiseScale ("噪声缩放", Float) = 8.0
        _GooThickness ("粘液厚度", Range(0.1, 2)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

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
            float4 _GooColor;
            float _EdgeWidth;
            float _FlowSpeed;
            float _FlowIntensity;
            float _NoiseScale;
            float _GooThickness;

            // 快速噪声函数
            float noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            // 平滑噪声（多频叠加）
            float smoothNoise(float2 uv)
            {
                float2 iuv = floor(uv);
                float2 fuv = frac(uv);
                
                // 双线性插值
                float a = noise(iuv);
                float b = noise(iuv + float2(1.0, 0.0));
                float c = noise(iuv + float2(0.0, 1.0));
                float d = noise(iuv + float2(1.0, 1.0));
                
                float2 u = fuv * fuv * (3.0 - 2.0 * fuv);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            // 计算边缘强度（核心函数）
            float getGooEdge(float2 uv, float time)
            {
                // 基础边缘计算
                float2 centerUV = abs(uv - 0.5);
                float edgeDist = max(centerUV.x, centerUV.y);
                float baseEdge = smoothstep(0.5 - _EdgeWidth, 0.5, edgeDist);
                // 流动效果 - 使用噪声扭曲边缘
                float2 flowUV = uv * _NoiseScale;
                flowUV.y += time * _FlowSpeed; // 垂直流动
                
                float flowNoise = smoothNoise(flowUV) * _FlowIntensity;
                
                // 结合基础边缘和流动效果
                float animatedEdge = baseEdge * (1.0 + flowNoise);
                
                return saturate(animatedEdge * _GooThickness);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 基础纹理颜色
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                
                // 计算粘液效果
                float gooIntensity = getGooEdge(i.uv, _Time.y);
                fixed4 finalColor=baseColor+_GooColor;
                
                // 边缘部分增加透明度变化（模拟粘液光泽）
                float alphaVariation = 0.2 + sin(_Time.y * 3.0 + gooIntensity * 10.0) * 0.2;
                finalColor.a = lerp(baseColor.a, alphaVariation, gooIntensity);
                return finalColor;
            }
            ENDCG
        }
    }
}