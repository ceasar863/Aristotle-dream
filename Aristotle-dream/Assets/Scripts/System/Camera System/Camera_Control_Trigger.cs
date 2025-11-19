using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using Unity.Cinemachine;


public class Camera_Control_Trigger : MonoBehaviour
{

    public Custom_Inspector_Objects custom_inspector_objects;
    private Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Vector2 exit_direction = (collider.bounds.center- collision.transform.position).normalized;

            if(custom_inspector_objects.swap_cameras && custom_inspector_objects.camera_on_left!=null &&custom_inspector_objects.camera_on_right!=null)
            {
                Debug.Log(exit_direction);
                Camera_Follow_Effect.instance.Swap_Camera(custom_inspector_objects.camera_on_left, custom_inspector_objects.camera_on_right, exit_direction);
            }

            if(custom_inspector_objects.pan_camera_on_contact)
            {
                //pan the camera
                Camera_Follow_Effect.instance.Pan_Camera_On_Contact(custom_inspector_objects.pan_distance, custom_inspector_objects.pan_time, custom_inspector_objects.pan_direction, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(custom_inspector_objects.pan_camera_on_contact)
            {
                //pan the camera;
                Camera_Follow_Effect.instance.Pan_Camera_On_Contact(custom_inspector_objects.pan_distance, custom_inspector_objects.pan_time, custom_inspector_objects.pan_direction, true);
            }
        }
    }
}

[System.Serializable]
public class Custom_Inspector_Objects
{
    public bool swap_cameras = false;
    public bool pan_camera_on_contact = false;

    [HideInInspector] public CinemachineCamera camera_on_left;
    [HideInInspector] public CinemachineCamera camera_on_right;

    [HideInInspector] public Pan_Direction pan_direction;
    [HideInInspector] public float pan_distance = 3f;
    [HideInInspector] public float pan_time = 0.35f;
}

public enum Pan_Direction
{
    Up,
    Down,
    Left,
    Right,
}
