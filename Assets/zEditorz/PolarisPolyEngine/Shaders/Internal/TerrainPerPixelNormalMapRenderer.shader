Shader "Hidden/Griffin/TerrainNormalMapRenderer"
{
    Properties
    {
		_HeightMap("Height Map", 2D) = "black" {}
		_Width("Width", Float) = 0
		_Height("Height", Float) = 0
		_Length("Length", Float) = 0
		_TangentSpace("Tangent Space", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
		Blend One Zero

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
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
            };

			sampler2D _HeightMap;
			float4 _HeightMap_TexelSize;
			float _Width;
			float _Height;
			float _Length;
			int _TangentSpace;
			
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
				float2 texel = _HeightMap_TexelSize.xy;
				float2 uvLeft = i.uv - float2(texel.x, 0);
				float2 uvUp = i.uv + float2(0, texel.y);
				float2 uvRight =  i.uv + float2(texel.x, 0);
				float2 uvDown = i.uv - float2(0, texel.y);
				float2 uvCenter = i.uv;
				float2 uvLeftUp = i.uv - float2(texel.x, 0) + float2(0, texel.y);
				float2 uvUpRight = i.uv + float2(0, texel.y) + float2(texel.x, 0);
				float2 uvRightDown = i.uv + float2(texel.x, 0) - float2(0, texel.y);
				float2 uvDownLeft = i.uv - float2(0, texel.y) - float2(texel.x, 0);

				float leftHeight = tex2D(_HeightMap, uvLeft).r*_Height;
				float upHeight = tex2D(_HeightMap, uvUp).r*_Height;
				float rightHeight = tex2D(_HeightMap, uvRight).r*_Height;
				float downHeight = tex2D(_HeightMap, uvDown).r*_Height;
				float centerHeight = tex2D(_HeightMap, uvCenter).r*_Height;
				float leftUpHeight = tex2D(_HeightMap, uvLeftUp).r*_Height;
				float upRightHeight = tex2D(_HeightMap, uvUpRight).r*_Height;
				float rightDownHeight = tex2D(_HeightMap, uvRightDown).r*_Height;
				float downLeftHeight = tex2D(_HeightMap, uvDownLeft).r*_Height;

				float3 left = float3(_Width*uvLeft.x, leftHeight, _Length*uvLeft.y);
				float3 up = float3(_Width*uvUp.x, leftHeight, _Length*uvUp.y);
				float3 right = float3(_Width*uvRight.x, leftHeight, _Length*uvRight.y);
				float3 down = float3(_Width*uvDown.x, leftHeight, _Length*uvDown.y);
				float3 center = float3(_Width*uvCenter.x, centerHeight, _Length*uvCenter.y);
				float3 leftUp = float3(_Width*uvLeftUp.x, leftUpHeight, _Length*uvLeftUp.y);
				float3 upRight = float3(_Width*uvUpRight.x, upRightHeight, _Length*uvUpRight.y);
				float3 rightDown = float3(_Width*uvRightDown.x, rightDownHeight, _Length*uvRightDown.y);
				float3 downLeft = float3(_Width*uvDownLeft.x, downLeftHeight, _Length*uvDownLeft.y);

				float3 n0 = cross(left-center, leftUp-center);
				float3 n1 = cross(up-center, upRight-center);
				float3 n2 = cross(right-center, rightDown-center);
				float3 n3 = cross(down-center, downLeft-center);

				float3 n4 = cross(leftUp-center, up-center);
				float3 n5 = cross(upRight-center, right-center);
				float3 n6 = cross(rightDown-center, down-center);
				float3 n7 = cross(downLeft-center, left-center);

				float3 nc = (n0+n1+n2+n3+n4+n5+n6+n7)/8;
				
				float3 n = _TangentSpace*float3(nc.x, nc.z, nc.y) + (1-_TangentSpace)*nc;
                float3 normal = normalize(n);

				float3 col = float3(
					(normal.x + 1)/2,
					(normal.y + 1)/2,
					(normal.z + 1)/2);
				#ifdef UNITY_COLORSPACE_GAMMA
				return float4(col,1);
				#else
				return float4(GammaToLinearSpace(col.rgb), 1);
				#endif
            }
            ENDCG
        }
    }
}
