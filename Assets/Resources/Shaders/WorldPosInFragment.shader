Shader "FI/WorldPosInFragment"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
	}

	SubShader
	{
		// Declared for post-processing
		ZWrite Off
		ZTest Always
		Cull Off

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 ray : TEXCOORD1;
			};

			float _Radius;
			float3 _FrustumCorners[4];
			float4 _InRadiusColor;

			sampler2D _MainTex, _CameraDepthTexture;
			
			v2f vert (appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.ray = _FrustumCorners[o.uv.x + 2 * o.uv.y];
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = float4(0, 0, 0, 0);
				float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv));

				float3 viewDistance = i.ray * depth;
				float3 worldPos = _WorldSpaceCameraPos.xyz + viewDistance;

				float dst = distance(worldPos, _WorldSpaceCameraPos.xyz);

				if (dst >= _Radius)
				{
					col = tex2D(_MainTex, i.uv);
				}
				else if (dst < _Radius && dst >= _Radius - _Radius * 0.025)
				{
					col = float4(0, 0, 0, 0);
				}
				else if (dst < _Radius - _Radius * 0.025)
				{
					col = tex2D(_MainTex, i.uv);
					float grayScale = dot(col.rgb, float3(0.299, 0.587, 0.114));
					col = grayScale * _InRadiusColor;
				}

				return col;
			}

			ENDCG
		}
	}
}
