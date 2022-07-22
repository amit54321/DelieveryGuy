using System.Collections;
using System.Collections.Generic;
using LitJson;
using RoomContoller;
using UnityEngine;

public class MissionScreen : MonoBehaviour
{
    [SerializeField] Mission missionPrefab;
    [SerializeField] private Transform parent;

    void OnEnable()
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        GetMissionsDone();
    }

    public void GetMissionsDone()
    {
        LobbyData.DiceRolled data;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.GETALLMISSIONS,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    GetAllMissions(
                        JsonUtility.FromJson<LobbyData.GetAllMissions>(JsonMapper.ToJson(args[0])));
                }
            },
            data = new LobbyData.DiceRolled()
            {
                _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
            });
    }

    void GetAllMissions(LobbyData.GetAllMissions missions)
    {
       

        {
            foreach (LobbyData.MissionData mission in missions.missions)
            {
                 if (mission.complete < mission.value)
                {
                    InstantiateChat(mission);
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