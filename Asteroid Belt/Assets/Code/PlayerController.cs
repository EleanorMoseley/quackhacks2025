using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Aim Settings")]
    public Camera cam; // assign in Inspector
    [SerializeField] private LayerMask planeLayer;
    public Transform tip;   // assign in Inspector

    //public float sensitivity = 100f;    // less important
    [SerializeField]
    [Range(0f, 1f)] private float rotSpeed = 0.1f;

    // Clamp values
    private const float pitchMin = -65f;
    private const float pitchMax = 65f;
    private const float yawMin = -65f;
    private const float yawMax = 65f;

    // ---------- Debug ----------
    private Vector3 debugHitPoint;
    private bool hasHit = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    private void Update()
    {
        AimGunAtMouse();
    }

    private void AimGunAtMouse()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        // ray from camera through mouse position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, planeLayer))
        {
            Vector3 hitPoint = hit.point;
            debugHitPoint = hitPoint;
            hasHit = true;

            Vector3 dir = (hitPoint - tip.position).normalized;
            // DO NOT MAKE THIS tip.up. YOU WILL SUFFER.
            // also, don't change the rotation of the tip on the z axis, it may look in the editor, but it isn't. This is funny.
            Quaternion targetRot = Quaternion.FromToRotation(tip.forward, dir);

            // come back to this later
            //// Clamp pitch & yaw
            Vector3 euler = targetRot.eulerAngles;

            float pitch = euler.x;
            if (pitch > 180f) pitch -= 360f;

            float yaw = euler.y;
            if (yaw > 180f) yaw -= 360f;

            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
            yaw = Mathf.Clamp(yaw, yawMin, yawMax);

            Quaternion clampedRot = Quaternion.Euler(pitch, yaw, 0f);

            // set rotation :)
            // Lerping is bad for this
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed);
            // incase lerping breaks, switch back to this and turn off the clamping.
            //transform.rotation = targetRot;

            //Debug.Log($"Pitch: {pitch}, Yaw: {yaw}");
            //Debug.Log($"Pointing at: {hitPoint}");
        }
        else
        {
            hasHit = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (tip != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(tip.position, 0.1f);
            Gizmos.DrawRay(tip.position, tip.up * 0.5f);
        }
        if (hasHit)
        {
            // hit point
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(debugHitPoint, 0.2f);

            // line from gun -> hit point
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, debugHitPoint);
        }
    }
}
