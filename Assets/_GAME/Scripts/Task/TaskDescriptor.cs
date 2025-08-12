using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TaskDescriptor : MonoBehaviour
{
    public TMP_Text taskTitle;
    public TMP_Text taskDescription;
    public Button activateTask;
    public TMP_Text activateBtnTxt;
    public Toggle doneByP1;
    public Toggle doneByP2;
    public Toggle doneByTutorial;

    private static TaskDescriptor _instance;

    public void Initialize()
    {
        _instance = this;
    }

    public static void DescribeTask(Task task)
    {
        _instance.taskTitle.text = task.taskName;
        _instance.taskDescription.text = task.taskDescription;
        _instance.activateTask.onClick.RemoveAllListeners();
        if (!task.IsActivated)
        {
            _instance.activateTask.onClick.AddListener(() =>
            {
                task.IsActivated = true;
                _instance.activateBtnTxt.text = "DEACTIVATE TASK";
            });
        }
        else
        {
            _instance.activateTask.onClick.AddListener(() =>
            {
                task.IsActivated = false;
                _instance.activateBtnTxt.text = "ACTIVATE TASK";
            });
        }
        _instance.gameObject.SetActive(true);
    }
}
