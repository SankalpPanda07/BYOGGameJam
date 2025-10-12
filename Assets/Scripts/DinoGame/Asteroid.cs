using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Invoke(nameof(Explode), lifetime);
    }

    void Explode()
    {

        Destroy(gameObject);
    }

}
