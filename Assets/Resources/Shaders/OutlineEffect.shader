Shader "FI/OutlineEffect"
{
	SubShader
	{
		// Declared for post-processing
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex, _CameraDepthTexture;
			float4 _CameraDepthTexture_TexelSize;

			fixed4 _OutlineColor;
			float threshold, width, offset;

			static const float2 coordXR = float2(1, 0);
			static const float2 coordXL = float2(-1, 0);
			static const float2 coordYU = float2(0, 1);
			static const float2 coordYD = float2(0, -1);

			// -------------------------------------------
			v2f vert(appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			// --------------------------------------------------------------
			float4 SampleDepth(v2f i, float2 coord, float depth)
			{
				return depth - LinearEyeDepth(
					SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv + coord));
			}
			
			// ------------------------------------------------------
			float4 frag (v2f i) : SV_Target
			{
				float txSize = _CameraDepthTexture_TexelSize.x;
				float tySize = _CameraDepthTexture_TexelSize.y;

				float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(
					_CameraDepthTexture, i.uv));

				float col =
					  SampleDepth(i, coordXR * txSize * offset, depth)
					+ SampleDepth(i, coordXL * txSize * offset, depth)
					+ SampleDepth(i, coordYU * tySize * offset, depth)
					+ SampleDepth(i, coordYD * tySize * offset, depth);

				col = max(0, col * width + threshold);

				return col * _OutlineColor;
			}

			ENDCG
		}
	}
}
