using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class tk2dAnimationAdapter : MonoBehaviour {

	public Color color = Color.white;
	public Vector3 scale = Vector3.one;
	tk2dBaseSprite sprite = null;
	tk2dTextMesh textMesh = null;

	// Use this for initialization
	void Start() {
		sprite = GetComponent<tk2dBaseSprite>();
		textMesh = GetComponent<tk2dTextMesh>();
		if (sprite != null)
			color = sprite.color;
		if (textMesh != null)
			color = textMesh.color;
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
	}

	void DoUpdate() {
		if (sprite != null && (sprite.color != color || sprite.scale != scale)) {
			sprite.color = color;
			sprite.scale = scale;
		}
		if (textMesh != null && (textMesh.color != color || textMesh.scale != scale)) {
			if (textMesh.color != color) textMesh.color = color;
			if (textMesh.scale != scale) textMesh.scale = scale;
			textMesh.Commit();
		}
	}
}