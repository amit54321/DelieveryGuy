using System.Collections;
using System.Collections.Generic;
using RoomContoller;
using UnityEngine;

public class MissionCompleteScreen : MonoBehaviour
{
    [SerializeField] Mission missionPrefab;
    [SerializeField] private Transform parent;

    void OnEnable()
    {
        GetAllMissions();
        SocketMaster.missionCompleted.Clear();
    }

    void GetAllMissions()
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        {
            foreach (LobbyData.MissionComplete mission
                in SocketMaster.missionCompleted)
            {
                for (int i = 0; i < mission.missionDone.Count; i++)
                {
                    InstantiateChat(mission.missionDone[i]);
                }
            }
        }
    }

    void InstantiateChat(LobbyData.MissionData missionData)
    {
        Mission mission = Instantiate(missionPrefab, parent);
        mission.SetMission(missionData);
    }
}