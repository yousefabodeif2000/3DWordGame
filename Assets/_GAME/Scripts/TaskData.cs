using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTaskData", menuName = "Task Data")]
public class TaskData : ScriptableObject
{
    public string TaskName = "Default Task Name";
    public string TaskDescription = "Default Task Description";
    public int TaskScoreValue = 10;
    /// <summary>
    /// logic to be met so task is completed.
    /// </summary>
    public TaskSphereData Rules = new TaskSphereData { red = 1, green = 1, blue = 1 };
}


[Serializable]
public struct TaskSphereData
{
    public int red, green, blue;
}