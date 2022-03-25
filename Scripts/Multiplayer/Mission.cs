using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    [SerializeField] private Text missionText, missionWinValue;
    [SerializeField] private Image fill;

    public void SetMission(LobbyData.MissionData mission)
    {
        missionText.text = mission.text;
        missionWinValue.text = mission.win.ToString();
        fill.fillAmount = (float) ((float) mission.complete / (float) mission.value);
    }
}