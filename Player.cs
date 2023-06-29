using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MirrorBasics
{

    [Serializable]
    public class PlayerColorData
    {
        public Color color;
        public string ColorName;
        public Vector2 Xpos;
        public Vector2 Zpos;
        public float YPos;

    }





    //  [RequireComponent (typeof (NetworkMatch))]
    public class Player : NetworkBehaviour
    {

        public List<PlayerColorData> playercolordata;




        [SyncVar] public Match currentMatch;

        [SyncVar] public bool Killed = false;


        public static Player localPlayer;
        //  [SyncVar]
        //   public string matchID;
        //   [SyncVar] public int playerIndex;

        [SyncVar(hook = nameof(UpdateTheTeamColor))] public int team;

        //    NetworkMatch networkMatch;

        [SyncVar]public int pod;

        [SerializeField] GameObject playerLobbyUI;

        [SyncVar(hook = nameof(UpdateThePlayerHealth))] public int Health;
        public event Action<int, int> PlayerOnHealthUpdate;

        [SyncVar] public string Playername;

        [SerializeField]
        Image Healthbar;

        [SerializeField]
        Image ownerHeatlbar;

        public bool Testingposition;

        [SerializeField]
        GameObject TeamIndicator;

        [SerializeField]
        GameObject OwnTeamIndicator;

        [SyncVar] public string userId;

        [SyncVar] public int playerIndex;

        [SyncVar] public Vector3 position;
        public Action teamColorChanged;

        //  Guid netIDGuid;
        [Server]
        public void PlayerCountUpdated(int playerCount, string matchID)
        {
            //  Debug.Log("PLAYER COUNT CURRENTLY " + playerCount + "  " + matchID);
            // RoomManager.Instance.TargetPlayerCountUpdated(playerCount);
        }
        //public void StartGame()
        //{

        //    //  Debug.Log($"MatchID: {matchID} | StartGame FUNCTION");//Server
        // GetComponent<RoomManager>().TargetBeginGame();
        //}
        protected void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoaded;
            if (!isLocalPlayer)
            {
              //  transform.GetComponent<PlayerSetup>().UpdatePlayername(Playername);
            }

        }
        [Command]
        public void CmdEnablePlayerOnServer(Player player)
        {
            if (!player.gameObject.activeSelf)
                player.gameObject.SetActive(true);
        }
        void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= SceneLoaded;
        }
        void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

            //if (scene.name.Equals(AuthenticationConstants.GAME))
            //{
            //    MirrorBasics.Player.localPlayer.GetComponent<FirstPersonController>().CursorLockToggle(CursorLockMode.Locked);
            //    //CmdSetMaxHealth();
            //    transform.GetComponent<PlayerSetup>().UpdatePlayername(Playername);
            //    if(Testingposition)
            //    {
            //        transform.position = new Vector3(1135, 15, 82);
            //    }
            //    //transform.position = new Vector3(1151, 15, 479);
            //}
            
            //else if (scene.name.Equals(AuthenticationConstants.HOUSE))
            //{
            //    transform.GetComponent<PlayerSetup>().UpdatePlayername(Playername);
            //   // transform.position = new Vector3(38, 80, 182);
            //}
            //else
            //{
            //    MirrorBasics.Player.localPlayer.GetComponent<FirstPersonController>().CursorLockToggle(CursorLockMode.None);
            //    transform.GetComponent<PlayerSetup>().UpdatePlayername(Playername);
            //    //transform.position = new Vector3(1151, 15, 479);
            //}

        }

        public void SetTeamLocally(int team, int pod)
        {
           
            CmdSetTeam(team,pod);
        }
        
        [Command]
        void CmdSetTeam(int team,int pod)
        {
            this.team = team;
            this.pod = pod;
        }

        [Command]
        public void CmdSetMaxHealth(Player _player, int newhealth)
        {
            _player.Health = newhealth;
          
        }

        [Command]
        void CmdUpdatePlayerName(string name, string id, Player _player)
        {
            Debug.Log("Player name updated" + name);
            _player.Playername = name;
            _player.userId = id;
        }




        void Awake()
        {

            DontDestroyOnLoad(gameObject);
            PlayerOnHealthUpdate += UpdatePlayerHealthBarFill;
            StartCoroutine(SendPlayerPosition());
        }
        [Command]
        void CmdUpdatePlayerPosition(Vector3 position, Player player)
        {
            player.position = position;
            //Debug.Log("NEW POSITION " + position + "  " + player.gameObject.activeSelf);
        }
        IEnumerator SendPlayerPosition()
        {
            yield return new WaitForSeconds(2);

       //     if (RoomContoller.SocketMaster.instance.currentScenes == RoomContoller.Scenes.GAME)
            {

                CmdUpdatePlayerPosition(transform.position, this);

            }
            StartCoroutine(SendPlayerPosition());
        }
        private void OnDestroy()
        {
            PlayerOnHealthUpdate -= UpdatePlayerHealthBarFill;
        }

        public override void OnStartServer()
        {

        }

        public override void OnStartClient()
        {
            if (isLocalPlayer)
            {
                localPlayer = this;
                Debug.Log("SATRT PLAYER CLENT CALLS");
                GetComponent<RoomManager>().Set();
               // CmdUpdatePlayerName(APIHandler.instance.userData.name, AutoHostClient.userId , this);
            }
        }

        public override void OnStopClient()
        {

        }



        public override void OnStopServer()
        {

           GetComponent<RoomManager>().ServerDisconnect();
        }

        void UpdateThePlayerHealth(int oldhealth, int newhealth)
        {

            PlayerOnHealthUpdate?.Invoke(newhealth, Health);
            
        }

       

        private void UpdatePlayerHealthBarFill(int oldhealth, int newhealth)
        {
            if(Healthbar.fillAmount <= 0)
            {
               //transform.GetComponent<shootbullet>().RpcDamage()
            }

            Healthbar.fillAmount = (float)((float)newhealth / 100);
            ownerHeatlbar.fillAmount = (float)((float)newhealth / 100);
        }
        public void ResetData()
        {
            TeamIndicator.GetComponent<MeshRenderer>().material.color = Color.white;
            OwnTeamIndicator.GetComponent<Image>().color = Color.white;
            OwnTeamIndicator.GetComponentInChildren<Text>().text = "";
        }
        void UpdateTheTeamColor(int oldnum, int newnum)
        {
            //if(newnum<0)
            //{
            //    return;
            //}
            //TeamIndicator.GetComponent<MeshRenderer>().material.color = playercolordata[newnum - 1].color;
            //Debug.Log("Player color =" + playercolordata[newnum - 1].color);
            //if (GetComponent<RoomManager>().reconnected
            //   && RoomContoller.SocketMaster.instance.currentScenes == RoomContoller.Scenes.GAME)
            //{
            //    GetComponent<RoomManager>().reconnected = false;
            //    Debug.LogError(GetComponent<RoomManager>().positionOfPlayerReconnection + " RECONNECTION");
            //    transform.position = GetComponent<RoomManager>().positionOfPlayerReconnection;

            //}
            //else
            //{
            //    // transform.position = new Vector3(1135, 15, 82);
            //    CmdSetMaxHealth(this, 100);
            //      float RandomXPos = UnityEngine.Random.Range(playercolordata[newnum - 1].Xpos.x, playercolordata[newnum - 1].Xpos.y);
            //    float RandomZPos = UnityEngine.Random.Range(playercolordata[newnum - 1].Zpos.x, playercolordata[newnum - 1].Zpos.y);
            //    transform.position = new Vector3(RandomXPos, 25f, RandomZPos);
            //}
            //if (teamColorChanged != null)
            //{
            //    teamColorChanged.Invoke();
            //}
            //Debug.Log(" Checkeing update team color");
            //OwnTeamIndicator.GetComponent<Image>().color = playercolordata[newnum - 1].color;
            //OwnTeamIndicator.GetComponentInChildren<Text>().text = playercolordata[newnum - 1].ColorName;
            //Killed = false;
            //CmdPlayerAliveAgain(this);
        }

        [Command]
        public void CmdPlayerAliveAgain(Player player)
        {
            player.Killed = false;
        }



    }

    
}