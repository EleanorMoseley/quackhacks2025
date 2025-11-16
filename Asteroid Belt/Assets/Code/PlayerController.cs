using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Aim Settings")]
    public Camera cam;                    // assign in Inspector
    [SerializeField] private LayerMask planeLayer;
    public Transform tip;                // assign in Inspector

    [SerializeField]
    [Range(0f, 1f)] private float rotSpeed = 0.1f;

    [Header("Laser Shooting")]
    [SerializeField] private ParticleSystem laserParticleSystem;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private LayerMask hitLayers;   // layers the laser can damage / hit
    [SerializeField] private float laserRange = 1000f;
    [SerializeField] private int laserDamage = 10;

    private Vector3 currentAimDir = Vector3.right;
    private bool hasValidAim = false;

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

        if (Input.GetButtonDown("Fire1"))
        {
            FireLaser();
        }
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

            currentAimDir = dir;
            hasValidAim = true;

            // Rotation toward aim
            Quaternion targetRot = Quaternion.FromToRotation(tip.forward, dir);

            // ----- Clamp pitch & yaw -----
            Vector3 euler = targetRot.eulerAngles;

            float pitch = euler.x;
            if (pitch > 180f) pitch -= 360f;

            float yaw = euler.y;
            if (yaw > 180f) yaw -= 360f;

            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
            yaw = Mathf.Clamp(yaw, yawMin, yawMax);

            Quaternion clampedRot = Quaternion.Euler(pitch, yaw, 0f);

            // Use clampedRot (was using targetRot before)
            transform.rotation = Quaternion.Slerp(transform.rotation, clampedRot, rotSpeed);
            // Or if you want snappy:
            // transform.rotation = clampedRot;
        }
        else
        {
            // No plane hit: still allow aiming along camera ray
            hasHit = false;
            currentAimDir = ray.direction;
            hasValidAim = true;
        }
    }

    private void FireLaser()
    {
        if (!hasValidAim) return;

        // 1) Do the actual gameplay raycast
        if (Physics.Raycast(tip.position, currentAimDir, out RaycastHit hit, laserRange, hitLayers))
        {
            // Debug info for gizmos
            debugHitPoint = hit.point;
            hasHit = true;

            // If you want hitscan damage instead of projectile collision:
            // Destroy(hit.collider.gameObject);
        }

        // 2) Spawn projectile that visually travels along the same direction
        if (projectilePrefab != null)
        {
            GameObject proj = Instantiate(
                projectilePrefab,
                tip.position,
                Quaternion.LookRotation(currentAimDir, Vector3.up)
            );

            LaserProjectile laserProj = proj.GetComponent<LaserProjectile>();
            if (laserProj != null)
            {
                laserProj.Init(currentAimDir);
            }
        }

        // 3) Optional: keep your muzzle flash / laser PS at the tip
        if (laserParticleSystem != null)
        {
            laserParticleSystem.transform.position = tip.position;
            laserParticleSystem.transform.rotation = Quaternion.LookRotation(currentAimDir, Vector3.up);
            laserParticleSystem.Play();
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
            Gizmos.DrawLine(tip != null ? tip.position : transform.position, debugHitPoint);
        }
    }
}
