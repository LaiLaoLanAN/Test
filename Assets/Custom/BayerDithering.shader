Shader "Custom/BayerDithering"
{
    Properties
    {
        _MainTex ("原图", 2D) = "white" {}
        _DitherIntensity ("抖动强度", Range(0, 1)) = 1.0
        _DitherScale ("抖动缩放", Range(0.1, 10)) = 1.0
        _Brightness ("亮度", Range(0, 2)) = 1.0
        _Contrast ("对比度", Range(0, 2)) = 1.0
        _Colorize ("色彩化", Color) = (1, 1, 1, 1)
        [Toggle]_ShowOriginal ("显示原图", Float) = 0
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
                float4 screenPos : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _DitherIntensity;
            float _DitherScale;
            float _Brightness;
            float _Contrast;
            float4 _Colorize;
            float _ShowOriginal;
            
            // 4x4 Bayer矩阵（经典抖动模式）
            static const float4x4 bayerMatrix4x4 = {
                0,  8,  2,  10,
                12, 4,  14, 6,
                3,  11, 1,  9,
                15, 7,  13, 5
            };
            
            // 8x8 Bayer矩阵（更精细的抖动）
            static const float bayerMatrix8x8[64] = {
                 0, 32,  8, 40,  2, 34, 10, 42,
                48, 16, 56, 24, 50, 18, 58, 26,
                12, 44,  4, 36, 14, 46,  6, 38,
                60, 28, 52, 20, 62, 30, 54, 22,
                 3, 35, 11, 43,  1, 33,  9, 41,
                51, 19, 59, 27, 49, 17, 57, 25,
                15, 47,  7, 39, 13, 45,  5, 37,
                63, 31, 55, 23, 61, 29, 53, 21
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            
            // 获取Bayer矩阵阈值（0-1范围）
            float getBayerThreshold(float2 position, int matrixSize)
            {
                if (matrixSize == 4)
                {
                    int x = fmod(position.x, 4);
                    int y = fmod(position.y, 4);
                    return bayerMatrix4x4[x][y] / 16.0;
                }
                else // 8x8
                {
                    int x = fmod(position.x, 8);
                    int y = fmod(position.y, 8);
                    return bayerMatrix8x8[x + y * 8] / 64.0;
                }
            }
            
            // RGB转亮度（心理学权重）
            float rgbToLuminance(float3 rgb)
            {
                return dot(rgb, float3(0.299, 0.587, 0.114));
            }
            
            // 应用亮度和对比度
            float adjustBrightnessContrast(float value, float brightness, float contrast)
            {
                // 先调整亮度
                value = value * brightness;
                // 再调整对比度
                value = (value - 0.5) * contrast + 0.5;
                return saturate(value);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // 采样原图
                fixed4 originalColor = tex2D(_MainTex, i.uv);
                
                if (_ShowOriginal > 0.5)
                    return originalColor;
                
                // 转换为灰度
                float luminance = rgbToLuminance(originalColor.rgb);
                
                // 调整亮度和对比度
                luminance = adjustBrightnessContrast(luminance, _Brightness, _Contrast);
                
                // 计算屏幕空间位置（用于抖动）
                float2 screenPos = i.screenPos.xy / i.screenPos.w * _ScreenParams.xy;
                screenPos /= _DitherScale;
                
                // 获取Bayer阈值
                float threshold = getBayerThreshold(screenPos, 8); // 使用8x8矩阵
                
                // Bayer抖动：比较亮度与阈值
                float dithered = step(threshold, luminance);
                
                // 混合原图和抖动结果
                float finalLuminance = lerp(luminance, dithered, _DitherIntensity);
                
                // 应用色彩化
                fixed3 finalColor = finalLuminance * _Colorize.rgb;
                
                return fixed4(finalColor, originalColor.a);
            }
            ENDCG
        }
    }
}