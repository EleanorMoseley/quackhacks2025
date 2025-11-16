using UnityEngine;

public class AutoOrbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed = 10f;
    public Vector3 axis = Vector3.up;

    public bool gameStarted = false;




    void LateUpdate()
    {
        if (!gameStarted)
        {
            OrbitShip();
        }
        else
        {
            gameStarted = true;
        }

        //if (target == null) return;

            //transform.RotateAround(target.position, axis, orbitSpeed * Time.unscaledDeltaTime);
            //transform.LookAt(target);
    }

    void OrbitShip()
    {
        if (target == null) return;

        transform.RotateAround(target.position, axis, orbitSpeed * Time.unscaledDeltaTime);
        transform.LookAt(target);
    }
}
