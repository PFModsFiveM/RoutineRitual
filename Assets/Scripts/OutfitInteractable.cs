using UnityEngine;

public class OutfitInteractable : Interactable
{
    [SerializeField] CharacterMover character;
    [SerializeField] string outfitName = "Dressed";

    protected override void OnInteract()
    {
        if (character != null) character.Wear(outfitName);
        CompleteTask();
    }

    public override void ResetState()
    {
        if (character != null) character.ResetOutfit();
        base.ResetState();
    }
}
