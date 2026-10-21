using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] float reach = 0.5f;

    Interactable[] interactables;
    Interactable current;

    void Start()
    {
        var room = GetComponentInParent<RoomController>();
        interactables = room != null
            ? room.GetComponentsInChildren<Interactable>(true)
            : new Interactable[0];
    }

    void Update()
    {
        var target = FindNearest();
        if (target != current)
        {
            if (current != null) current.SetFocused(false);
            current = target;
        }
        if (current == null) return;

        current.SetFocused(true);
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.eKey.wasPressedThisFrame)
            current.Interact();
    }

    Interactable FindNearest()
    {
        float x = transform.position.x;
        Interactable nearest = null;
        float nearestDistance = float.MaxValue;
        float nearestCenter = float.MaxValue;

        foreach (var interactable in interactables)
        {
            if (!interactable.isActiveAndEnabled) continue;
            if (!interactable.CanInteract && !interactable.HasLockedPrompt) continue;
            var zone = interactable.Zone;
            float distance = x < zone.min.x ? zone.min.x - x : x > zone.max.x ? x - zone.max.x : 0f;
            float center = Mathf.Abs(x - zone.center.x);
            if (distance > reach) continue;
            if (distance < nearestDistance || (distance == nearestDistance && center < nearestCenter))
            {
                nearest = interactable;
                nearestDistance = distance;
                nearestCenter = center;
            }
        }
        return nearest;
    }
}
