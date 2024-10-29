using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.LowLevel;
public class Player : MonoBehaviour{

    public Rigidbody rb;

    private float spd = 2.0f;

    public Camera cam;
    
    public Animator anim;

    InputAction move_action;
    
    InputAction interact_action;

    private InputAction look_action;

    InputAction exit_action;

    public float sensitivity = 0.75f;

    float interact_counter = 0.0f;

    void Start()
    {
        move_action = InputSystem.actions.FindAction("Move");
        interact_action = InputSystem.actions.FindAction("Interact");
        look_action = InputSystem.actions.FindAction("Look");
        exit_action = InputSystem.actions.FindAction("Exit");
        Cursor.lockState = CursorLockMode.Locked;
    }


    void FixedUpdate() {
        
        Vector2 movement_direction = move_action.ReadValue<Vector2>();
        Vector2 look_change = look_action.ReadValue<Vector2>() * sensitivity * 0.5f;

        
        
        if(movement_direction.SqrMagnitude() > 0.0f){
            anim.SetBool("Walking", true);
        }
        else{
            anim.SetBool("Walking", false);
        }

        rb.linearVelocity = transform.rotation * new Vector3(movement_direction.x, rb.linearVelocity.y, movement_direction.y) * spd;

        transform.Rotate(new Vector3(0.0f, look_change.x, 0.0f));

        cam.transform.Rotate(new Vector3(-look_change.y, 0.0f, 0.0f));
        //cam.transform.eulerAngles = new Vector3(Mathf.Clamp(cam.transform.eulerAngles.x, 90.0f, -90.0f), cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

        if(interact_action.IsPressed()){
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 30, Time.deltaTime);
            
        }
        else{
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime * 2.0f);
        }

        if(exit_action.IsPressed()){
            if(Application.isEditor){
                UnityEditor.EditorApplication.isPlaying = false;
            }
            else{
                Application.Quit();
            }
        }

    
        if(interact_action.IsPressed()){
            Ray ray = new Ray(cam.transform.position, cam.transform.rotation * Vector3.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 10.0f)){
                Interactable obj = hit.collider.GetComponent<Interactable>();
                if(obj != null && interact_counter >= 1.0f){
                    obj.Iteract();
                }

                Interactable obj_parent = hit.collider.GetComponentInParent<Interactable>();
                if(obj_parent != null && interact_counter >= 1.0f){
                    obj_parent.Iteract();
                }
            }
            if(interact_counter < 100.0f){
                interact_counter += Time.deltaTime;
            }
        }
        else{
            interact_counter = 0.0f;
        }
    }

}
