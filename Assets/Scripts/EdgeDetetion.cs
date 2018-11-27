using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetetion : PostEffectsBase {

//	float _EdgeOnly;
//	fixed4 _EdgeColor;
//	fixed4 _BackgroundColor;
    [Range(0.0f, 3.0f)] public float EdgeOnly = 1.0f;
    public Color EdgeColor;
    public Color32 BackgroundColor;
    
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
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material!=null)
        {
            material.SetFloat("_EdgeOnly", EdgeOnly);
            material.SetColor("_EdgeColor", EdgeColor);
            material.SetColor("_BackgroundColor", BackgroundColor);
            Graphics.Blit(src,dest,material);
        }
        else
        {
            Graphics.Blit(src,dest);
        }
    }
}
