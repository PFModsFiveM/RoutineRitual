using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected string taskId;
    [SerializeField] protected GameObject prompt;
    [SerializeField] protected GameObject lockedPrompt;
    [SerializeField] protected SpriteRenderer[] highlightTargets;
    [SerializeField] protected Color highlightColor = new Color(1f, 0.9f, 0.75f);

    protected RoomController room;
    Collider2D zone;
    bool focused;

    public virtual bool CanInteract =>
        room == null || string.IsNullOrEmpty(taskId) || !room.IsTaskComplete(taskId);
    public bool HasLockedPrompt => lockedPrompt != null;
    public Bounds Zone => zone != null ? zone.bounds : new Bounds(transform.position, Vector3.zero);

    protected virtual void Awake()
    {
        room = GetComponentInParent<RoomController>();
        zone = GetComponent<Collider2D>();
        Refresh();
    }

    public void SetFocused(bool value)
    {
        focused = value;
        Refresh();
    }

    public void Interact()
    {
        if (!CanInteract) return;
        OnInteract();
        Refresh();
    }

    protected virtual void OnInteract() => CompleteTask();

    protected void CompleteTask()
    {
        if (room != null && !string.IsNullOrEmpty(taskId))
            room.CompleteTask(taskId);
    }

    public virtual void ResetState()
    {
        focused = false;
        Refresh();
    }

    protected void Refresh()
    {
        bool usable = CanInteract;
        if (prompt != null) prompt.SetActive(focused && usable);
        if (lockedPrompt != null) lockedPrompt.SetActive(focused && !usable);
        if (highlightTargets == null) return;
        foreach (var target in highlightTargets)
            if (target != null) target.color = focused && usable ? highlightColor : Color.white;
    }
}
