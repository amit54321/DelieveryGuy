using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mirror;
using UnityEngine;
using MirrorBasics;

namespace MirrorBasics
{

    [System.Serializable]
    public class Match
    {
        public string matchID;
        public bool publicMatch;
        public bool inMatch;
        public bool matchFull;
        public List<Player> players = new List<Player>();
        public List<NetworkIdentity> npcs = new List<NetworkIdentity>();
        public List<NetworkIdentity> dummyPlayer = new List<NetworkIdentity>();

        public Match(string matchID, Player player, bool publicMatch)
        {
            matchFull = false;
            inMatch = false;
            this.matchID = matchID;
            this.publicMatch = publicMatch;
            //  players.Add(player);
        }

        public void AddNpc(GameObject g)
        {
            npcs.Add(g.GetComponent<NetworkIdentity>());
        }

        public void AddDummyPlayer(GameObject g)
        {
            dummyPlayer.Add(g.GetComponent<NetworkIdentity>());
        }
        public Match() { }
    }

    public class MatchMaker : NetworkBehaviour
    {

        public static MatchMaker instance;

        public SyncList<Match> matches = new SyncList<Match>();
        public SyncList<String> matchIDs = new SyncList<String>();

        [SerializeField] int maxMatchPlayers = 2560;

        [SerializeField] GameObject DummyPrefab;


        void Awake()
        {
            instance = this;
        }

        public bool HostGame(string _matchID, Player _player, bool publicMatch, out int playerIndex)
        {
            playerIndex = -1;

            if (!matchIDs.Contains(_matchID))
            {
                matchIDs.Add(_matchID);
                Match match = new Match(_matchID, _player, publicMatch);
                matches.Add(match);
                Debug.Log($"Match generated");
                _player.currentMatch = match;

                playerIndex = 1;
                return true;
            }
            else
            {
                Debug.Log($"Match ID already exists");
                return false;
            }
        }

