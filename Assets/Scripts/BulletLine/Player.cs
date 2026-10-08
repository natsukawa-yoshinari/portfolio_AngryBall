using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    public System.Action OnPlayerStop;
    private bool _isOnce = false;

    private void Update()
    {
        if (_rigidbody2D.linearVelocity.x < 0.15f)
        {
            if (_isOnce == false)
            {
                _isOnce = true;
                OnPlayerStop?.Invoke();
            }
        }
    }
}
