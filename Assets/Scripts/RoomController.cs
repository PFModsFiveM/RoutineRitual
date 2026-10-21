using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomController : MonoBehaviour
{
    [Serializable]
    public class RoutineTask
    {
        public string id;
        public GameObject todo;
        public GameObject done;
    }

    [SerializeField] List<RoutineTask> tasks = new List<RoutineTask>();
    [SerializeField] GameObject completionPanel;

    readonly HashSet<string> completed = new HashSet<string>();

    public bool IsComplete => CurrentTask == null;
    public bool IsTaskComplete(string id) => completed.Contains(id);
    RoutineTask CurrentTask => tasks.Find(t => !completed.Contains(t.id));

    void Start() => RefreshUI();

    void Update()
    {
        var keyboard = Keyboard.current;
        if (!Debug.isDebugBuild || keyboard == null) return;
        if (keyboard.rKey.wasPressedThisFrame) ResetRoom();
        if (keyboard.cKey.wasPressedThisFrame) CompleteCurrentTask();
    }

    public bool CompleteTask(string id)
    {
        if (!tasks.Exists(t => t.id == id))
        {
            Debug.LogWarning($"{name}: unknown task '{id}'", this);
            return false;
        }
        if (!completed.Add(id)) return false;
        RefreshUI();
        return true;
    }

    [ContextMenu("Complete Current Task")]
    public void CompleteCurrentTask()
    {
        var task = CurrentTask;
        if (task != null) CompleteTask(task.id);
    }

    [ContextMenu("Reset Room")]
    public void ResetRoom()
    {
        completed.Clear();
        foreach (var interactable in GetComponentsInChildren<Interactable>(true))
            interactable.ResetState();
        RefreshUI();
    }

    void RefreshUI()
    {
        foreach (var task in tasks)
        {
            bool isDone = completed.Contains(task.id);
            if (task.todo != null) task.todo.SetActive(!isDone);
            if (task.done != null) task.done.SetActive(isDone);
        }
        if (completionPanel != null) completionPanel.SetActive(IsComplete);
    }
}
