using System;
using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;

public class LobbyData
{
    [Serializable]
    public class CreateRoomData
    {
        public string _id;
        public string roomName;
        public int numberOfPlayers;
        public int _public;
        public int time;
        public int bet;
    }

    [Serializable]
    public class JoinRoomData
    {
        public string _id;
        public string roomCode;
        public int bet;
    }

    [Serializable]
    public class RoomData
    {
        public string name;
        public int code;
        public string _id;
        public int no_of_players;
        public int _public;
        public List<UserProfile> players_joined;
    }

    [Serializable]
    public class RoomDataCallBack
    {
        public int status;
        public string message;
        public RoomData room;
        public int timeLeft;
    }

    [Serializable]
    public class ChatData
    {
        public int chatId;
        public string chatType;
        public string chatText;
    }

    [Serializable]
    public class SendChatDataCallBack
    {
        public SyncPosition chat;
    }

    [Serializable]
    public class SyncPosition
    {
        public string id;
        public string gameId;
       public PlayerPosition message;
       
    }

    public class CollisionCallBack
    {
        public Collision collison;
    
        public int status;
      
    }
    [System.Serializable]
    public class SetCoinData
    {
        public SetCoin setcoin;
        public List<CollisonData> collision;
    }
    [System.Serializable]
        public class SetCoin 
    {
        public int a;

        public int b;

    }
    public class Collision
    {
        public string id;
        public string gameId;
        public int policeId;
        public int thiefId;
    }
    [Serializable]
    public class SendChatData
    {
        public SyncPosition id;
        public string gameId;
        public int chatId;
        public string chatType;
        public string chatText;
    }

    [Serializable]
    public class AddChats
    {
        public string id;
        public ChatData chats;
    }

    [Serializable]
    public class UserProfile
    {
        public string name;
        public string email;
        public string _id;
        public string avatar;
        public string token;
        public string social_id;
        public int coins;
        public int level;
        public int bet;
        public string role;
        public int tutorial;
    }

    [Serializable]
    public class DefaultAUth
    {
        public int status;
        public UserProfile message;
        public List<ChatData> chatPacks;
        public List<MissionData> missions;
        public string error;
    }

    [Serializable]
    public class MissionData
    {
        public int id;
        public string type;
        public string text;
        public int win;
        public int value;
        public int complete;
    }

    [Serializable]
    public class MissionDone
    {
        public string id;
        public int missionId;
    }
    [Serializable]
    public class MissionDoneMany
    {
        public string id;
        public List<int> missionId;
    }
    [Serializable]
    public class MissionComplete
    {
        public List<MissionData> missionDone;
    }

    [Serializable]
    public class GetAllChats
    {
        public List<ChatData> chatPacks;
    }

    [Serializable]
    public class GetAllMissions
    {
        public List<MissionData> missions;
    }

    [Serializable]
    public class CheckRoom
    {
        public string _id;
    }

    [Serializable]
    public class LeaveRoom
    {
        public string room_id;
        public string _id;
    }

    [Serializable]
    public class ShowPopUp
    {
        public string name;
        public string desc;
    }

    [Serializable]
    public class PlayerPegs
    {
        public string user_id;
        public string room_id;
        public List<PegData> pegs_data;
    }

    [Serializable]
    public class PegData
    {
        public int peg_id;
        public string peg_type;
        public int peg_position;
        public int status;
    }

    [Serializable]
    public class GamePlayUserData
    {
        public string user_id;
        public string avatar;
        public string name;
        public int color;
        public List<GamePlayUserPegData> pegs_data;
    }

    [Serializable]
    public class GamePlayUserPegData
    {
        public string peg_type;
        public int peg_id;
        public int status;
        public int peg_position;
    }

    [Serializable]
    public class TasksDone
    {
        public int id;
        public int restaurantId;
        public int customerId;
    }
    [Serializable]
    public class GamePlayCallBack
    {
        public int status;
        public GamePlay gameplay;
    }
    [Serializable]
    public class GamePlay
    {
        public string _id;
        public string winnerId;
        public string game_id;
        public string current_Color;
        public List<TaskDoneData> tasksDone;
        public List<ProfileData> users_data;
        public int round;
        public int ai;
        public int time;
       
        public List<TaskData> tasks;
       

    }
    [Serializable]
    public class TaskDoneData
    {
        public string id;
            public string _id;
        public List<int> taskDone;

    }

    [Serializable]
    public class CollisonData
    {
        public string id;
        public int collision;
    }
    [Serializable]
    public class DiceRolled
    {
        public string _id;
        public bool rolled;
        public string game_id;
    }

    [Serializable]
    public class DiceRolledCallBack
    {
        public int status;
        public DiceRolled dice;
    }


    [Serializable]
    public class RoundCallBack
    {
        public int status;
        public Round round;
    }

    [Serializable]
    public class Round
    {
        public string turnId;
        public int diceNumber;
        public int pegPosition;
        public string pegMoveTurn;
        public List<int> pegsCanMove;
        public int pegId;
        public List<PegDataOfPlayer> pegsToKill;
        public string winnerId;
        public int winnerPos;
        public List<string> players;
        public int coins;
    }

    [Serializable]
    public class PegDataOfPlayer
    {
        public string id;
        public int pegId;
    }

    [Serializable]
    public class PegClickedData
    {
        public string pegType;
        public int pegId;
        public string id;
        public string game_id;
    }
}