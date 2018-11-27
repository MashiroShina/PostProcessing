using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBlur : PostEffectsBase {

	public Shader gaussianBlurShader;
	private Material gaussianBlurMaterial = null;
	
	public Material material
	{
		get
		{
			gaussianBlurMaterial = checkShaderAndCreateMaterial(gaussianBlurShader, gaussianBlurMaterial);
			return gaussianBlurMaterial;
		}
	}
	[Range(0, 4)]   //迭代次数，值越大，模糊应用次数越高
	public int iterations = 3;
	[Range(0.2f, 3.0f)] //模糊计算的范围，越大越模糊    
	public float blurSpread = 0.6f;
	[Range(1, 8)] //降采样数值，越大，计算的像素点越少，节约性能，但是降采样的值太大会出现像素化风格
	public int downSample = 2;

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (material!=null)
		{
			int rtW = src.width/downSample;
			int rtH = src.height/downSample;
			
			//获取临时渲染纹理
			RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
			buffer0.filterMode = FilterMode.Bilinear;
			
			Graphics.Blit(src, buffer0);
			for (int i = 0; i < iterations; i++)
			{
				material.SetFloat("_BlurSize", 1.0f + i*blurSpread);
				//_BlurSize用来控制采样的距离，在n次迭代下，每次迭代会计算向外一圈的采样结果，
				//再进行高斯核的横向和纵向的计算，结果也就会更加模糊
				RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
				Graphics.Blit(buffer0, buffer1, material, 0);
				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;
				buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
				Graphics.Blit(buffer0, buffer1, material, 1);
				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;

			}
			Graphics.Blit(buffer0, dest);
			RenderTexture.ReleaseTemporary(buffer0);
		}else
		{
			Graphics.Blit(src,dest);
		}
	}
}
