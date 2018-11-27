using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectsBase : MonoBehaviour {

	protected void CheckResources()
	{
		bool isSupport = CheckSupport();
		if (isSupport==false)
		{
			NotSupport();
		}
	}

	protected bool CheckSupport()
	{
		if (SystemInfo.supportsImageEffects==false||SystemInfo.supportsRenderTextures==false)
		{
			return false;
		}
		return true;
	}

	protected void NotSupport()
	{
		enabled = false;
	}

	

	protected Material checkShaderAndCreateMaterial(Shader shader,Material material)
	{
		if (shader==null)
		{
			return null;
		}

		if (shader.isSupported && material && material.shader == shader) {
			return material;
		}

		if (!shader.isSupported)
		{
			return null;
		}
		else
		{
			material=new Material(shader);
			//hideFlags：控制对象销毁的位掩码
			//HideFlags.DontSave对象不会保存到场景中。加载新场景时不会被破坏
			material.hideFlags = HideFlags.DontSave;
			return material;
		}
	}
	void Start()
	{
		CheckResources();
	}
	
	
}
