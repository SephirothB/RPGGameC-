using UnityEngine;
using System.Collections;

/// <summary>
/// A class Responsible for rotating the camera arround the 3-D character, when the left mouse button is pressed.
/// </summary>
public class CameraOrbit : MonoBehaviour {

    GameObject MainCamera;
    //Main Camera Attatched to Player
    public GameObject LookAt;
    //Game Object the camera is looking at.
    #region CameraLocationConstants
    public float cam_yloc; //Factor of lookat object height by which the camera is placed above the character.
    Vector3 cam_yvect; //Used to add to the position of character to place the camera above the character.
    float LookAtHeight;
    float rotx = 0;//Rotation about the x-axis
    float rotxclampmin;
    float rotxclampmax;
    float roty = 0;//Rotaion about the LookAt's y-axis
    float y_omega = 2f;
    float zoom;//Multiple by which the camera is placed away from character.
    #endregion

    public float cam_dist;
    Vector3 vect_zoom;
    Vector3 Pos_Camera;
    Vector3 initposcamera;
    float Pos_Camx;
    float Pos_Camy;
    float Pos_Camz;
    // Use this for initialization
    void Start () {
        MainCamera = gameObject;
        zoom = 1f;
        cam_yloc = 1.41f;
        LookAtHeight = LookAt.transform.lossyScale.y * LookAt.GetComponent<CapsuleCollider>().height;
        cam_yvect = (LookAt.transform.up) * LookAtHeight * cam_yloc;
        rotxclampmin = -30;
        rotxclampmax = 135; // ~Pi/2
        cam_dist = 10f;
        initposcamera = new Vector3(0, 2.5f, -3);
        MainCamera.transform.localPosition = new Vector3(1,1,1);
    }
	
	// Update is called once per frame
	void Update () {
        UpdateCameraPostion();//Zoom/Location of Camera
        
    }

    void UpdateCameraPostion()
    { 
        zoom += Input.GetAxis("Mouse ScrollWheel") ;
        //zoom = Mathf.Clamp(zoom, 0.5f, 1);
        Debug.Log("Zoom" + zoom);
        if (Input.GetButton("Fire2"))
        {//Rotates Camera About its x or y axis
            Debug.Log("Rotx" + rotx);
            rotx += Input.GetAxis("Mouse Y");
            rotx = Mathf.Clamp(rotx, rotxclampmin, rotxclampmax); 
            roty += Input.GetAxis("Mouse X");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RevertCamera();
        }
        PositionCamera();//Place the Camera at appropiate postion.
        RotateCamera();//Rotate the Camera to current x/y rotaion
    }

    void RotateCamera()
    {
        MainCamera.transform.LookAt(LookAt.transform.position + LookAt.transform.up * LookAtHeight);
        //roty += Input.GetAxis("Mouse X");
        //rotx += Input.GetAxis("Mouse Y");
        //MainCamera.transform.Translate(Vector3.up * 10, Space.Self);
        //MainCamera.transform.Rotate(LookAt.transform.right, 20);//RotateAround(LookAt.transform.up, Input.GetAxis("Mouse X") );
        //MainCamera.transform.eulerAngles = new Vector3(rotx, MainCamera.transform.eulerAngles.z, MainCamera.transform.eulerAngles.z);
    }
    void PositionCamera()
    {
        /* if (Input.GetButton("Fire2"))
         {//Rotates Camera About its x or y axis
             MainCamera.transform.Translate(Vector3.up * Input.GetAxis("Mouse Y"), Space.Self);
             MainCamera.transform.Translate(Vector3.right * Input.GetAxis("Mouse X"), Space.Self);
             MainCamera.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"), Space.Self);
         }
         else 
         {

             cam_yvect = (LookAt.transform.up) * LookAtHeight * cam_yloc;

             MainCamera.transform.position = (LookAt.transform.position + cam_yvect) - (LookAt.transform.forward * zoom);
             //Zoom in or Out.

         }*/
        //For Zoom! Pos_Camera = (Vector3.Normalize(MainCamera.transform.position - LookAt.transform.position)) * cam_dist + LookAt.transform.position;
        // cam_dist = cam_dist * zoom;

        // Rotation About X-axis
        /*Pos_Camz = -1 * cam_dist * Mathf.Cos(rotx); //Stay Behind Target
        Pos_Camy = 1 * cam_dist * Mathf.Sin(rotx);
        Pos_Camx = -1 * cam_dist * Mathf.Sin(roty);
        Pos_Camera = new Vector3(Pos_Camx, Pos_Camy, Pos_Camz);
        // 

        //
        Pos_Camz = -1 * Pos_Camera.z * Mathf.Cos(roty);*/
        cam_dist = 1;// cam_dist * zoom;
        Pos_Camera = Quaternion.Euler(rotx, roty, 0) * initposcamera
            * cam_dist ; 
        vect_zoom = (Vector3.Normalize(MainCamera.transform.localPosition)) * zoom;
        MainCamera.transform.localPosition = Pos_Camera + vect_zoom;
        
    }

    void RevertCamera()
    {
        rotx = 0;
        roty = 0;
        zoom = 1;
    }
}
