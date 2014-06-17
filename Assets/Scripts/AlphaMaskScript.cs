using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class AlphaMaskScript : MonoBehaviour
{
	public Texture2D clipTexture;
	public float scale;
	public int xOffsetPixel;
	public int yOffsetPixel;

	public string ToName()
	{
		return string.Format(
			"{0} xy={1}x{2} scale={3}",
			name,
			xOffsetPixel,
			yOffsetPixel,
			scale
		);
	}
}