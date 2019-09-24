using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class tk2dColorsAdaptor : MonoBehaviour
{
	public Color color = Color.black;
	
	tk2dSprite[] sprites = null;
	tk2dTextMesh text = null;
	
	void Awake()
	{
		sprites = GetComponentsInChildren<tk2dSprite>();
		text = GetComponent<tk2dTextMesh>();
	}
	
	void Update()
	{
		if(sprites != null)
		{
			foreach(tk2dSprite sprite in sprites)
			{
				sprite.color = color;
			}
		}
		if(text != null)
		{
			text.color = color;
			text.Commit();
		}
	}
}
