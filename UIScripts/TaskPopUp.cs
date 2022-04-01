using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TaskData
{
    public int id;
    public string taskText;
    public int building_id;
    public int value;

}
public class TaskPopUp : BasePOpUp
{
    public TaskPrefab taskPrefab;
    public Transform parent;

    public List<TaskData> taskDatas;

     
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(Transform t in parent)
        {
            Destroy(t.gameObject);

        }

        foreach (TaskData taskData in taskDatas)
        {
            TaskPrefab task = Instantiate(taskPrefab, parent);
            task.SetData(taskData);
        }     
    }

   
}
