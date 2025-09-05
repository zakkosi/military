using Oculus.Interaction.HandGrab;
using UnityEngine;

public class HandPoseValidator : MonoBehaviour
{
    private HandGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<HandGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }
    }
}