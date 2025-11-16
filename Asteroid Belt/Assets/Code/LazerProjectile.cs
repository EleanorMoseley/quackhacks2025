using UnityEngine;

public class LazerProjectile : MonoBehaviour
{
    private Vector3 _direction;
    [SerializeField] private float speed = 25f;
    [SerializeField] private float maxLifetime = 3f;

    public void Init(Vector3 direction)
    {
        _direction = direction.normalized;
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // For now: just destroy what we hit
        // Later you can add tags / layers / health etc.
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
