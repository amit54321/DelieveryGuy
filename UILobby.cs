using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MirrorBasics
{

    public class UILobby : MonoBehaviour
    {

        public static UILobby instance;

        [Header("Host Join")]
        [SerializeField] InputField joinMatchInput;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable>();
        [SerializeField] Canvas lobbyCanvas;
        [SerializeField] Canvas searchCanvas;
        bool searching = false;

        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText, xpText;
        [SerializeField] GameObject beginGameButton;

        GameObject localPlayerLobbyUI;

        public GameObject cameraInScene, eventSystem;

        public Transform parentOfMatchTable;
        public MatchUI matchUI;


        public Transform lootPositionParent, dronesPositionParent, extractionsPositionParent;
     //   public Timer squadGameStartTimer, ReadyGameStartTimer;
      //  public RegionCalculation RegionCalculator;

        public GameObject lobbyUI;
        public void StartSquadGameTimer(double time)
        {

           // squadGameStartTimer.StartTimer(time);
          //  ReadyGameStartTimer.StartTimer(time);
        }

        public InputField roomcodeInput, matchIdInput;
        public void JoinGameReconnection()
        {
            Debug.Log("Joingamereconnection " + AutoHostClient.userId + "   " + RoomManager.Instance + "   " + roomcodeInput.text);
          //  RoomManager.Instance.CmdFindUser(AutoHostClient.userId, roomcodeInput.text, matchIdInput.text);
        }
        protected void OnEnable()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }
        void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= SceneLoaded;
        }
        void Start()
        {
            instance = this;
        }
        void SendFriendRequestTemp(string s)
        {

        }


        public void DeleteSquads()
        {
          //  PlayerPrefs.SetString(AuthenticationConstants.SQUADCREATED, "");
          //  APIHandler.instance.DeleteAllSquads(MirrorBasics.AutoHostClient.userId.ToString(), SendFriendRequestTemp);
        }
        public void SetMatchesUI(List<Match> matches)
        {
            foreach (Transform t in parentOfMatchTable)
            {
                Destroy(t.gameObject);
            }
            foreach (Match m in matches)
            {


               MatchUI match = Instantiate(matchUI, parentOfMatchTable);
                match.Set(m);
            }
        }
        void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

            if (scene.name.Equals("Lobby"))
            {

            }
            else
            {
                lobbyUI.gameObject.SetActive(false);
                cameraInScene.SetActive(false);
                eventSystem.SetActive(false);
            }


        }
        public GameObject env;
        public void EnableUILObby(bool toggle)
        {
            lobbyUI.gameObject.SetActive(toggle);
            cameraInScene.SetActive(toggle);
            eventSystem.SetActive(toggle);
          //  xpText.text = "XP : " + APIHandler.instance.userData.xp.ToString();

        }


        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Tab) && RoomContoller.SocketMaster.instance.currentScenes == RoomContoller.Scenes.EDEN)
            //{
            //    GetComponent<LobbySection>().DisableAllScreens();

            //    lobbyUI.SetActive(!lobbyUI.gameObject.activeSelf);
            //    if (lobbyUI.activeSelf)
            //    {
            //        MirrorBasics.Player.localPlayer.GetComponent<PlayerSetup>().SetUpPlayers(true);
            //        Debug.Log("Eden Scene Loaded from UILobby screen");
            //        EnableUILObby(true);

            //    }
            //    else
            //    {
            //        MirrorBasics.Player.localPlayer.GetComponent<PlayerSetup>().SetUpPlayers(false);
            //        EnableUILObby(false);
            //    }
            //}
        }
        public void SetStartButtonActive(bool active)
        {
            beginGameButton.SetActive(active);
        }
        /// <summary>
        /// Private match By Squad
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="showMatchSuccess"></param>
        public void HostPrivateMatchId(string matchId, bool showMatchSuccess)
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            RoomManager.Instance.HostGameMatchId(false, matchId, showMatchSuccess);
        }

        /// <summary>
        /// Public match
        /// </summary>
        public void HostPublic()
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            RoomManager.Instance.HostGame(true);
        }

        /// <summary>
        /// Private match By Code
        /// </summary>
        public void HostPrivate()
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            RoomManager.Instance.HostGame(false);
        }

        public void GetMatchs()
        {
            RoomManager.Instance.GetAllMatchs();
        }
        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                if (lobbyCanvas != null)
                    lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
                if (matchID != null)
                    matchIDText.text = matchID;
            }
            else
            {
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void Join()
        {
            if (string.IsNullOrEmpty(joinMatchInput.text))
            {
                return;
            }
            lobbySelectables.ForEach(x => x.interactable = false);

            RoomManager.Instance.JoinGame(joinMatchInput.text.ToUpper());
        }
        public void Join(string id)
        {
            lobbySelectables.ForEach(x => x.interactable = false);

            RoomManager.Instance.JoinGame(id.ToUpper());
        }
        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer);
                matchIDText.text = matchID;
            }
            else
            {
                lobbySelectables.ForEach(x => x.interactable = true);
            }
        }

        public void DisconnectGame()
        {
            if (localPlayerLobbyUI != null) Destroy(localPlayerLobbyUI);
            RoomManager.Instance.DisconnectGame();

            lobbyCanvas.enabled = false;
            lobbySelectables.ForEach(x => x.interactable = true);
        }

        public GameObject SpawnPlayerUIPrefab(Player player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
            newUIPlayer.transform.SetSiblingIndex(player.playerIndex - 1);

            return newUIPlayer;
        }
        public void BeginGameNow()
        {
            Invoke("BeginGame", 1);
        }

        public void BeginGame()
        {
            Debug.Log("Begin game called" );
            RoomManager.Instance.BeginGame();
        }

        public void SearchGame()
        {
            // StartCoroutine (Searching ());
            searchCanvas.enabled = true;
            searching = true;
            RoomManager.Instance.GetAllMatchs();
            //   RoomManager.Instance.SearchGame();
        }

        public void CancelSearchGame()
        {
            searching = false;
            searchCanvas.enabled = false;
        }

        public void SearchGameSuccess(bool success, string matchID)
        {
            if (success)
            {
                searchCanvas.enabled = false;
                searching = false;
                JoinSuccess(success, matchID);
            }
        }
        public void JoinGameBYID(string id, bool publicMatch)
        {
           Debug.Log("Join game by id when 2 player=" +id + " match = " +publicMatch);
            RoomManager.Instance.SearchGameByID(id, publicMatch);
        }


        IEnumerator Searching()
        {
            searchCanvas.enabled = true;
            searching = true;

            float searchInterval = 1;
            float currentTime = 1;

            while (searching)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = searchInterval;
                    RoomManager.Instance.SearchGame();
                }
                yield return null;
            }
            searchCanvas.enabled = false;
        }
        public void OpenInventory()
        {
            SceneManager.LoadScene(4);
        }
        public void OpenDomes()
        {
            SceneManager.LoadScene(5);
        }
    }

}