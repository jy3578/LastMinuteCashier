using UnityEngine;
using System.Collections;

public class AspectRatioUtility : MonoBehaviour {
	public float _wantedAspectRatio = 0.666667f;
	public bool landscapeModeOnly = false;
	public Camera camera1;
	public tk2dCamera camera2;
	public Camera camera3;

	public bool _landscapeModeOnly = false;
	float wantedAspectRatio;
//	static Camera cam;
//	static Camera backgroundCam;

	void Awake () {
		_landscapeModeOnly = landscapeModeOnly;
//		cam = camera;
		wantedAspectRatio = _wantedAspectRatio;
		SetCamera();
	}
	
	public void SetCamera () {
		float currentAspectRatio = 0.0f;
		if(Screen.orientation == ScreenOrientation.LandscapeRight ||
		   Screen.orientation == ScreenOrientation.LandscapeLeft) {
			currentAspectRatio = (float)Screen.width / Screen.height;
		}
		else { //portrait.
			if(Screen.height  > Screen.width && _landscapeModeOnly) {
				currentAspectRatio = (float)Screen.height / Screen.width;
			}
			else {
				currentAspectRatio = (float)Screen.width / Screen.height;
			}
		}
		// If the current aspect ratio is already approximately equal to the desired aspect ratio,
		// use a full-screen Rect (in case it was set to something else previously)

		if ((int)(currentAspectRatio * 100) / 100.0f <= (int)(wantedAspectRatio * 100) / 100.0f) {
			camera1.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
	//		camera2.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
			camera2.viewportRegion= new Vector4(0.0f,0.0f,1.0f,1.0f);
			camera3.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		//	cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);		
			return;
		}
		
		// Pillarbox
		if (currentAspectRatio > wantedAspectRatio) {
			float inset = 1.0f - wantedAspectRatio/currentAspectRatio;
		//	cam.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
			camera1.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
		//	camera2.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
			camera2.viewportRegion= new Vector4(inset/2, 0.0f, 1.0f-inset, 1.0f);
			camera3.rect = new Rect(inset/2, 0.0f, 1.0f-inset, 1.0f);
		}

/*
		if (!backgroundCam) {
			// Make a new camera behind the normal camera which displays black; otherwise the unused space is undefined
			backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).camera;
			backgroundCam.depth = int.MinValue;
			backgroundCam.clearFlags = CameraClearFlags.SolidColor;
			backgroundCam.backgroundColor = Color.black;
			backgroundCam.cullingMask = 0;
		}
*/
	}
	



}
