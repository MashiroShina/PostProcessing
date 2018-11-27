Shader "Custom/Chapter12_MotionBlur" {
Properties{
	_MainTex("Maintex",2D)="white"{}
	_BlurAmount("BlurAmount",Float)=1.0
}
SubShader{
	CGINCLUDE
		#include "UnityCG.cginc"  
		
		sampler2D _MainTex;
		float _BlurAmount;

		struct v2f{
			float4 pos:SV_POSITION;
			half2 uv:TEXCOORD0;
		};

		v2f vert(appdata_img v){
			v2f o;
			o.pos=UnityObjectToClipPos(v.vertex);
			o.uv=v.texcoord;

			return o;
		}

		//对当前图像进行采样，将其A通道的值设为_BlurAmount,在后续混合时可以使用透明通道进行混合
		fixed4 fragRGB(v2f i):SV_Target{
			return fixed4(tex2D(_MainTex,i.uv).rgb,_BlurAmount);
		}

		//直接返回当前图像的采样结果，维护渲染纹理的透明通道值，不让其受到混合时使用的透明度值的影响
		half4 fragA(v2f i):SV_Target{
			return tex2D(_MainTex,i.uv);
		}		
	ENDCG

	ZTest Always
	Cull Off
	ZWrite Off
	//这里将RGB和A通道分开，是由于在做混合时，按照_BlurAmount参数值将源图像和目标图像进行混合
	//而同时不让其纹理受到A通道值的影响，只是用来做混合，不改变其透明度
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask RGB
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragRGB
			ENDCG
		}
		Pass{
			Blend One Zero
			ColorMask A
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment fragA
			ENDCG
		}
}
}