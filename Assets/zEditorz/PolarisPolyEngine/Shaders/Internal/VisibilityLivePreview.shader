Shader "Hidden/Griffin/VisibilityLivePreview" 
{
	Properties 
	{
		_Color ("Line Color", Color) = (1,1,1,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_Thickness ("Thickness", Float) = 1

		_HeightMap ("Height Map", 2D) = "black" {}
		_Dimension ("Dimension", Vector) = (0,0,0,0)

		_SubdivMap ("Subdiv Map", 2D) = "black" {}
		_SubdivRange("Subdiv Range", Vector) = (0,0,0,0)

		_Mask ("Mask", 2D) = "white" {}
	}

	SubShader 
	{
		Pass
		{
			Tags { "RenderType"="AlphaTest" "Queue"="Overlay" }

			Blend SrcAlpha OneMinusSrcAlpha 
			ZWrite Off
			ZTest Always
			LOD 200
			
			CGPROGRAM
				#pragma target 5.0
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag
				#pragma geometry geom
				#pragma multi_compile _ USE_MASK

				struct v2g
				{
					float4	pos		: POSITION;		// vertex position
					float2  uv		: TEXCOORD0;	// vertex uv coordinate
					float2  heightMapUv : TEXCOORD1;
					float4  worldPos : TEXCOORD2;
				};

				struct g2f
				{
					float4	pos		: POSITION;		// fragment position
					float2	uv		: TEXCOORD0;	// fragment uv coordinate
					float2  heightMapUv : TEXCOORD2;
					float4  worldPos : TEXCOORD3;
					float3  dist	: TEXCOORD1;	// distance to each edge of the triangle
				};

				float _Thickness = 1;		// Thickness of the wireframe line rendering
				float4 _Color = {1,1,1,1};	// Color of the line
				float4 _MainTex_ST;			// For the Main Tex UV transform
				sampler2D _MainTex;			// Texture used for the line
				sampler2D _HeightMap;
				float4 _Dimension;			
				float4 _BoundMin;
				float4 _BoundMax;
				sampler2D _SubdivMap;
				float4 _SubdivRange;
				sampler2D _Mask;
				float4x4 _WorldPointToMask;

				v2g vert(appdata_base v)
				{
					v2g output;

					float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
					float4 heightMapUv = float4(
						(worldPos.x-_BoundMin.x)/(_BoundMax.x-_BoundMin.x),
						(worldPos.z-_BoundMin.z)/(_BoundMax.z-_BoundMin.z),
						0, 0
					);
					float newHeight = tex2Dlod(_HeightMap, heightMapUv).r*_Dimension.y;
					v.vertex.y = newHeight;
					
					output.pos =  UnityObjectToClipPos(v.vertex);
					output.uv = TRANSFORM_TEX (v.texcoord, _MainTex);//v.texcoord;
					output.heightMapUv = heightMapUv;
					//output.maskUv = mul(_WorldPointToMask, worldPos).xz;
					output.worldPos = worldPos;

					return output;
				}

				[maxvertexcount(3)]
				void geom(triangle v2g p[3], inout TriangleStream<g2f> triStream)
				{
					//points in screen space
					float2 p0 = _ScreenParams.xy * p[0].pos.xy / p[0].pos.w;
					float2 p1 = _ScreenParams.xy * p[1].pos.xy / p[1].pos.w;
					float2 p2 = _ScreenParams.xy * p[2].pos.xy / p[2].pos.w;
	
					//edge vectors
					float2 v0 = p2 - p1;
					float2 v1 = p2 - p0;
					float2 v2 = p1 - p0;

					//area of the triangle
 					float area = abs(v1.x*v2.y - v1.y * v2.x);

					//values based on distance to the edges
					float dist0 = area / length(v0);
					float dist1 = area / length(v1);
					float dist2 = area / length(v2);
	
					g2f pIn;
	
					//add the first point
					pIn.pos = p[0].pos;
					pIn.uv = p[0].uv;
					pIn.heightMapUv = p[0].heightMapUv;
					//pIn.maskUv = p[0].maskUv;
					pIn.worldPos = p[0].worldPos;
					pIn.dist = float3(dist0,0,0);
					triStream.Append(pIn);

					//add the second point
					pIn.pos =  p[1].pos;
					pIn.uv = p[1].uv;
					pIn.heightMapUv = p[1].heightMapUv;
					//pIn.maskUv = p[1].maskUv;
					pIn.worldPos = p[1].worldPos;
					pIn.dist = float3(0,dist1,0);
					triStream.Append(pIn);
	
					//add the third point
					pIn.pos = p[2].pos;
					pIn.uv = p[2].uv;
					pIn.heightMapUv = p[2].heightMapUv;
					//pIn.maskUv = p[2].maskUv;
					pIn.worldPos = p[2].worldPos;
					pIn.dist = float3(0,0,dist2);
					triStream.Append(pIn);
				}

				float4 frag(g2f input) : COLOR
				{	
					#if USE_MASK
					float2 maskUv = mul(_WorldPointToMask, input.worldPos).xz;
					if (maskUv.x < 0) discard;
					if (maskUv.x > 1) discard;
					if (maskUv.y < 0) discard;
					if (maskUv.y > 1) discard;
					#endif

					float visibilityValue = tex2D(_HeightMap, input.heightMapUv.xy).a;
					if (visibilityValue >= 0.5) 
						discard;

					//find the smallest distance
					float val = min( input.dist.x, min( input.dist.y, input.dist.z));
	
					//calculate power to 2 to thin the line
					val = exp2( -1/_Thickness * val * val );
		
					//blend between the lines and the negative space to give illusion of anti aliasing
					float4 targetColor = _Color * tex2D( _MainTex, input.uv);
					float4 transCol = _Color * tex2D( _MainTex, input.uv);
					transCol.a = 0;
					float4 col = val * targetColor + ( 1 - val ) * transCol;

					float subdivValue = tex2D(_SubdivMap, input.heightMapUv.xy).r;

					#if USE_MASK
					float maskValue = tex2D(_Mask, maskUv).r;
					#else
					float maskValue = 1;
					#endif

					col.a = col.a*maskValue*(subdivValue >= _SubdivRange.x)*(subdivValue <= _SubdivRange.y);

					return col;
				}
			
			ENDCG
		}
	} 
}
