Shader "Custom/Chapter12_BrightnessSaturateAndContrast"
{
	Properties
	{
		_MainTex("Maintex",2D)="white"{}
	    _Brightness("Brightness",Float)=1
	    _Saturation("Saturation",Float)=1
	    _Contrast("Contrast",Float)=1
	     //Graphics.Blit(src,dest,material)会将第一个参数传递给Shader中名为_MainTex的属性
	}
	SubShader
	{
		Pass
		{
		    ZTest Always
		    Cull Off
		    ZWrite Off
			
			CGPROGRAM
			#pragma vertex vert
		    #pragma fragment frag
		    #include "UnityCG.cginc"

		    sampler2D _MainTex;
		    float4 _MainTex_ST;
		    half _Brightness;
		    half _Saturation;
		    half _Contrast;

			struct v2f
			{
				float4 pos:SV_POSITION;
			    float2 uv:TEXCOORD0;
			};
			
			v2f vert (appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed4 renderTex=tex2D(_MainTex,i.uv);
                
                //应用明亮度
                fixed3 finalColor=renderTex.rgb*_Brightness;
                //应用饱和度，乘以一定系数
			    fixed luminance=0.2125*renderTex.r+0.7154*renderTex.g+0.0721*renderTex.b;
			    fixed3 luminanceColor=fixed3(luminance,luminance,luminance);
			    finalColor=lerp(luminanceColor,finalColor,_Saturation);
			    //应用对比度
    			fixed3 avgColor=fixed3(0.5,0.5,0.5);
	    		finalColor=lerp(avgColor,finalColor,_Contrast);
                
				return fixed4(finalColor,renderTex.a);
			}
			ENDCG
		}
	}
	FallBack Off
}
