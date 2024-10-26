using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour{

    public Rigidbody rb;

    private float spd = 4.0f;

    public Camera cam;
    
    public Animator anim;

    InputAction move_action;
    
    InputAction interact_action;

    private InputAction look_action;

    void Start()
    {
        move_action = InputSystem.actions.FindAction("Move");
        interact_action = InputSystem.actions.FindAction("Interact");
        look_action = InputSystem.actions.FindAction("Look");
    }


    void Update(){
        
        Vector2 movement_direction = move_action.ReadValue<Vector2>();
        Vector2 look_change = look_action.ReadValue<Vector2>();
        
        if(movement_direction.SqrMagnitude() > 0.0f){
            anim.SetBool("Walking", true);
        }
        else{
            anim.SetBool("Walking", false);
        }

        rb.linearVelocity = transform.rotation * new Vector3(movement_direction.x, rb.linearVelocity.y, movement_direction.y) * spd;

        transform.Rotate(new Vector3(0.0f, look_change.x, 0.0f));

        if(interact_action.IsPressed()){
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 30, Time.deltaTime);
        }
        else{
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, Time.deltaTime);
        }

    }

}
