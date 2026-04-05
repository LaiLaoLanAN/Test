Shader "Custom/SmoothCrackWaveEffect"
{
    Properties
    {
        _MainTex ("原图", 2D) = "white" {}
        _GridSize ("方格大小", Range(0.000001, 0.2)) = 0.05
        _CrackWidth ("裂缝宽度", Range(0.000001, 0.02)) = 0.005
        _CrackColor ("裂缝颜色", Color) = (0.1, 0.1, 0.1, 1)
        _WaveDirection ("裂隙波方向", Vector) = (1, 0, 0, 0)
        _WaveWidth ("裂隙波宽度", Range(0.1, 3.0)) = 1.0
        _WaveIntensity ("裂隙波强度", Range(0, 1)) = 0.8
        _WaveProgress ("裂隙波进度", Range(-1, 2)) = -1.0
        _EdgeSoftness ("边缘柔化", Range(0, 0.1)) = 0.02
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
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
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            float _GridSize;
            float _CrackWidth;
            float4 _CrackColor;
            float2 _WaveDirection;
            float _WaveWidth;
            float _WaveIntensity;
            float _WaveProgress;
            float _EdgeSoftness;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }
            // 计算平滑的裂隙波
            float calculateSmoothCrackWave(float2 uv, float2 direction, float progress)
            {
                float2 dir = normalize(direction);
                float pos = dot(uv, dir);
                
                // 裂隙波位置（从-0.5到1.5）
                float waveCenter;
                if(direction.x<0){
                    waveCenter = (1.2 + 2.0 * _WaveWidth) * progress - _WaveWidth-0.1;
                }
                else{
                    waveCenter = (0.94 + 2.0 * _WaveWidth) * progress - _WaveWidth+0.03;
                }
                // 裂隙波范围
                float waveStart = waveCenter - _WaveWidth * 0.5;
                float waveEnd = waveCenter + _WaveWidth * 0.5;
                
                // 平滑的边缘过渡
                float wave = smoothstep(waveStart, waveStart + _EdgeSoftness, pos) * 
                           (1.0 - smoothstep(waveEnd - _EdgeSoftness, waveEnd, pos));
                
                return wave * _WaveIntensity;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 originalColor = tex2D(_MainTex, i.uv);
                
                if (_WaveProgress < 0)
                    return originalColor;
                
                if (originalColor.a < 0.01)
                    return fixed4(0, 0, 0, 0);
                
                // 计算裂隙波强度
                float waveStrength = calculateSmoothCrackWave(i.uv, _WaveDirection, _WaveProgress);
                
                if (waveStrength < 0.01)
                    return originalColor;
                    
                float2 texelSize = 1.0 / _MainTex_TexelSize.zw;
                float aspectRatio = texelSize.x / texelSize.y;
                
                // 修正GridSize的宽高比
                float2 adjustedGridSize = float2(_GridSize * aspectRatio, _GridSize);
                // 方格计算
                float2 gridID = floor(i.uv / adjustedGridSize);
                float2 localUV = frac(i.uv / adjustedGridSize);
                float2 centerUV = (gridID + 0.5) * adjustedGridSize;
                
                fixed4 centerColor = tex2D(_MainTex, centerUV);
                if (centerColor.a < 0.01)
                    return originalColor;
                
                // 裂缝计算（带平滑）
                float crack = 0.0;
                float edgeWidth = _CrackWidth;
                
                float2 offset = float2(
                    hash(gridID + float2(13.5, 27.3)) * 2.0 - 1.0,
                    hash(gridID + float2(45.6, 89.1)) * 2.0 - 1.0
                ) * 0.2;
                
                float2 randomizedUV = localUV + offset;


                crack = max(crack, step(1.0 - edgeWidth, randomizedUV.x));
                crack = max(crack, step(1.0 - edgeWidth, randomizedUV.y));
                crack = max(crack, step(randomizedUV.x,edgeWidth));
                crack = max(crack, step(randomizedUV.y,edgeWidth));
                
                // 关键修改：有裂缝的部分直接透明
                if (crack > 0.5)
                {
                    return fixed4(0, 0, 0, 0);
                }

                // 最终颜色
                // float3 gridColor = lerp(centerColor.rgb, _CrackColor.rgb, crack);
                float3 gridColor=centerColor.rgb;
                float3 finalColor = lerp(originalColor.rgb, gridColor,waveStrength);
                
                return fixed4(finalColor, originalColor.a);
            }
            ENDCG
        }
    }
}