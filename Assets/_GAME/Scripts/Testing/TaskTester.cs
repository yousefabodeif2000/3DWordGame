using UnityEditor;
using UnityEngine;

public class TaskTester : MonoBehaviour
{
    public TaskManager TaskManager;
    public Task Task;
    public void add_random_task()
    {
        Task = TaskManager.CreateTaskData(new TaskData()
        {
            TaskName = "Test",
            TaskDescription = "Test task",
            TaskScoreValue = 99,
        });
    }
    public void mark_done_p1()
    {

    }
    public void mark_done_p2()
    {

    }
    public void mark_done_tutorial()
    {

    }
    public void activate_task()
    {

    }
    public void deactivate_task()
    {

    }
}
[CustomEditor(typeof(TaskTester))]
public class TaskTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(Application.isPlaying)
        {
            TaskTester tester = (TaskTester)target;

            var addTaskBtn = GUILayout.Button("Add Random Task");
            var doneByP1 = GUILayout.Button("Mark P1 Done");
            var doneByP2 = GUILayout.Button("Mark P2 Done");
            var doneByTut = GUILayout.Button("Mark Tutorial Done");
            var activateTask = GUILayout.Button("Activate Task");
            var deactivateTask = GUILayout.Button("Deactivate Task");

            if(addTaskBtn)
            {
                tester.add_random_task();
            }
            if(doneByP1)
            {
                tester.mark_done_p1();
            }
            if (doneByP2)
            {
                tester.mark_done_p2();
            }
            if(doneByTut)
            {
                tester.mark_done_tutorial();
            }
            if(activateTask)
                tester.activate_task();
            if (deactivateTask)
                tester.deactivate_task();
        }

    }
}
