using UnityEngine;

public class TaskUIItem : MonoBehaviour
{
    public Task task { get; set; } // Reference to the Task scriptable object

    public TMPro.TMP_Text taskNameText; // UI Text component for displaying task name
    public TMPro.TMP_Text taskDescriptionText; // UI Text component for displaying task name
    public TMPro.TMP_Text scoreValueText; // UI Text component for displaying score value
    public void UpdateTaskUI()
    {
        taskNameText.text = task.taskName;
        scoreValueText.text = "Score: " + task.scoreValue.ToString();
    }
}
