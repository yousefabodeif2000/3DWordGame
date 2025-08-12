using Dimensional;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class Task : MonoBehaviour
{
    static public Action<Task> OnTaskCompleted;
    static public Action<Task, bool> OnTaskActivated;
    static public Action<Task> OnTaskDoneP1;
    static public Action<Task> OnTaskDoneP2;
    static public Action<Task> OnTaskDoneTut;
    bool isActivated, isCompletedP1, isCompletedP2, isCompletedTut;
    public bool IsActivated
    {
        get { return isActivated; }
        set
        {
            isActivated = value;
            OnTaskActivated?.Invoke(this, isActivated);
        }
    }
    public bool IsCompletedP1
    {
        get { return isCompletedP1; }
        set
        {
            isCompletedP1 = value;
            OnTaskDoneP1?.Invoke(this);
        }
    }
    public bool IsCompletedP2
    {
        get { return isCompletedP2; }
        set
        {
            isCompletedP2 = value;
            OnTaskDoneP2?.Invoke(this);
        }
    }
    public bool IsCompletedTutorial
    {
        get { return isCompletedTut; }
        set
        {
            isCompletedTut = value;
            OnTaskDoneTut?.Invoke(this);
        }
    }

    public string taskName;
    /// <summary>
    /// How much score will be given for completing this task.
    /// </summary>
    public int scoreValue;
    public string taskDescription;
    public TaskManager TaskManager { get; set; }



    public TaskSphereData CurrentSpheres = new TaskSphereData { red = 0, green = 0, blue = 0 };
    public TaskSphereData TaskRules = new TaskSphereData { red = 1, green = 1, blue = 1 };

    public void OnSphereClicked(Sphere sphere)
    {
        if(IsActivated == false)
        {
            return;
        }
        CurrentSpheres.red += sphere.SphereColor == SphereColor.Red ? 1 : 0;
        CurrentSpheres.green += sphere.SphereColor == SphereColor.Green ? 1 : 0;
        CurrentSpheres.blue += sphere.SphereColor == SphereColor.Blue ? 1 : 0;
    }

}


public static class TaskExtensions
{
    public static void Initialize(this Task task, TaskManager taskManager, TaskData taskData)
    {
        task.TaskManager = taskManager;
        task.taskName = taskData.TaskName;
        task.scoreValue = taskData.TaskScoreValue;
        task.taskDescription = taskData.TaskDescription;
        task.TaskRules = taskData.Rules;
    }
    public static void CreateUI(this Task task)
    {
        TaskUIItem taskUIItem = GameObject.Instantiate(task.TaskManager.taskUIItemPrefab, task.TaskManager.tasksHolder);
        taskUIItem.Initialize(task);
    }
    public static void CheckCompletion(this Task task)
    {
        if (!task.IsActivated)
            return;

        Player player = GameManager.Instance.GetPlayerInTurn();
        var chosenSpheres = player.ChosenSpheres;

        int totalSpheres = CubicleUtility.GetTotalSpheres(task);

        // Validate color counts
        if (!HasCorrectColors(task, chosenSpheres))
        {
            Debug.LogError("Task is not completed, sphere colors do not match the task requirements.");
            return;
        }

        // Shape validation
        bool shapeValid = totalSpheres switch
        {
            4 => CubicleUtility.IsEquilateralTriangle(chosenSpheres),
            5 => CubicleUtility.IsEquilateralRectangle(chosenSpheres),
            _ => false
        };

        if (!shapeValid)
        {
            Debug.LogError($"Task is not completed, spheres do not form a valid {GetShapeName(totalSpheres)}.");
            return;
        }

        // If all checks pass
        Task.OnTaskCompleted?.Invoke(task);
    }

    private static bool HasCorrectColors(Task task, List<Sphere> chosenSpheres)
    {
        int reds = chosenSpheres.Count(s => s.SphereColor == SphereColor.Red);
        int greens = chosenSpheres.Count(s => s.SphereColor == SphereColor.Green);
        int blues = chosenSpheres.Count(s => s.SphereColor == SphereColor.Blue);

        return task.CurrentSpheres.green == greens
            && task.CurrentSpheres.red == reds
            && task.CurrentSpheres.blue == blues;
    }

    private static string GetShapeName(int totalSpheres) => totalSpheres switch
    {
        4 => "equilateral triangle",
        5 => "equilateral rectangle",
        _ => "shape"
    };


}