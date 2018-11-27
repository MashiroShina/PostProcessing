using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloom : PostEffectsBase {
	public Shader bloomShader;
	private Material bloomMaterial;
	public Material material {
		get { bloomMaterial = checkShaderAndCreateMaterial(bloomShader, bloomMaterial);
			return bloomMaterial;
		}
	}
	[Range(0, 4)]   //迭代次数，值越大，模糊应用次数越高
	public int iterations = 3;
	[Range(0.2f, 3.0f)] //模糊计算的范围，越大越模糊    
	public float blurSpread = 0.6f;
	[Range(1, 8)] //降采样数值，越大，计算的像素点越少，节约性能，但是降采样的值太大会出现像素化风格
	public int downSample = 2;
	[Range(0.0f, 4.0f)]
	public float luminanceThreshold = 0.6f;
	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (material != null)
		{
			material.SetFloat("_LuminanceThreshold", luminanceThreshold);

			int rtW = src.width / downSample;
			int rtH = src.height / downSample;

			RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
			buffer0.filterMode=FilterMode.Bilinear;
			
			Graphics.Blit(src, buffer0, material, 0);

			for (int it = 0; it < iterations; it++)
			{
				material.SetFloat("_BlurSize", 1.0f + it * blurSpread);
				RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
				Graphics.Blit(buffer0, buffer1, material, 1);
				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;
				buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
				Graphics.Blit(buffer0, buffer1, material, 2);
				RenderTexture.ReleaseTemporary(buffer0);
				buffer0 = buffer1;
			}
			material.SetTexture("_BloomTex", buffer0);
			Graphics.Blit(src, dest, material, 3);
			RenderTexture.ReleaseTemporary(buffer0);
		}
		else {
			Graphics.Blit(src,dest);
		}
	}
}
