﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Kuwahara"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Radius ("Size of the Strokes", Range(0, 50)) = 3
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform int _Radius;
            float2 textureSize;

            struct v2f {
                float4 position : POSITION;
                float4 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            v2f_img vert(appdata_base v) {
                v2f_img o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            struct region {
                int x1, y1, x2, y2;
            };

            float4 _MainTex_TexelSize;

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float n = float((_Radius + 1) * (_Radius + 1));
                float4 col = tex2D(_MainTex, uv);

                float3 m[4];
                float3 s[4];

                for (int k = 0; k < 4; ++k) {
                    m[k] = float3(0, 0, 0);
                    s[k] = float3(0, 0, 0);
                }

                region R[4] = {
                    {-_Radius, -_Radius,       0,       0},
                    {       0, -_Radius, _Radius,       0},
                    {       0,        0, _Radius, _Radius},
                    {-_Radius,        0,       0, _Radius}
                };

                for (int l = 0; l < 4; ++l) {
                    for (int j = R[l].y1; j <= R[l].y2; ++j) {
                        for (int i = R[l].x1; i <= R[l].x2; ++i) {
                            float3 c = tex2D(_MainTex, uv + (float2(i * _MainTex_TexelSize.x, j * _MainTex_TexelSize.y))).rgb;
                            m[l] += c;
                            s[l] += c * c;
                        }
                    }
                }

                float min = 1e+2;
                float s2;
                for (k = 0; k < 4; ++k) {
                    m[k] /= n;
                    s[k] = abs(s[k] / n - m[k] * m[k]);

                    s2 = s[k].r + s[k].g + s[k].b;
                    if (s2 < min) {
                        min = s2;
                        col.rgb = m[k].rgb;
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}