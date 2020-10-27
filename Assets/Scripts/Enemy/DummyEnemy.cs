using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DummyEnemy : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!gameObject.activeInHierarchy)
            return;
        
        if (collision.gameObject.name == "Frostbolt Projectile(Clone)")
        {
            _renderer.material.color = Random.ColorHSV();
        }
    }
}