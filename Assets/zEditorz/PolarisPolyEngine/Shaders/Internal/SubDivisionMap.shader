Shader "Hidden/Griffin/SubDivisionMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Epsilon ("Epsilon", Float) = 0.01
		_PixelOffset ("Pixel Offset", Int) = 2
		_Step ("Step", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float _Epsilon;
			int _PixelOffset;
			float _Step;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 texel = _MainTex_TexelSize.xy*_PixelOffset;
				float4 centerColor = tex2D(_MainTex, i.uv);
				float center = centerColor.r;
				float top = tex2D(_MainTex, i.uv + float2(0,texel.y)).r;
				float bottom = tex2D(_MainTex, i.uv + float2(0,-texel.y)).r;
				float left = tex2D(_MainTex, i.uv + float2(-texel.x,0)).r;
				float right = tex2D(_MainTex, i.uv + float2(texel.x,0)).r;
				float bottomLeft = tex2D(_MainTex, i.uv + float2(-texel.x, -texel.y)).r;
				float topLeft = tex2D(_MainTex, i.uv + float2(-texel.x, texel.y)).r;
				float topRight = tex2D(_MainTex, i.uv + float2(texel.x, texel.y)).r;
				float bottomRight = tex2D(_MainTex, i.uv + float2(texel.x, -texel.y)).r;
				float step = _Step;

				float value = 0;
				//compare offset
				value += (abs(center-top)>=_Epsilon)*step;
				value += (abs(center-bottom)>=_Epsilon)*step;
				value += (abs(center-left)>=_Epsilon)*step;
				value += (abs(center-right)>=_Epsilon)*step;
				value += (abs(center-bottomLeft)>=_Epsilon)*step;
				value += (abs(center-topLeft)>=_Epsilon)*step;
				value += (abs(center-topRight)>=_Epsilon)*step;
				value += (abs(center-bottomRight)>=_Epsilon)*step;

				//G channel for additional sub div
				value += centerColor.g;
				//A channel for visibility/holes
				value += centerColor.a;
				value = saturate(value);

                return float4(value, value, value, step);
            }
            ENDCG
        }
    }
}
