// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Chapter12_EdgeDetetion"
{
Properties{
	_MainTex("MainTex",2D)="white"{}
	_EdgeOnly("Edge Only",Float)=1.0
	_EdgeColor("Edge Color",Color)=(0,0,0,1)
	_BackgroundColor("BackgroundColor",Color)=(1,1,1,1)
}
SubShader{
	Pass{
		ZTest Always
		Cull Off
		ZWrite Off

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST;
		half4 _MainTex_TexelSize;
		float _EdgeOnly;
		fixed4 _EdgeColor;
		fixed4 _BackgroundColor;

		struct v2f{
			float4 pos:SV_POSITION;
			half2 uv[9]:TEXCOORD0;
		};

		v2f vert(appdata_img v){
			v2f o;
			o.pos=UnityObjectToClipPos(v.vertex);
			half2 uv=v.texcoord;
			//中线像素点及周围8个像素点的UV坐标
			o.uv[0]=uv+_MainTex_TexelSize.xy*half2(-1,-1);
			o.uv[1]=uv+_MainTex_TexelSize.xy*half2(0,-1);
			o.uv[2]=uv+_MainTex_TexelSize.xy*half2(1,-1);
			o.uv[3]=uv+_MainTex_TexelSize.xy*half2(-1,0);
			o.uv[4]=uv+_MainTex_TexelSize.xy*half2(0,0);
			o.uv[5]=uv+_MainTex_TexelSize.xy*half2(1,0);
			o.uv[6]=uv+_MainTex_TexelSize.xy*half2(-1,1);
			o.uv[7]=uv+_MainTex_TexelSize.xy*half2(0,1);
			o.uv[8]=uv+_MainTex_TexelSize.xy*half2(1,1);
		
			return o;
		}

		fixed luminance(fixed4 color){
			return color.r*0.2125+color.g*0.7154+color.b*0.0721;
		}
		half Sobel(v2f i){
			const half Gx[9]={-1,-2,-1,
										0,0,0,
										1,2,1};
			const half Gy[9]={-1,0,1,
										-2,0,2,
										-1,0,1};
			half texColor;
			half edgeX;
			half edgeY;
			for(int it=0;it<9;it++){
				texColor=luminance(tex2D(_MainTex,i.uv[it]));
				edgeX+=texColor*Gx[it];
				edgeY+=texColor*Gy[it];
			}
			half edge=1-abs(edgeX)-abs(edgeY);

			return edge;
		}

		fixed4 frag(v2f i):SV_Target{
			half edge=Sobel(i);

			fixed4 withEdgeColor=lerp(_EdgeColor,tex2D(_MainTex,i.uv[4]),edge);
			fixed4 withBackGroundColor=lerp(_EdgeColor,_BackgroundColor,edge);
			//非边缘的像素点withEdgeColor插值结果靠近原有颜色
			//而withBackGroundColor的插值结果就靠近给定的背景色
			return lerp(withEdgeColor,withBackGroundColor,_EdgeOnly);
			//最后的返回结果，通过_EdgeOnly来混合，
			//来控制处理后的图像非边缘的颜色是靠近原颜色还是给定的背景色
		}
		ENDCG
	}
}
FallBack Off
}
