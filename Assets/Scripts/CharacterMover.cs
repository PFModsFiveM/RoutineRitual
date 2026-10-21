using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class CharacterMover : MonoBehaviour
{
    [Serializable]
    public class Outfit
    {
        public string name;
        public Sprite idle;
        public Sprite[] walkFrames;
    }

    [SerializeField] Outfit[] outfits;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float framesPerSecond = 8f;
    [SerializeField] Transform leftLimit;
    [SerializeField] Transform rightLimit;

    static int currentOutfit;

    SpriteRenderer spriteRenderer;
    float animTime;

    public bool IsWalking { get; private set; }
    public string OutfitName => Current != null ? Current.name : null;
    Outfit Current => outfits != null && outfits.Length > 0 ? outfits[Mathf.Clamp(currentOutfit, 0, outfits.Length - 1)] : null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetOnLaunch() => currentOutfit = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ShowIdle();
    }

    public void Wear(string outfitName)
    {
        int index = Array.FindIndex(outfits, o => o.name == outfitName);
        if (index < 0)
        {
            Debug.LogWarning($"{name}: unknown outfit '{outfitName}'", this);
            return;
        }
        currentOutfit = index;
        if (!IsWalking) ShowIdle();
    }

    public void ResetOutfit()
    {
        currentOutfit = 0;
        ShowIdle();
    }

    void Update()
    {
        float input = 0f;
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed) input -= 1f;
            if (keyboard.dKey.isPressed) input += 1f;
        }

        var position = transform.position;
        float x = position.x + input * walkSpeed * Time.deltaTime;
        if (leftLimit != null) x = Mathf.Max(x, leftLimit.position.x);
        if (rightLimit != null) x = Mathf.Min(x, rightLimit.position.x);
        IsWalking = !Mathf.Approximately(x, position.x);

        if (!IsWalking)
        {
            ShowIdle();
            return;
        }

        spriteRenderer.flipX = x < position.x;
        position.x = x;
        transform.position = position;

        animTime += Time.deltaTime;
        var frames = Current != null ? Current.walkFrames : null;
        if (frames != null && frames.Length > 0)
            spriteRenderer.sprite = frames[(int)(animTime * framesPerSecond) % frames.Length];
    }

    void ShowIdle()
    {
        animTime = 0f;
        spriteRenderer.flipX = false;
        if (Current != null && Current.idle != null) spriteRenderer.sprite = Current.idle;
    }
}
