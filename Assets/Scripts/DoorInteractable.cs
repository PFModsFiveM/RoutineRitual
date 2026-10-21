using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteractable : Interactable
{
    [SerializeField] string targetScene;

    public override bool CanInteract => room == null || room.IsComplete;

    protected override void OnInteract() => SceneManager.LoadScene(targetScene);
}
