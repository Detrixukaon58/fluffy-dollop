using System;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour, Interactable
{

    public bool is_open = false;

    public bool invert_open_direction = false;

    public float local_y;

    public GameObject coll;

    bool has_interacted = false;

    public Door other_door;

    public Camera view_camera;

    public RawImage img;
    public void Iteract()
    {
        if(!has_interacted){
            if(!is_open){
                is_open = true;
                // if(other_door != null){
                //     other_door.is_open = true;
                // }
            }
            else{
                is_open = false;
                // if(other_door != null){
                //     other_door.is_open = false;
                // }
            }
            has_interacted = true;
            // if(other_door != null){
            //     other_door.has_interacted = true;
            // }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        local_y = transform.eulerAngles.y;
        // if(other_door!= null){
        //     if(other_door.view_camera != null){
        //         if(img != null && img.texture != null){
        //                 img.texture = other_door.view_camera.activeTexture;
        //         }
        //     }
        // }
        // view_camera.targetTexture = new RenderTexture(265, 552, 32);
    }

    // Update is called once per frame
    void Update()
    {
        int dir = -1;
        if(invert_open_direction){
            dir = 1;
        }

        if(is_open){
            coll.transform.eulerAngles = new Vector3(
                coll.transform.eulerAngles.x, 
                Mathf.LerpAngle(coll.transform.eulerAngles.y, local_y + dir * 90.0f, Time.deltaTime), 
                coll.transform.eulerAngles.z);
            if( Mathf.Abs(Mathf.DeltaAngle(coll.transform.eulerAngles.y, local_y + dir * 90.0f)) <= 0.1f){
                has_interacted = false;
            }

            
        }
        else{
            coll.transform.eulerAngles = new Vector3(
                coll.transform.eulerAngles.x, 
                Mathf.LerpAngle(coll.transform.eulerAngles.y, local_y, Time.deltaTime), 
                coll.transform.eulerAngles.z);
            if(Mathf.Abs(Mathf.DeltaAngle(coll.transform.eulerAngles.y, local_y)) <= 0.1f){
                has_interacted = false;

            }
        

        }
    }
}
