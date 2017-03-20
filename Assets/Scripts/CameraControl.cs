using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour {

	public     float     screenWidth = 375;

	private Camera    camera;

	private float    size;
	private float    ratio;
	private float    screenHeight;

	void Awake()
	{
		camera = GetComponent<Camera> ();
		ratio = (float)Screen.height / (float)Screen.width;
		print (Screen.height.ToString() + "," + Screen.width.ToString ());

		print ("ratio:" + ratio.ToString ());
		print ("screenwidth:" + screenWidth.ToString ());

		screenHeight =  screenWidth * ratio;
		print ("screenheight:" + screenHeight.ToString ());

		size = screenHeight / 200;
		camera.orthographicSize = size;
	
		print ("Size:" + size.ToString ());
	}

	void Update ()
	{
		/*
		camera = GetComponent<Camera> ();
		ratio = (float)Screen.height / (float)Screen.width;
		screenHeight =  screenWidth * ratio;
		size = screenHeight / 200;
		camera.orthographicSize = size;
		*/
	}

	/*
	void Awake () {
		print ("Camera Controll Called");

		float height = 667f;
		float width = 375f;

		Camera cam = GetComponent<Camera>();
		float baseAspect = height / width;
		float nowAspect = (float)Screen.height/(float)Screen.width;
		float changeAspect;

		//cam.orthographicSize = (width / 2f / 100f);
		cam.orthographicSize = Screen.height/2f/height;

		if(baseAspect > nowAspect){   

			//float bgScale = height / Screen.height;
			//float camWidth = width / (Screen.width * bgScale);
			//cam.rect = new Rect ((1f - camWidth) / 2f, 0f, camWidth, 1f);

			changeAspect = nowAspect/baseAspect;
			cam.rect=new Rect((1f - changeAspect)*0.5f,0f,changeAspect,1f);

		}else{
			
			//float bgScale = width / Screen.width;
			//float camHeight = height / (Screen.height * bgScale);
			//cam.rect = new Rect (0f, (1f - camHeight) / 2f, 1f, camHeight);

			changeAspect = baseAspect/nowAspect;
			cam.rect=new Rect(0,(1f - changeAspect)*0.5f,1f,changeAspect);
		}

		Destroy(this);
	}
*/
	/* 
	 
	  	    #region(inspector settings)
	    public int fixWidth = 1280;
	    public int fixHeight = 720;
	    public bool portrait = false;
	    public Camera[] fixedCamera;
	    #endregion
	 
	    public static float resolutionScale = -1.0f;

	    void Awake() {
		        int fw = portrait ? this.fixHeight : this.fixWidth;
		        int fh = portrait ? this.fixWidth : this.fixHeight;
		 
		        // camera
		        if( this.fixedCamera != null ){
			            Rect set_rect = this.calc_aspect(fw, fh, out resolutionScale);
			            foreach( Camera cam in this.fixedCamera ){
				                cam.rect = set_rect;
				            }
			        }
		 
		        // MEMO:NGUIのmanualHeight設定は不要、
		        // UI Root下のカメラのアスペクト比固定すればよい
		        // UI RootのAutomaticはOFF, Manual Heightは想定heightを設定する
		 
		        // アスペクト比を設定のみなので、設定後は削除
		        this.Destroy(this);
		    }
	 
	    // アスペクト比 固定するようにcameraのrect取得
	    Rect calc_aspect(float width, float height, out float res_scale) {
		        float target_aspect = width / height;
		        float window_aspect = (float)Screen.width / (float)Screen.height;
		        float scale = window_aspect / target_aspect;
		 
		        Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
		        if( 1.0f > scale ){
			            rect.x = 0;
			            rect.width = 1.0f;
			            rect.y = (1.0f - scale) / 2.0f;
			            rect.height = scale;
			            res_scale = (float)Screen.width / width;
		        } else {
			            scale = 1.0f / scale;
			            rect.x = (1.0f - scale) / 2.0f;
			            rect.width = scale;
			            rect.y = 0.0f;
			            rect.height = 1.0f;
			            res_scale = (float)Screen.height / height;
			        }
		 
		        return rect;
		    }
	*/
}