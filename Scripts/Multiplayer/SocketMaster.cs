using Authentication;
using BestHTTP.SocketIO;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace RoomContoller
{
    public class SocketMaster : Authentication.Authentication
    {
        public BestHTTP.SocketIO.SocketManager socketMaster;
        public static SocketMaster instance;
        public static bool socketConnected = false;
        public static string userType;
        public static string KEY;
        public LobbyData.GamePlay gamePlay;
        public List<Texture> defaultSprites;
        public static List<LobbyData.MissionComplete> missionCompleted = new List<LobbyData.MissionComplete>();


        public ProfileData profileData;
        public List<Missions> missions;
        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // private void InitialiseSocket()
        //  {
        //    InitialiseSocket();
        //   }

        void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                // Put code in here that you want to run when the game enters background.
            }
            else
            {
                InitialiseSocket();
                // Put code in here that you want to run when the game enters foreground.
            }
        }


        void Error(string error)
        {
        }

        void CheckGame()
        {

            LobbyData.CheckRoom data;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.CHECKGAME,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.LogError("createroom" + JsonMapper.ToJson(args[0]));
                    }
                },
                data = new LobbyData.CheckRoom()
                {
                    _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                });
        }

        void CheckDailyReward()
        {
            Debug.LogError("dicerolled");
            LobbyData.CheckRoom data;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.DAILYREWARD,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.LogError("createroom" + JsonMapper.ToJson(args[0]));
                    }
                },
                data = new LobbyData.CheckRoom()
                {
                    _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                });
        }

       
        public void MissionDone(int missionIdt)
        {
           
            
            LobbyData.MissionDone data;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.MISSIONDONE,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.LogError("createroom" + JsonMapper.ToJson(args[0]));
                    }
                },
                data = new LobbyData.MissionDone()
                {
                    id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                    missionId = missionIdt
                });
        }


        public void InitialiseSocket()
        {
            SocketOptions so = new SocketOptions();
            so.AutoConnect = true;
            so.Reconnection = false;
            so.ReconnectionAttempts = 0;
            so.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;
            PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string> dic =
                new PlatformSupport.Collections.ObjectModel.ObservableDictionary<string, string>();
            dic.Add("token", PlayerPrefs.GetString(PlayerPrefsData.TOKEN).Replace("Bearer ", ""));
              Debug.LogError("tokrn  " + PlayerPrefs.GetString(PlayerPrefsData.TOKEN).Replace("Bearer ", ""));
            so.AdditionalQueryParams = dic;
            socketMaster =
                new BestHTTP.SocketIO.SocketManager(new System.Uri(AuthenticationConstants.SOCKETURL + "socket.io/"),
                    so);
            socketMaster.Encoder = new BestHTTP.SocketIO.JsonEncoders.LitJsonEncoder();
            socketMaster.Open();

            Debug.LogError("start");
            socketMaster.Socket.On(SocketIOEventTypes.Connect,
                (socket, packet, args) =>
                {
                    // CheckGame();
                    //CheckDailyReward();
                    Debug.LogError("connect");

                    //    CheckGame();
                });
            socketMaster.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) =>
               Debug.LogError(string.Format("Error: {0}", args[0].ToString())));

            socketMaster.Socket.On(SocketIOEventTypes.Disconnect,
                (socket, packet, args) => { InitialiseSocket(); });

            socketConnected = true;
            socketMaster.Socket.On(LobbyConstants.ONROOMJOINED, OnRoomJoined);
            socketMaster.Socket.On(LobbyConstants.SHOWPOPUP, ShowPopUp);
            socketMaster.Socket.On(LobbyConstants.ROOMCANCEL, RoomCancel);
            socketMaster.Socket.On(LobbyConstants.STARTGAME, StartGame);
            socketMaster.Socket.On(LobbyConstants.COLLISIONCALLBACK, CollisionCallback);
            socketMaster.Socket.On(LobbyConstants.SELECTPEG, SelectPeg);
            socketMaster.Socket.On(LobbyConstants.CHANGEROUND, ChangeRound);
            socketMaster.Socket.On(LobbyConstants.GAMEEND, GameEnd);
            socketMaster.Socket.On(LobbyConstants.SETCOIN, SetCoin);
            //    socketMaster.Socket.On(LobbyConstants.ROUNDTURN, RoundChange);
            //    socketMaster.Socket.On(LobbyConstants.DICEROLLEDCALLBACK, DiceRolledCallBack);
            socketMaster.Socket.On(LobbyConstants.PLAYERQUIT, PlayerQuitsTheGame);
            socketMaster.Socket.On(LobbyConstants.CHATSENDCALLBACK, ChatReceived);
            socketMaster.Socket.On(LobbyConstants.MISSIONCOMPLETE, MissionComplete);


            socketMaster.Socket.On(LobbyConstants.UPDATEDUSER, UpdateUser);
            socketMaster.Socket.On(LobbyConstants.CONSTRUCTFINISH, ConstructionCompleted);
            socketMaster.Socket.On(LobbyConstants.UPGRADEFINISH, UpgradeCompleted);
            socketMaster.Socket.On(LobbyConstants.TASKRECEIVED, TaskReceived);
            socketMaster.Socket.On(LobbyConstants.CHATSENDCALLBACK, ChatReceived);
            socketMaster.Socket.On(LobbyConstants.SWAPFINISH, SwapFinish);
            #region Game

            #endregion
        }


        #region Game

        void TaskReceived(Socket socket, Packet packet, params object[] args)
        {
            Debug.LogError(JsonMapper.ToJson(args[0]) + "  TaskDOne  ");
            TaskDoneRecieved resp = new TaskDoneRecieved();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            SendTaskDone s = resp.message;
            if(!s.id.Equals(PlayerPrefs.GetString(PlayerPrefsData.ID)))
            {
                GameManager.Instance.SetOpponentTasks();
            }
           
        }
        void SetCoin(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  SETCOIN  ");
            LobbyData.SetCoinData resp = new LobbyData.SetCoinData();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            //   GameManager.instance.SetCoin(resp);

            // Debug.Log("MISSIONS  "+callbackdata.coinsWon[0] +"   "+callbackdata.missionDone[0]);
        }
        void ChangeRound(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  CHANGEROUND  ");
            LobbyData.GamePlayCallBack resp = new LobbyData.GamePlayCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            gamePlay = resp.gameplay;
            SceneManager.LoadScene("GameScene");

            // Debug.Log("MISSIONS  "+callbackdata.coinsWon[0] +"   "+callbackdata.missionDone[0]);
        }


        void GameEnd(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  GAMEEND  ");
            LobbyData.GamePlayCallBack resp = new LobbyData.GamePlayCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            gamePlay = resp.gameplay;
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.winnerPopUp);
            
            InGame.UIManager.Instance.HideInputs();
         //   SceneManager.LoadScene("GameScene");

        }
        void SwapFinish(Socket socket, Packet packet, params object[] args)
        {
          //  Debug.Log(JsonMapper.ToJson(args[0]) + "  GAMEEND  ");
          //  LobbyData.GamePlayCallBack resp = new LobbyData.GamePlayCallBack();
          //  JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            GameManager.Instance.SwapBuildings();
           
        }
        void MissionComplete(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.MissionComplete callbackdata = new LobbyData.MissionComplete();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), callbackdata);
            if(!missionCompleted.Contains(callbackdata))
            missionCompleted.Add(callbackdata);

        }

        void OnRoomJoined(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.RoomDataCallBack callbackdata = new LobbyData.RoomDataCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), callbackdata);
            if (callbackdata.status == 200)
            {
                UIManager.instance.EnablePanel(UIManager.instance.alreadyJoinedRoomScreen);
                UIManager.instance.alreadyJoinedRoomScreen.GetComponent<RoomContoller.AlreadyRoomJoined>()
                    .SetData(callbackdata.room, callbackdata.timeLeft);
            }
        }

        void ShowPopUp(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.ShowPopUp callbackdata = new LobbyData.ShowPopUp();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), callbackdata);
            UIManager.instance.OpenPopUpFromServer(callbackdata);
        }

        void RoomCancel(Socket socket, Packet packet, params object[] args)
        {
            LobbyData.RoomData resp = new LobbyData.RoomData();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            SceneManager.LoadScene("Lobby");
            Debug.LogError("ROOMm CancEL  " + resp.name);
        }

        void SelectPeg(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.RoomData resp = new LobbyData.RoomData();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            Invoke("SelectPeg", 0.25f);
        }

        void StartGame(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.GamePlayCallBack resp = new LobbyData.GamePlayCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            gamePlay = resp.gameplay;
           // SocketMaster.instance.StartCoroutine(SocketMaster.instance.MissionCompleted(0, 0));
            //SocketMaster.instance.StartCoroutine(SocketMaster.instance.MissionCompleted(1, 0.3f));
           // SocketMaster.instance.StartCoroutine(SocketMaster.instance.MissionCompleted(2, 0.6f));
            SceneManager.LoadScene("GameScene");
        }

        void CollisionCallback(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  Collision  ");
            LobbyData.GamePlayCallBack resp = new LobbyData.GamePlayCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            // GameManager.instance.CollisionOccured(resp);

        }

        void SelectPeg()
        {
            UIManager.instance.EnablePanel(UIManager.instance.selectPegScreen);
        }

      

        void PlayerQuitsTheGame(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.DiceRolledCallBack resp = new LobbyData.DiceRolledCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            // GameManager.instance.PlayerQuits(resp.dice._id);
        }
       
        void UpdateUser(Socket socket, Packet packet, params object[] args)
        {
            //Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
           
            RegisterCallBack(JsonMapper.ToJson(args[0]));

        }

        void ChatReceived(Socket socket, Packet packet, params object[] args)
        {
           // Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");
            LobbyData.SendChatDataCallBack resp = new LobbyData.SendChatDataCallBack();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), resp);
            //  if (GameManager.instance.playerSync != null)
            {
                //    StopCoroutine(GameManager.instance.playerSync);
            }

              GameManager.Instance.SetPlayerPosition(resp.chat);
        }
        public IEnumerator MissionCompleted(int missionId, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            LobbyData.MissionDone data;
            SocketMaster.instance.socketMaster.Socket.Emit(
                  LobbyConstants.MISSIONDONE,
                  (socket, packet, args) =>
                  {
                      if (args != null && args.Length > 0)
                      {

                      }
                  },
                  data = new LobbyData.MissionDone()
                  {
                      id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                      missionId = missionId

                  });
        }
        #endregion

       public IEnumerator SendMissions(List <int> missions)
        {
            for(int i=0;i<missions.Count;i++)
            {
                SocketMaster.instance.MissionDone(missions[i]);
                yield return new WaitForSeconds(0.5f);
            }
        }

        void ConstructionCompleted(Socket socket, Packet packet, params object[] args)
        {
            
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA CONSTRUCTION COMPLETED ");
            RestaurantFinished callbackdata = new RestaurantFinished();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), callbackdata);
            GameManager.Instance.ConstructionFInisged(callbackdata.message,true);

        }
        void UpgradeCompleted(Socket socket, Packet packet, params object[] args)
        {
           
        
            Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA UPGRADE COMPLETED ");
            RestaurantFinished callbackdata = new RestaurantFinished();
            JsonUtility.FromJsonOverwrite(JsonMapper.ToJson(args[0]), callbackdata);
            GameManager.Instance.ConstructionFInisged(callbackdata.message,false);

        }
        public void RegisterCallBack(string callback)
        {
            RegisterCallback data = JsonUtility.FromJson<RegisterCallback>(callback);

            if (data.status == 200)
            {
                profileData = data.message;
                if(GameManager.Instance!=null)
                GameManager.Instance.SetRestaurantPopUp();
            }
        }
    }
}