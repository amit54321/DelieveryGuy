using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchUI : MonoBehaviour
{
    public Text matchID;
    public MirrorBasics.Match match;
    public Text currentMemberText;
    // Start is called before the first frame update
    public void Set(MirrorBasics.Match match)
    {
        this.match = match;
        currentMemberText.text = match.players.Count.ToString();
        matchID.text = match.matchID.ToString();
    }
    public void StartMatch()
    {
        MirrorBasics.UILobby.instance.JoinGameBYID(match.matchID,true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
