using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Camera_Control_Trigger))]
public class Customized_Script_Editor : Editor
{
    Camera_Control_Trigger camera_control_trigger;

    private void OnEnable()
    {
        camera_control_trigger = (Camera_Control_Trigger)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (camera_control_trigger.custom_inspector_objects.swap_cameras)
        {
            camera_control_trigger.custom_inspector_objects.camera_on_left =
                EditorGUILayout.ObjectField("Camera on Left", camera_control_trigger.custom_inspector_objects.camera_on_left,
                typeof(CinemachineCamera)/*限制选择框只能选择「CinemachineCamera 类型」的对象*/ , true) as CinemachineCamera;

            camera_control_trigger.custom_inspector_objects.camera_on_right =
                EditorGUILayout.ObjectField("Camera on Right", camera_control_trigger.custom_inspector_objects.camera_on_right,
                typeof(CinemachineCamera)/*限制选择框只能选择「CinemachineCamera 类型」的对象*/ , true) as CinemachineCamera;
        }

        if (camera_control_trigger.custom_inspector_objects.pan_camera_on_contact)
        {
            //枚举下拉选择框
            camera_control_trigger.custom_inspector_objects.pan_direction = (Pan_Direction)EditorGUILayout.EnumPopup("Camera Pan Direction",
                camera_control_trigger.custom_inspector_objects.pan_direction);

            camera_control_trigger.custom_inspector_objects.pan_distance = EditorGUILayout.FloatField("Pan Distance", camera_control_trigger.custom_inspector_objects.pan_distance);
            camera_control_trigger.custom_inspector_objects.pan_time = EditorGUILayout.FloatField("Pan Time", camera_control_trigger.custom_inspector_objects.pan_time);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(camera_control_trigger);
        }
    }
}