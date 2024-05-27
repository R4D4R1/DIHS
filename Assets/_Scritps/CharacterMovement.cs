using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed;
    private CustomInput _input = null;
    private Vector2 _userInput;
    private Vector3 _currentMovement;

    private void Awake()
    {
        _input = new CustomInput();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += MovementPerformed;
        _input.Player.Movement.canceled += MovementCancelled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= MovementPerformed;
        _input.Player.Movement.canceled -= MovementCancelled;
    }

    private void MovementPerformed(InputAction.CallbackContext value)
    {
        _userInput = value.ReadValue<Vector2>();
    }

    private void MovementCancelled(InputAction.CallbackContext value)
    {
        _userInput = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleGravity();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            _currentMovement = new Vector3(_userInput.x, 0, _userInput.y);
            characterController.Move(_currentMovement * movementSpeed * Time.deltaTime);
        }
    }

    private void HandleGravity()
    {
        if(characterController.isGrounded)
        {
            float groundGravity = -.05f;
            _currentMovement.y = groundGravity;
        }
        else
        {
            float gravity = -9.8f;
            _currentMovement.y += gravity;
            characterController.Move(_currentMovement * Time.deltaTime);
        }
    }
}
