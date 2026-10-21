using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class CurtainInteractable : Interactable
{
    [SerializeField] GameObject closedLayer;
    [SerializeField] Light2D roomLight;
    [SerializeField] float openLightIntensity = 1f;
    [SerializeField] float lightFadeSpeed = 1.5f;

    bool startOpen;
    float startLightIntensity;

    public bool IsOpen { get; private set; }
    public override bool CanInteract => !IsOpen;

    protected override void Awake()
    {
        base.Awake();
        startOpen = closedLayer == null || !closedLayer.activeSelf;
        if (roomLight != null) startLightIntensity = roomLight.intensity;
        IsOpen = startOpen;
    }

    void Update()
    {
        if (roomLight != null)
        {
            float target = IsOpen ? openLightIntensity : startLightIntensity;
            roomLight.intensity = Mathf.MoveTowards(roomLight.intensity, target, lightFadeSpeed * Time.deltaTime);
        }

        var keyboard = Keyboard.current;
        if (Debug.isDebugBuild && keyboard != null && keyboard.tKey.wasPressedThisFrame)
            SetOpen(!IsOpen);
    }

    protected override void OnInteract() => SetOpen(true);

    public void SetOpen(bool open)
    {
        IsOpen = open;
        if (closedLayer != null) closedLayer.SetActive(!open);
        if (open) CompleteTask();
        Refresh();
    }

    public override void ResetState()
    {
        IsOpen = startOpen;
        if (closedLayer != null) closedLayer.SetActive(!startOpen);
        if (roomLight != null) roomLight.intensity = startLightIntensity;
        base.ResetState();
    }
}
