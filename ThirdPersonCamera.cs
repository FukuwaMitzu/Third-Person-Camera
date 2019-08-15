using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ThirdPersonCamera : MonoBehaviour {

  

    
    [Header("Player Setup")]
    public Camera PlayerCamera;      
    public Transform Player;            

    [Space(10)]
    public float MouseSensitivity = 2f;     

    [Header("Camera Offset Setup")]
    public float RightOffset;          
    public float HightOffset;          

    [Space(10)]
    public float minDistance;
    public float maxDistance;
    float Distance;
    float wallOffset = 0.2f;

    [Space(10)]                             
    public LayerMask CameraOcclusion;      
   

   
    float offsetX;              
    float offsetY;              
    

    float x_Velocity;           
    float y_Velocity;                              
    float SmoothX = 0;        
    float SmoothY = 0;         




    private void Awake()
    {
        if (PlayerCamera == null && GetComponent<Camera>())
            PlayerCamera = GetComponent<Camera>();      
    }


    private void FixedUpdate()
    {


       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        /*--------------------------------------------*/


        offsetX += Input.GetAxis("Mouse X") * MouseSensitivity;
        offsetY -= Input.GetAxis("Mouse Y") * MouseSensitivity;


        //We might dont want the character look up or look down more than 90° so we clamp it. 
        offsetY = Mathf.Clamp(offsetY, -90, 90);                  



        SmoothX = Mathf.SmoothDamp(SmoothX, offsetX, ref x_Velocity, 0.06f);         
        SmoothY = Mathf.SmoothDamp(SmoothY, offsetY, ref y_Velocity, 0.06f);    

        Quaternion camRot = Quaternion.Euler(SmoothY, SmoothX, 0.0f);   


      

        
        Vector3 camDistance = Player.position + Vector3.up * HightOffset + PlayerCamera.transform.right * RightOffset - PlayerCamera.transform.forward * Distance;
      
        PlayerCamera.transform.position = camDistance;
        PlayerCamera.transform.rotation = camRot;  
       
        CameraCollision();

    }

    void CameraCollision()
    {
        

        RaycastHit hit;
        Vector3 camDistance = Player.position + Vector3.up * HightOffset + PlayerCamera.transform.right * RightOffset;

        bool lineCast = Physics.Linecast(camDistance, PlayerCamera.transform.position - PlayerCamera.transform.forward , out hit, CameraOcclusion);
       
        if (lineCast)
        {       
            Distance = Mathf.Clamp(Distance, minDistance, maxDistance + wallOffset);
            Distance = Mathf.Lerp(Distance, hit.distance - wallOffset, 0.5f);
        }
        else
        {
            Distance = Mathf.Lerp(Distance, maxDistance, 0.5f);
        }
    }
   
}

