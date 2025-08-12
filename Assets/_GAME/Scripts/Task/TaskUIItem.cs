using UnityEngine;
using UnityEngine.UI;

public class TaskUIItem : MonoBehaviour
{
    public Task task { get; set; } // Reference to the Task scriptable object

    public TMPro.TMP_Text taskNameText; // UI Text component for displaying task name
    public TMPro.TMP_Text scoreValueText; // UI Text component for displaying score value

    Button Button => GetComponent<Button>();
    public void Initialize(Task _task)
    {
        task = _task;
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() =>
        {
            TaskDescriptor.DescribeTask(task);
        }); 
        UpdateTaskUI();
    }
    public void UpdateTaskUI()
    {
        taskNameText.text = task.taskName;
        scoreValueText.text = "Score: " + task.scoreValue.ToString();
    }
}