        public bool JoinGame(string _matchID, Player _player, out int playerIndex)
        {
            playerIndex = -1;

            if (matchIDs.Contains(_matchID))
            {

                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].matchID == _matchID)
                    {
                        // if (!matches[i].inMatch && !matches[i].matchFull) {
                        if (!matches[i].matchFull)
                        {
                            matches[i].players.Add(_player);
                            _player.currentMatch = matches[i];
                            playerIndex = matches[i].players.Count;
                            Debug.Log("Player count  Match Maker= " + matches[i].players.Count);

                            matches[i].players[0].PlayerCountUpdated(matches[i].players.Count, _matchID);

                            if (matches[i].players.Count == maxMatchPlayers)
                            {
                                matches[i].matchFull = true;
                            }

                            break;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                Debug.Log($"Match joined");
                return true;
            }
            else
            {
                Debug.Log($"Match ID does not exist");
                return false;
            }
        }
        public Match GetMatchById(Player _player, string _matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    return matches[i];
                }
            }
            return null;
        }

        public SyncList<Match> GetMatches(Player _player)
        {
            SyncList<Match> reqMatches = new SyncList<Match>();
            foreach (Match m in matches)
            {
                if (m.publicMatch)
                {
                    reqMatches.Add(m);
                }
            }
            return reqMatches;
        }
        public bool SearchGameByID(Player _player, out int playerIndex, string matchID, bool publicMatch)
        {
            playerIndex = -1;
            //     matchID = "";

            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"Checking match {matches[i].matchID} | inMatch {matches[i].inMatch} | matchFull {matches[i].matchFull} | publicMatch {matches[i].publicMatch}");


                //      if (!matches[i].inMatch && !matches[i].matchFull && matches[i].publicMatch)
                if (!matches[i].matchFull && matches[i].publicMatch && matchID.Equals(matches[i].matchID) && publicMatch)
                {

                    if (JoinGame(matches[i].matchID, _player, out playerIndex))
                    {
                        matchID = matches[i].matchID;
                        return true;
                    }
                }
                if (!matches[i].matchFull && !matches[i].publicMatch && matchID.Equals(matches[i].matchID) && !publicMatch)
                {

                    if (JoinGame(matches[i].matchID, _player, out playerIndex))
                    {
                        matchID = matches[i].matchID;
                        return true;
                    }
                }
            }

            return false;
        }

        public void PrintAllMatchPlayer()
        {
            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"Checking match {matches[i].matchID} | inMatch {matches[i].inMatch} | matchFull {matches[i].matchFull} | publicMatch {matches[i].publicMatch}");

                for (int j = 0; j < matches[i].players.Count; j++)
                {
                    Debug.Log("Player name " + matches[i].players[j].name + "|" + " Player net id " + matches[i].players[j].GetComponent<NetworkIdentity>().netId
                        + " | " + "Player Location " + matches[i].players[j].gameObject.transform.position);
                }
            }

        }
        public bool SearchGame(Player _player, out int playerIndex, out string matchID)
        {
            playerIndex = -1;
            matchID = "";

            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"Checking match {matches[i].matchID} | inMatch {matches[i].inMatch} | matchFull {matches[i].matchFull} | publicMatch {matches[i].publicMatch}");


                //      if (!matches[i].inMatch && !matches[i].matchFull && matches[i].publicMatch)
                if (!matches[i].matchFull && matches[i].publicMatch)
                {

                    if (JoinGame(matches[i].matchID, _player, out playerIndex))
                    {
                        matchID = matches[i].matchID;
                        return true;
                    }
                }
            }

            return false;
        }


        public void BeginGameAutomatically(string _matchID, RoomManager room)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"MatchID: {_matchID} | BeginGameAutomatically" + "   " + matches[i].matchID);
                if (matches[i].matchID == _matchID)
                {
                    matches[i].inMatch = true;

                    {

                        room.StartGame(_matchID);
                        Debug.Log($"MatchID: {_matchID} | BeginGameAutomatically");
                    }
                    break;
                }
            }
        }

        public void BeginGame(string _matchID, RoomManager room)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    matches[i].inMatch = true;

                    foreach (var player in matches[i].players)
                    {
                        player.GetComponent<RoomManager>().StartGame(_matchID);
                       
                    }
                    break;
                }
            }
        }

        public static string GetRandomMatchID()
        {
            string _id = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);
                if (random < 26)
                {
                    _id += (char)(random + 65);
                }
                else
                {
                    _id += (random - 26).ToString();
                }
            }
            Debug.Log($"Random Match ID: {_id}");
            return _id;
        }

        [ClientRpc]
        public void RpcEnableDummyPrefab(NetworkIdentity dummyid, string roomcode)
        {
            if (roomcode == RoomManager.Instance.matchID)
            {
                dummyid.gameObject.SetActive(true);
            }


        }

        void spawnDummyPlayer(Player _player, Match _match)
        {
            //NetworkIdentity RequiredDummy = null;

            //foreach (NetworkIdentity ni in _match.dummyPlayer)
            //{
            //    if (_player.userId.Equals(ni.GetComponent<DummyPlayer>().DummyId))
            //    {
            //        RequiredDummy = ni;

            //        //Enable dummy fbx

            //        RequiredDummy.gameObject.SetActive(true);
            //        RequiredDummy.GetComponent<DummyPlayer>().DummyplayerHealth = _player.Health;
            //        RequiredDummy.GetComponent<DummyPlayer>().DummyId = _player.userId;
            //        RequiredDummy.transform.position = _player.position;
            //        RequiredDummy.GetComponent<DummyPlayer>().DummyKilled = _player.Killed;
            //        //RpcEnableDummyPrefab(ni, _match.matchID);
            //        break;
            //    }

            //}

            //if(RequiredDummy == null)
            //{
            //    GameObject _dummyprefab = Instantiate(DummyPrefab, _player.position + new Vector3(0,-1,0), Quaternion.identity);
            //    _dummyprefab.GetComponent<DummyPlayer>().DummyplayerHealth = _player.Health;
            //    _dummyprefab.GetComponent<DummyPlayer>().DummyId = _player.userId;
            //    _dummyprefab.GetComponent<DummyPlayer>().DummyKilled = _player.Killed;
            //    //dummyprefab.transform.SetParent(this.transform);
            //    NetworkServer.Spawn(_dummyprefab);
            //    _match.AddDummyPlayer(_dummyprefab);

                
            //        NetworkMatch networkMatch = _dummyprefab.GetComponent<NetworkMatch>();
            //    Debug.Log("Dummy player spawned in match maker");
            //        if (networkMatch)
            //        {
            //         networkMatch.matchId = _match.matchID.ToGuid();
            //         Debug.Log("Match id of dummy player = " + _match.matchID + "Guid = " + _match.matchID.ToGuid());
            //        }
                        
                
            //}
        }

        public void PlayerDisconnected(Player player, string _matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"matchID " + matches[i].matchID + "    " + player.currentMatch.matchID + "Player count = " + matches[i].players.Count);
                if (matches[i].matchID == player.currentMatch.matchID)
                {
                    int playerIndex = matches[i].players.IndexOf(player);
                     

                    Debug.Log($"Player disconnected from match {player.currentMatch.matchID} | {matches[i].players.Count} players remaining");

                    if(!_matchID.Contains("DOME"))
                    {
                        Debug.Log("Spawn Function called");
                        spawnDummyPlayer(player, matches[i]);
                    }
                    
                       // matches[i].players[0].PlayerCountUpdated(matches[i].players.Count, player.currentMatch.matchID);

                    //if (matches[i].players.Count == 0)
                    //{
                    //    Debug.Log($"No more players in Match. Terminating {_matchID}");
                    //    if (_matchID.Contains("DOME"))
                    //    {
                    //        matches.RemoveAt(i);
                    //        matchIDs.Remove(_matchID);

                    //    }
                    //}
                    //else
                    //{
                    //    //Spawn dummy player here
                    //    Debug.Log("Spawn Function called");
                    //    spawnDummyPlayer(player, matches[i]);
                    //    matches[i].players[0].PlayerCountUpdated(matches[i].players.Count, _matchID);
                    //}

                    //if (matches[i].players.Count > playerIndex)
                    //    matches[i].players.RemoveAt(playerIndex);

                    //break;
                }
            }
        }

        [Command]
        public void CmdPlayerCountOnServer(Player player, string _matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                Debug.Log($"matchID " + matches[i].matchID + "    " + _matchID);
                if (matches[i].matchID == _matchID)
                {
                    int playerIndex = matches[i].players.IndexOf(player);
                    //  if (matches[i].players.Count > playerIndex)
                    // matches[i].players.RemoveAt(playerIndex);

                    Debug.Log($"Player disconnected from match {_matchID} | {matches[i].players.Count} players remaining");


                }
            }
        }

    }

    
}

    public static class MatchExtensions
    {
        public static Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }
    }

