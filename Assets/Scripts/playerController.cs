using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private PlayerControls _playerControls;
    private Rigidbody2D rb;

    [Header("Customization")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _lookOffset;
    [SerializeField] private GameObject _attack;
    public healthScript _healthScript;
    [SerializeField] private float _dodgeLength;
    [SerializeField] private float _dodgeCooldown;

    private bool canEnterDodge;
    private InputAction move;
    private InputAction fire;
    private InputAction mouseIn;
    private InputAction dodge;

    void Awake() {
        _playerControls = new PlayerControls();
        canEnterDodge = true;
    }
    private void OnEnable() {
        move = _playerControls.Player.Move;
        move.Enable();

        fire = _playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Fire;

        mouseIn = _playerControls.Player.MousePos;
        mouseIn.Enable();

        dodge = _playerControls.Player.Dodge;
        dodge.Enable();
        dodge.performed += EnterDodge;
    }

    private void OnDisable() {
        move.Disable();
        fire.Disable();
        mouseIn.Disable();
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        LookToMouse();
    }
    
    void FixedUpdate() {
        ApplyPlayerMovement();
    }

    private void ApplyPlayerMovement() {
        Vector2 moveDirection = move.ReadValue<Vector2>();

        rb.velocity = moveDirection * _movementSpeed;
    }

    private void Fire(InputAction.CallbackContext context) {
        Instantiate(_attack, transform.position, transform.rotation);
        //Debug.Log("fired");
    }

    private void EnterDodge(InputAction.CallbackContext context) {
        if(canEnterDodge) {
            Debug.Log("Entering Dodge");
            canEnterDodge = false;
            _healthScript.SetInvulnerable(_dodgeLength);
            funcTimer.Create(ExitDodge, _dodgeCooldown);
        }
    }
    public void ExitDodge() {
        Debug.Log("Dodge Off Cooldown");
        canEnterDodge = true;
    }

    private void LookToMouse() {
        //look towards cursor
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(mouseIn.ReadValue<Vector2>());
        Vector2 lookDirection = mousePosition - new Vector2(transform.position.x, transform.position.y);

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + _lookOffset;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotation;
    }
    
}
