// by Dmitry Timofeev
// https://assetstore.unity.com/packages/vfx/shaders/fullscreen-camera-effects/pixelation-65554

Shader "Hidden/Pixelation"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;	
			float2 BlockCount;
			float2 BlockSize;

			fixed4 frag (v2f_img i) : SV_Target
			{
				float2 blockPos = floor(i.uv * BlockCount);
				float2 blockCenter = blockPos * BlockSize + BlockSize * 0.5;

				float4 tex = tex2D(_MainTex, blockCenter);
				return tex;
			}
			ENDCG
		}
	}
}
