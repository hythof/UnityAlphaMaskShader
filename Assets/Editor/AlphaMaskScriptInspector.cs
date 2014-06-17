using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AlphaMaskScript))]
public class AlphaMaskScriptInspector : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		var mask = target as AlphaMaskScript;
		var spriteRenderer = mask.GetComponent<SpriteRenderer>();

		if(check(spriteRenderer, mask))
		{
			updateSprite(spriteRenderer, mask);
		}
	}

	bool check(SpriteRenderer spriteRenderer, AlphaMaskScript mask)
	{
		if(mask.clipTexture == null)
		{
			GUILayout.Label("mask texture not found");
			return false;
		}
		if(spriteRenderer.sprite == null)
		{
			GUILayout.Label("render texture not found");
			return false;
		}
		var mat = spriteRenderer.sharedMaterial;
		if(mat.name == mask.ToName())
		{
			return false;
		}
		return true;
	}

	void updateSprite(SpriteRenderer spriteRenderer, AlphaMaskScript mask)
	{
		Texture2D a = mask.clipTexture;
		Texture2D b = spriteRenderer.sprite.texture;
		float w1 = (float)a.width;
		float h1 = (float)a.height;
		float w2 = (float)b.width;
		float h2 = (float)b.height;
		float xScale = h1 / h2;
		float yScale = w1 / w2;
		float adjustScale = xScale > yScale ? yScale : xScale;
		xScale = xScale / adjustScale / mask.scale;
		yScale = yScale / adjustScale / mask.scale;
		float xOffset = mask.xOffsetPixel / w2;
		float yOffset = mask.yOffsetPixel / h2;

		var mat = new Material(Shader.Find("Unlit/AlphaMask"));
		mat.name = mask.ToName();
		mat.SetTexture("_ClipTex", mask.clipTexture);
		mat.SetFloat("_xScale", xScale);
		mat.SetFloat("_yScale", yScale);
		mat.SetFloat("_xOffset", xOffset);
		mat.SetFloat("_yOffset", yOffset);

		spriteRenderer.sharedMaterial = mat;

		Debug.Log(string.Format(
			"update alpha mask target={0}x{1} mask={2}x{3}",
			w1,
			h1,
			w2,
			h2
		));
	}
}
