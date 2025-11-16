using UnityEngine;

public class Alien : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            Destroy(other.gameObject); // destroy alien
            Destroy(gameObject);       // destroy projectile
        }
    }

}
