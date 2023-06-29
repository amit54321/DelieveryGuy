using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorBasics
{
    [RequireComponent(typeof(NetworkMatch))]
    public class RoomManager : NetworkBehaviour
    {
        public static RoomManager Instance;
        NetworkMatch networkMatch;
        Guid netIDGuid;

        [SyncVar]
        public string matchID;

        [SerializeField] GameObject playerLobbyUI;

        bool lockBegingame = false;
        public string lastMatchId;

        public Action<string> newSceneLoaded;

        public Vector3 positionOfPlayerReconnection;
        public bool reconnected = false;

        

        void Awake()
        {

            networkMatch = GetComponent<NetworkMatch>();
            //Set();
        }

       
        public void Set()
        {
            if(isLocalPlayer)
            {
                Instance = this;

              //  APIHandler.instance.transform.GetComponent<AutoHostClient>().NetworkConnected = true;
            }
            
            

        }

        [Command]
        public void CmdFindUser(string userID, string roomCode, string matchID,int team)
        {
            //int health = 100;

            //for (int i = 0; i < MatchMaker.instance.matches.Count; i++)
            //{
            //    if (MatchMaker.instance.matches[i].matchID == roomCode)
            //    {                
            //        foreach (Player player in MatchMaker.instance.matches[i].players)
            //        {
            //            if (player.userId.Equals(userID))
            //            {
            //                List<NetworkIdentity> Dummyplayeridentity = new List<NetworkIdentity>();
            //                foreach (NetworkIdentity ni in MatchMaker.instance.matches[i].dummyPlayer)
            //                {
            //                    if (player.userId.Equals(ni.GetComponent<DummyPlayer>().DummyId))
            //                    {


            //                        //Enable dummy fbx

            //                        Debug.Log("Disable called  " + ni.GetComponent<DummyPlayer>().DummyplayerHealth);
            //                        health =  ni.GetComponent<DummyPlayer>().DummyplayerHealth;
                                    
            //                        Dummyplayeridentity.Add(ni);
            //                        //NetworkServer.Destroy(ni.gameObject);
            //                        //break;
            //                    }

            //                }
            //                foreach (NetworkIdentity dummylist in Dummyplayeridentity)
            //                {
            //                    MatchMaker.instance.matches[i].dummyPlayer.Remove(dummylist);
            //                    NetworkServer.Destroy(dummylist.gameObject);
            //                }
            //                TargetStartGameReconnection(roomCode, player, matchID, player.position,team, health);
            //                MatchMaker.instance.matches[i].players.Remove(player);
            //                break;
            //            }
            //        }
            //    }
            //}
        }

        

        [TargetRpc]
        void TargetStartGameReconnection(string code, Player player, string matchId, Vector3 pos,int team, int dummyhealth)
        {
            //reconnected = true;
            //positionOfPlayerReconnection = pos;
            //RoomContoller.SocketMaster.instance.StartGameAfterReconnection(code, player, matchId, team);
            //Player.localPlayer.CmdSetMaxHealth(Player.localPlayer, dummyhealth);
        }

        // Start is called before the first frame update

        public override void OnStartServer()
        {
            netIDGuid = netId.ToString().ToGuid();
            networkMatch.matchId = netIDGuid;
        }

        public override void OnStartClient()
        {

            if (isLocalPlayer)
            {

                Debug.Log($"Spawning other player UI Prefab");
               // playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab(GetComponent<Player>());
            }

        }

        public override void OnStopClient()
        {
            Debug.Log($"Client Stopped");
            ClientDisconnect();

        }



        public void OnStopServer()
        {
            Debug.Log($"Client Stopped on Server");
            ServerDisconnect();
        }

        public void HostGameMatchId(bool publicMatch, string matchID, bool showMatchSuccess)
        {


            CmdHostGame(matchID, publicMatch, showMatchSuccess);
        }

        public void HostGame(bool publicMatch)
        {

            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID, publicMatch, true);
          //  RoomContoller.SocketMaster.instance.SendRoomCodeToServer(matchID);
        }

        [Command]
        public void CmdDestryNpcOfMatch(string matchID)
        {
            Debug.Log("NPC Gameobject matchID" + matchID);
            for (int i = 0; i < MatchMaker.instance.matches.Count; i++)
            {
                if (MatchMaker.instance.matches[i].matchID == matchID)
                {
                    foreach (NetworkIdentity g in MatchMaker.instance.matches[i].npcs)
                    {

                        if (g != null)
                            NetworkServer.Destroy(g.gameObject);
                    }
                    MatchMaker.instance.matches.Remove(MatchMaker.instance.matches[i]);
                    MatchMaker.instance.matchIDs.Remove(matchID);
                    break;
                }
            }
        }

        [Command]
        void CmdHostGame(string _matchID, bool publicMatch, bool showMatchSuccess)
        {
            matchID = _matchID;
            if (MatchMaker.instance.HostGame(_matchID, GetComponent<Player>(), publicMatch, out GetComponent<Player>().playerIndex))
            {
                networkMatch.matchId = _matchID.ToGuid();

                TargetHostGame(true, _matchID, GetComponent<Player>().playerIndex, showMatchSuccess);
            }
            else
            {
                TargetHostGame(false, _matchID, GetComponent<Player>().playerIndex, showMatchSuccess);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string _matchID, int playerIndex, bool showMatchSuccess)
        {
            GetComponent<Player>().playerIndex = playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            if (showMatchSuccess)
            {
               UILobby.instance.HostSuccess(success, _matchID);
            }
        }

        /* 
            JOIN MATCH
        */

        public void JoinGame(string _inputID)
        {
            CmdJoinGame(_inputID);
        }

        [Command]
        void CmdJoinGame(string _matchID)
        {
            matchID = _matchID;
            if (MatchMaker.instance.JoinGame(_matchID, GetComponent<Player>(), out GetComponent<Player>().playerIndex))
            {
                Debug.Log($"<color=green>Game Joined successfully</color>");
                networkMatch.matchId = _matchID.ToGuid();

                TargetJoinGame(true, _matchID, GetComponent<Player>().playerIndex);

                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"<color=red>Game Joined failed</color>");
                TargetJoinGame(false, _matchID, GetComponent<Player>().playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string _matchID, int playerIndex)
        {
            GetComponent<Player>().playerIndex = playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.JoinSuccess(success, _matchID);
        }

        /* 
            DISCONNECT
        */

        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }

        [Command]
        void CmdDisconnectGame()
        {
            Debug.Log("QUIT MATCH SERVER");
            ServerDisconnect();
        }

        public void ServerDisconnect()
        {
            MatchMaker.instance.PlayerDisconnected(GetComponent<Player>(), matchID);
            RpcDisconnectGame();
            networkMatch.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame()
        {
            ClientDisconnect();

        }

        public void ClientDisconnect()
        {
            //if(isLocalPlayer)
            //{
            //    APIHandler.instance.transform.GetComponent<AutoHostClient>().NetworkConnected = false;

            //    APIHandler.instance.transform.GetComponent<AutoHostClient>().StartCoroutine(APIHandler.instance.transform.GetComponent<AutoHostClient>().ReconnectToServer());
            //}

            

            if (playerLobbyUI != null)
            {
                if (!isServer)
                {
                    Destroy(playerLobbyUI);
                }
                else
                {
                    playerLobbyUI.SetActive(false);
                }
            }
        }

        

        


        public void GetAllMatchs()
        {
            CmdGetAllMatches();
        }
        [Command]
        public void CmdGetAllMatches()
        {
            SyncList<Match> matches = MatchMaker.instance.GetMatches(GetComponent<Player>());
            List<Match> m = new List<Match>();
            foreach (Match match in matches)
            {
                m.Add(match);

            }
            TargetGetAllMatchese(m);
        }


        [TargetRpc]
        void TargetGetAllMatchese(List<Match> matches)
        {
            UILobby.instance.SetMatchesUI(matches);
        }



        public void SearchGame()
        {

            CmdSearchGame();
        }
        public void SearchGameByID(string matchID, bool publicMatch)
        {
            Debug.Log("Search game by id =" + matchID + "public match =" + publicMatch);
            CmdSearchGameByID(matchID, publicMatch);
        }
        [Command]
        void CmdSearchGameByID(string matchID, bool publicMatch)
        {
            Debug.Log("Cmd game by id =" + matchID + "public match =" + publicMatch);
            if (MatchMaker.instance.SearchGameByID(GetComponent<Player>(), out int playerIndex, matchID, publicMatch))
            {
                Debug.Log("Cmd game inside if  by id =" + matchID + "public match =" + publicMatch);
                networkMatch.matchId = matchID.ToGuid();

                Match m = MatchMaker.instance.GetMatchById(GetComponent<Player>(), matchID);
                if (m.inMatch)
                {
                    TargetStartGameAlready(true, matchID, playerIndex);

                }
                else
                {
                    TargetSearchGame(true, matchID, playerIndex);

                }
                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
                //Host

            }
            else
            {

                if (publicMatch)
                {
                    TargetHostGameIfNotFound(matchID);

                }

            }
        }

        [TargetRpc]
        void TargetHostGameIfNotFound(string matchID)
        {
            Debug.Log("Target Host  game by id =" + matchID );
            CmdHostGame(matchID, true, true);
        }


        [Command]
        void CmdSearchGame()
        {
            if (MatchMaker.instance.SearchGame(GetComponent<Player>(), out int playerIndex, out matchID))
            {

                networkMatch.matchId = matchID.ToGuid();


                Match m = MatchMaker.instance.GetMatchById(GetComponent<Player>(), matchID);
                if (m.inMatch)
                {
                    TargetStartGameAlready(true, matchID, GetComponent<Player>().playerIndex);

                }
                else
                {
                    TargetSearchGame(true, matchID, GetComponent<Player>().playerIndex);

                }
                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
                //Host

            }
            else
            {
                Debug.Log($"<color=red>Game Search Failed</color>");
                TargetSearchGame(false, matchID, GetComponent<Player>().playerIndex);
            }
        }

        [TargetRpc]
        void TargetStartGameAlready(bool success, string _matchID, int playerIndex)
        {
            GetComponent<Player>().playerIndex = playerIndex;
            matchID = _matchID;
            CmdBeginGameAutomatically(_matchID);
        }


        [TargetRpc]
        void TargetSearchGame(bool success, string _matchID, int playerIndex)
        {
            GetComponent<Player>().playerIndex = playerIndex;
            matchID = _matchID;
            UILobby.instance.SearchGameSuccess(success, _matchID);
        }

        /* 
            MATCH PLAYERS
        */



        [TargetRpc]
        public void TargetPlayerCountUpdated(int playerCount)
        {
            if (playerCount > 1)
            {
                UILobby.instance.SetStartButtonActive(true);
            }
            else
            {
                UILobby.instance.SetStartButtonActive(false);
            }
        }
        [Command]
        public void CmdBeginGameAutomatically(string matchID)
        {
            MatchMaker.instance.BeginGameAutomatically(matchID, this);

        }

        /* 
            BEGIN MATCH
        */


        public void BeginGame()
        {

            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(matchID, this);
            Debug.Log($"<color=red>Game Beginning</color>");
        }

        public void StartGame(string MatchId_)
        {

            Debug.Log($"MatchID: {matchID} | StartGame FUNCTION" + " Attribute match id =" + MatchId_);//Server
            TargetBeginGame(MatchId_);
        }

        [Command]
        void CmdRemovePlayer(Player player, string lastMatchId)
        {
            if (!string.IsNullOrEmpty(lastMatchId))
            {

                MatchMaker.instance.PlayerDisconnected(player, lastMatchId);
            }
        }



        [TargetRpc]
        public void TargetBeginGame(string MatchID_)
        {
            
            if (lockBegingame)
            {
                return;

            }
            lockBegingame = true;
          //  APIHandler.instance.PlayGame(AutoHostClient.userId, matchID, PlayGameCallBack);
            Invoke("UpdateLockBeginGame", 1);
            CmdRemovePlayer(GetComponent<Player>(), lastMatchId);
            lastMatchId = matchID;
            this.matchID = MatchID_;

          //  RoomContoller.SocketMaster.instance.unloadingScene = false;

            //if (RoomContoller.SocketMaster.instance.currentScenes == RoomContoller.Scenes.EDEN)
            //{

            //    StartCoroutine(SceneLoaded(SceneManager.GetSceneByName(AuthenticationConstants.EDEN), AuthenticationConstants.EDEN));
            //    MirrorBasics.Player.localPlayer.SetTeamLocally(-1,0);
            //    SceneManager.LoadScene(AuthenticationConstants.EDEN, LoadSceneMode.Additive);

            //}

            //else if (RoomContoller.SocketMaster.instance.currentScenes == RoomContoller.Scenes.GAME)
            //{
            //    StartCoroutine(SceneLoaded(SceneManager.GetSceneByName(AuthenticationConstants.GAME), AuthenticationConstants.GAME));
            //    SceneManager.LoadScene(AuthenticationConstants.GAME, LoadSceneMode.Additive);
            //}

            //else if (RoomContoller.SocketMaster.instance.currentScenes == RoomContoller.Scenes.HOUSE)
            //{
            //    StartCoroutine(SceneLoaded(SceneManager.GetSceneByName(AuthenticationConstants.HOUSE), AuthenticationConstants.HOUSE));
            //    SceneManager.LoadScene(AuthenticationConstants.HOUSE, LoadSceneMode.Additive);
            //}
        }
        IEnumerator SceneLoaded(Scene newScene, string sceneName)
        {
            //  yield return new WaitWhile(() =>newScene.isLoaded);
            yield return new WaitForSeconds(1);
            if (newSceneLoaded != null)
            {
                newSceneLoaded.Invoke(sceneName);
            }
           
        }
        void UpdateLockBeginGame()
        {
            lockBegingame = false;
        }


        void PlayGameCallBack(string c)
        {
            Debug.LogError(c);
        }

    }
}
