using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskPrefab : MonoBehaviour
{
    public Text task, taskNumber;
    TaskData taskData;
    public Image icon;
    public Button taskButton;
    // Start is called before the first frame update
    public void SetData(TaskData taskData)
    {
        this.taskData = taskData;
      
        task.text = taskData.restaurantId + "  delever to "+taskData.customerId;
        taskNumber.text = taskData.id.ToString();
        taskButton.onClick.AddListener(TaskSelected);
    }

    // Update is called once per frame
    void TaskSelected()
    {
        InGame.UIManager.Instance.DisablePopUp();
        GameManager.Instance.TaskSelected(taskData);
        
    }
}
