using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PortalWay : MonoBehaviour
{

    public PortalWay other_portal;

    public Camera view_cam;

    public RawImage img;

    public Collider entity;
    
    Collider find;

    float tmr = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        view_cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 32);
        if(other_portal != null && other_portal.other_portal == null){
            other_portal.other_portal = this;
            
        }
        img.texture = other_portal.view_cam.activeTexture;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(img.texture != other_portal.view_cam.activeTexture){
            img.texture = other_portal.view_cam.activeTexture;
        }
        Camera main = Camera.main;
        Camera other_cam = other_portal.view_cam;
        //float y_val = Vector3.SignedAngle(transform.forward, (main.transform.position -transform.position).normalized, Vector3.up);
        // now calculate the local rotation
        // other_cam.transform.localEulerAngles = new Vector3(
        //     0.0f,
        //     -y_val + 180.0f,
        //     0.0f
        // );

        // now we translate
        //other_cam.transform.localPosition = Quaternion.Euler(0.0f, -y_val + 180.0f, 0.0f) * Vector3.back + 1.25f * Vector3.up;
        other_cam.transform.localPosition = other_portal.transform.rotation * (main.transform.position - transform.position);
        other_cam.transform.rotation = main.transform.rotation;
        other_cam.fieldOfView = main.fieldOfView;
        // other_cam.transform.LookAt(other_cam.transform.parent);
        if(entity != null){
            tmr += Time.deltaTime;
        }
        if(find != null){
            RaycastHit hit;
            if(Physics.BoxCast(new Vector3(0.5f, 1.0f, 0.0f), new Vector3(1.0f, 2.0f, 2.0f), new Vector3(0,0,0), out hit) && hit.collider == find){
                if(entity == null && entity != find){
                    Vector3 diff = transform.position - find.transform.position;
                    Vector3 angle_diff = transform.eulerAngles - find.transform.eulerAngles;
                    find.transform.position = other_portal.transform.position + diff;
                    print(find.transform.position);
                    find.transform.eulerAngles = other_portal.transform.eulerAngles + angle_diff;
                    entity = other_portal.entity = find;
                    print(entity);
                }
            }
            find = null;
        }
    }

    void OnTriggerEnter(Collider other){
        find = other;
    }

    void OnTriggerExit(Collider other){
        if(other == entity && tmr > 0.1f){
            entity = other_portal.entity = null;
            tmr = 0.0f;
        }
    }
}
