using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public List<TaskData> GameTasks;
    private List<Task> Tasks;

    public TaskUIItem taskUIItemPrefab; // Prefab for the task UI item
    public Transform tasksHolder; // Parent transform for the task UI items


    public void Initialize()
    {
        foreach (var task in GameTasks)
        {
            CreateTaskData(task);
        }
    }
    public Task CreateTaskData(TaskData taskData)
    {
        Task task = new Task();
        task.Initialize(this, taskData);
        task.CreateUI();
        Tasks.Add(task);
        return task;
    }
    public void CheckTasksCompletion()
    {
        foreach (var task in Tasks)
        {
            if (task.IsActivated)
                task.CheckCompletion();
        }
        GameManager.SwitchTurns(); // Switch turns after checking tasks
    }
}
