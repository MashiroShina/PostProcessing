using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BrightnessSaturationAndContrast : PostEffectsBase
{
 //亮度 对比度 饱和度 参数
 [Range(0.0f, 3.0f)] public float brightness = 1.0f;
 [Range(0.0f, 3.0f)] public float contrast = 1.0f;
 [Range(0.0f, 3.0f)] public float saturation = 1.0f;

 public Shader m_shader;
 private Material m_material;

 public Material material
 {
  get
  {
   m_material = checkShaderAndCreateMaterial(m_shader, m_material);
   return m_material;
  }
 }
 //定义OnRenderImage函数来进特效处理
 //当OnRenderImage函数被调用的时候，他会检查材质球是否可用。如果可用，就把参数传递给材质，
 //再调用Graphics.Blit进行处理；否则，直接把原图像显示到屏幕上，不做任何处理。
 void OnRenderImage(RenderTexture src, RenderTexture dest)
 {
  if (material!=null)
  {
   material.SetFloat("_Brightness", brightness);
   material.SetFloat("_Saturation", saturation);
   material.SetFloat("_Contrast", contrast);
   Graphics.Blit(src,dest,material);
  }
  else
  {
   Graphics.Blit(src,dest);
  }
 }
}
