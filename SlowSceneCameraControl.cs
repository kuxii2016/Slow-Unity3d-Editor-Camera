#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class SlowSceneCameraControl
{
    private static bool controlActive = false;
    private static Vector3 camPosition;
    private static float yaw;
    private static float pitch;
    private static Vector3 startPosition;
    private static float startYaw;
    private static float startPitch;

    private static float baseMoveSpeed = 0.02f;
    private static float rotationSpeed = 90f;

    private static HashSet<KeyCode> keysDown = new HashSet<KeyCode>();

    static SlowSceneCameraControl()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.KeyDown)
            keysDown.Add(e.keyCode);
        else if (e.type == EventType.KeyUp)
            keysDown.Remove(e.keyCode);

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.L)
        {
            controlActive = !controlActive;
            if (controlActive)
            {
                camPosition = sceneView.camera.transform.position;
                Vector3 angles = sceneView.camera.transform.rotation.eulerAngles;
                yaw = angles.y;
                pitch = angles.x;
                startPosition = camPosition;
                startYaw = yaw;
                startPitch = pitch;
                Debug.Log("Slow Scene Camera Control: Aktiviert");
            }
            else
            {
                Debug.Log("Slow Scene Camera Control: Deaktiviert");
            }
            e.Use();
        }

        if (!controlActive)
            return;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
        {
            camPosition = startPosition;
            yaw = startYaw;
            pitch = startPitch;
            Debug.Log("Slow Scene Camera Control: Reset Position & Rotation");
            e.Use();
        }

        HandleCameraControl(sceneView);
        DrawHUD();
        sceneView.Repaint();
    }

    private static void HandleCameraControl(SceneView sceneView)
    {
        float deltaTime = Mathf.Clamp(Time.deltaTime, 0.0001f, 0.05f);

        Vector3 moveDir = Vector3.zero;

        float currentSpeed = baseMoveSpeed;
        if (keysDown.Contains(KeyCode.LeftShift) || keysDown.Contains(KeyCode.RightShift))
            currentSpeed *= 5f;
        else if (keysDown.Contains(KeyCode.LeftControl) || keysDown.Contains(KeyCode.RightControl))
            currentSpeed *= 0.03f;

        if (keysDown.Contains(KeyCode.W)) moveDir += Vector3.forward;
        if (keysDown.Contains(KeyCode.S)) moveDir += Vector3.back;
        if (keysDown.Contains(KeyCode.A)) moveDir += Vector3.left;
        if (keysDown.Contains(KeyCode.D)) moveDir += Vector3.right;
        if (keysDown.Contains(KeyCode.Q)) moveDir += Vector3.down;
        if (keysDown.Contains(KeyCode.E)) moveDir += Vector3.up;

        if (moveDir != Vector3.zero)
        {
            moveDir.Normalize();
            Quaternion rotation = Quaternion.Euler(0, yaw, 0);
            camPosition += rotation * moveDir * currentSpeed * deltaTime * 10f;
        }

        Event e = Event.current;
        if (e.type == EventType.MouseDrag && e.button == 1)
        {
            Vector2 delta = e.delta;
            float rotationFactor = 0.005f;
            yaw += delta.x * rotationSpeed * deltaTime * rotationFactor;
            pitch -= delta.y * rotationSpeed * deltaTime * rotationFactor;
            pitch = Mathf.Clamp(pitch, -89f, 89f);
            e.Use();
        }

        Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0);
        sceneView.pivot = camPosition;
        sceneView.rotation = camRotation;
        sceneView.size = Mathf.Max(0.1f, sceneView.size);
    }

    private static void DrawHUD()
    {
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(10, 10, 350, 160), GUI.skin.box);
        GUILayout.Label("Slow Scene Camera Control");
        GUILayout.Label("L = Modus an/aus");
        GUILayout.Label("R = Reset Position & Rotation");
        GUILayout.Label("Rechte Maustaste + Maus bewegen = drehen");
        GUILayout.Label("WASD + QE = bewegen");
        GUILayout.Label("Shift = schneller (x5)");
        GUILayout.Label("Ctrl = langsamer (x0.3)");
        GUILayout.Space(10);
        GUILayout.Label("Aktuelle Basis Geschwindigkeit: " + baseMoveSpeed.ToString("0.00"));
        baseMoveSpeed = GUILayout.HorizontalSlider(baseMoveSpeed, 0.05f, 5f);
        GUILayout.EndArea();
        Handles.EndGUI();
    }
}
#endif
