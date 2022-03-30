using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour, OnClick
{

    public int id;

    public void OnCLickMethod()
    {
        UnityEngine.Debug.LogError("CLICKED  " + id);
    }

  
}
