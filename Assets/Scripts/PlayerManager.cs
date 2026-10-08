using System.Collections.Generic;
using UniRx;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private InputAction _mouseMove;
    [SerializeField]
    private InputAction _leftClick;
    [SerializeField]
    private InputAction _rightClick;

    [SerializeField]
    private BulletLineRenderer _bulletLineRenderer;
    [SerializeField]
    private VelocityLineRenderer _velocityLineRenderer;
    [SerializeField]
    private OutPortal _portalManager;

    [SerializeField]
    private Transform _from;
    [SerializeField]
    private float _velocityPower = 5f;
    [SerializeField]
    private GameObject _playerPrefab;

    private Vector2 _velocity;
    private Vector2 _wasClickPos;
    private bool _isRenderLine = false;
    public ReactiveProperty<int> remainPlayer = new(3);
    public ReactiveProperty<int> stopPlayer = new(0);
    public bool isPlayerRemain = true;

    private void Start()
    {
        _bulletLineRenderer.Initialize(_portalManager);
    }

    public void EnableInput()
    {
        _leftClick.Enable();
        _rightClick.Enable();
        _mouseMove.Enable();
    }

    public void DisableInput()
    {
        _leftClick.Disable();
        _rightClick.Disable();
        _mouseMove.Disable();
    }

    private void Update()
    {
        if (isPlayerRemain == false) return;

        if (_leftClick.WasPressedThisFrame())
        {
            _wasClickPos = _mouseMove.ReadValue<Vector2>();
            _wasClickPos = Camera.main.ScreenToWorldPoint(_wasClickPos);

            _isRenderLine = true;
        }

        if (_leftClick.IsPressed() && _isRenderLine)
        {
            Vector2 mousePos = _mouseMove.ReadValue<Vector2>();
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            _velocity = (_wasClickPos - mousePos) * _velocityPower;
            float angle = Mathf.Atan2(_velocity.y, _velocity.x) * Mathf.Rad2Deg;
            _from.transform.parent.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            _bulletLineRenderer.DrawLine(_from.position, _velocity);
            _velocityLineRenderer.DrawLine(_wasClickPos, mousePos, _velocity);
        }

        if (_leftClick.WasReleasedThisFrame())
        {
            if (_isRenderLine) ShootPlayer();
            AfterShoot();
        }

        if (_rightClick.WasPressedThisFrame() && _isRenderLine) AfterShoot();
    }

    private void ShootPlayer()
    {
        GameObject obj = Instantiate(_playerPrefab, _from.position, Quaternion.identity);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = _velocity;
        Player player = obj.GetComponent<Player>();
        player.OnPlayerStop += DeletePlayer;
        remainPlayer.Value -= 1;
    }
    private void DeletePlayer() { stopPlayer.Value += 1; }
    private void AfterShoot()
    {
        _bulletLineRenderer.DeleteLine();
        _velocityLineRenderer.DeleteLine();
        _isRenderLine = false;
    }
}
