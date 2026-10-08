using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float _breakThreshold = 5f;

    [SerializeField]
    private GameObject _explosionParticlePrefab;

    private ParticleSystem _particleSystem;

    public static event System.Action<int> OnObstacleDestroyed;

    private void Start()
    {
        GameObject obj = Instantiate(_explosionParticlePrefab, transform.position, Quaternion.identity);
        _particleSystem = obj.GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude >= _breakThreshold)
        {
            if (_explosionParticlePrefab != null)
            {
                _particleSystem.gameObject.transform.position = this.gameObject.transform.position;
                _particleSystem.Emit(8);
            }

            OnObstacleDestroyed?.Invoke(100);

            Destroy(this.gameObject);
        }
    }
}
