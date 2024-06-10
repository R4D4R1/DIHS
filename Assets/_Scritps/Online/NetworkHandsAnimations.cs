using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkHandsAnimations : NetworkBehaviour
{
    [SerializeField] private InputActionReference gripReference;
    [SerializeField] private InputActionReference triggerReference;

    [SerializeField] private Animator animator;

    private void Start()
    {
        if (!isOwned)
        {
            this.enabled = false;
            this.gameObject.GetComponent<ActionBasedController>().enabled = false;
        }
    }

    private void Update()
    {
        float gripValue = gripReference.action.ReadValue<float>();
        float triggerValue = triggerReference.action.ReadValue<float>();

        animator.SetFloat("Grip", gripValue);
        animator.SetFloat("Trigger", triggerValue);

    }
}
