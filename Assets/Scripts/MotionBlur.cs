using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionBlur : PostEffectsBase {
	public Shader motionBlurShader;
	private Material motionBlurMaterial;

	public Material material
	{
		get
		{
			motionBlurMaterial = checkShaderAndCreateMaterial(motionBlurShader, motionBlurMaterial);
			return motionBlurMaterial;
		}
	}  
	//运动模糊在混合图像时使用的模糊参数
	[Range(0.0f, 0.9f)]
	public float blurAmount = 0.5f;

	private RenderTexture accumulationTexture;
	
	void OnDisable()
	{
		DestroyImmediate(accumulationTexture);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (material != null)
		{
			if (accumulationTexture==null||accumulationTexture.width!=src.width||accumulationTexture.height!=src.height)
			{
				DestroyImmediate(accumulationTexture);
				accumulationTexture=new RenderTexture(src.width,src.height,0);
				accumulationTexture.hideFlags = HideFlags.HideAndDontSave;
				//这里由于自己控制该变量的销毁，因此将hideFlags设置为HideAndDontSave，意味着该变量
				//不会显示在Hierarchy,也不会保存到场景中
				Graphics.Blit(src, accumulationTexture);
			}
			accumulationTexture.MarkRestoreExpected();
			//这里使用MarkRestoreExpected()方法表明需要进行一个渲染纹理的恢复操作
			//恢复操作发生在渲染到纹理而该纹理没有被提前清空或销毁的情况下，每次调用OnRenderImage()时需要把当前
			//的帧图像和accumulationTexture中的图像混合，accumulationTexture不需要提前清空，因为它保存了之前的混合结果  

			material.SetFloat("_BlurAmount", 1.0f - blurAmount);
			Graphics.Blit(src, accumulationTexture, material);
			Graphics.Blit(accumulationTexture, dest);
		}
		else
		{
			Graphics.Blit(src,dest);
		}
	}
}
