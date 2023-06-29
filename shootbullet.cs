using Mirror;
using MirrorBasics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shootbullet : NetworkBehaviour
{
    [SyncVar] public List<GameObject> npcs;
   

    [SerializeField]
    private GameObject Zombie;
    [SerializeField]
    private GameObject Drone;
    [SerializeField]
    private GameObject extractionPrefab;
    [SerializeField]
    private GameObject HypernavAgent;
    [SerializeField]
    private GameObject BTEnemy;


    [SerializeField]
    private GameObject loot;
    int spawnCount;


    private bool spawningnotAllowed;
    GameObject Wandernpc;

    //New Shooting mechanics
    public float ShootForce, UpwardForce;

    public float TimeBetweenShooting, spread, reloadtime, TimebetweenShots;
    public int magzieSize, bulletPerTap;
    public bool AllowButtonHold;

    int bulletLeft, BulletShot;

    bool Shooting, readytoshoot, reloadig;

    public Camera FPSCamera;
    public Transform AttackPoint;
    public Transform RayOriginPoint;

    public int bulletDamageAmount = 20;

    public bool allowInvoke = true;

    public LayerMask BulletRayhitlayer;
    public GameObject Flybysgameobject;
    public GameObject MuzzleFlash;
    public GameObject BulletHoleVFX;


    public bool Shootingallowed;

    [Header("Third Person References")]
    [SerializeField] GameObject RemoteAK74GunObj;
    [SerializeField] GameObject RemoteXM5GunObj;
    [SerializeField] GameObject RemoteKnifeObj;
    [SerializeField] GameObject RemoteGrenadeObj;

    [Header("Death Parameters")]
    //public RagdollManager RagdollManager;
    public GameObject RagdollGameobject;

    [Header("Consumables")]
    public GameObject RemoteWaterBottle;
    public GameObject RemoteMRE;
    public GameObject RemoteInjector;

    private void Awake()
    {
        bulletLeft = magzieSize;

    }

    private void Start()
    {


        
    }





    void Update()
    {
        if (!isLocalPlayer)
            return;



        if (Input.GetKeyDown(KeyCode.N))
        {
            MatchMaker.instance.CmdPlayerCountOnServer(Player.localPlayer, RoomManager.Instance.matchID);
        }

       

        myInput();
    }
    [Command]
    public void CmdEnableNPC()
    {
        Debug.Log("npc called");

        for (int i = 0; i < 10; i++)
        {
            int randomX = UnityEngine.Random.Range(670, 711);
            int randomZ = UnityEngine.Random.Range(905, 920);
            Debug.Log("npc spawn called");
            Vector3 spawnPosNPc = new Vector3(randomX, 5, randomZ);
           
        }
    }



    private void myInput()
    {

        

        if (Input.GetMouseButtonDown(0))
        {
            

            if (!isOwned) { return; }
            //Debug.Log("npc input called");
            //Shoot();
            MuzzleFlash.SetActive(true);



        }

        if (Input.GetMouseButtonUp(0))
        {
            MuzzleFlash.SetActive(false);
        }


        

        if (Input.GetKeyDown(KeyCode.M))
        {
            // if (GetComponent<MirrorBasics.Player>().currentMatch.players[0] == GetComponent<MirrorBasics.Player>())
            {
                Debug.Log("npc input called");
                CmdSpawnHNDrone();
            }
        }

       

        if(Input.GetKeyDown(KeyCode.B))
        {
            CmdSpawnBTEnemy();
        }
    }



    public void SpawnDroneByloot(String code)
    {
        Debug.Log("NETWORK  " + NetworkServer.active);
        //if(FindObjectOfType<InstantiateNPC>() == null)
        //{
        //    Debug.Log("Didnt find the Instantiate NPC Script on server");
        //}
        //else
        //{
        //    FindObjectOfType<InstantiateNPC>().SpawnDrone(code);
        //}

        //NetworkManager.singleton.SpawnDrones();
        for (int i = 0; i < 5; i++)
        {
            spawnCount++;


            if (spawnCount < 5)
            {
                //int randomX = UnityEngine.Random.Range(30, 220);
                //int randomZ = UnityEngine.Random.Range(88, 230);

                int randomX = UnityEngine.Random.Range(1126, 1137);
                int randomZ = UnityEngine.Random.Range(99, 102);

                Vector3 spawnPosDrone = new Vector3(randomX, 9, randomZ);
                GameObject ZombieScreaming = Instantiate(HypernavAgent, spawnPosDrone, Quaternion.identity);
                //GameObject owner = this.gameObject;
                NetworkServer.Spawn(ZombieScreaming);
            

            }


        }


    }



    [Command]
    private void CmdSpawnHNDrone()
    {
        Debug.Log("HN Agent Spawmed ");
        Vector3 SpawnhypernavDrone = new Vector3(75, 33, 113);
        GameObject ZombieScreaming = Instantiate(HypernavAgent, SpawnhypernavDrone, Quaternion.identity);
        NetworkServer.Spawn(ZombieScreaming);
    }

    [Command]
    private void CmdSpawnBTEnemy()
    {
        Debug.Log("AD Agent Spawmed ");
        Vector3 SpawnhypernavDrone = new Vector3(1127, 22.64f, 143.23f);
        GameObject _BTEnemy = Instantiate(BTEnemy, SpawnhypernavDrone, Quaternion.identity);
        NetworkServer.Spawn(_BTEnemy);
    }


    //[Command]
    //public void CmdLoot(String code, int zone, int position)
    //{
    //    Debug.Log("NETWORK  " + NetworkServer.active);


    //    Vector3 lootSpawnPos = UILobby.instance.RegionCalculator.zonedata[zone].spawnPoints[position].position;
    //    Debug.Log("Spawn region = " + UILobby.instance.RegionCalculator.zonedata[zone] + " and position = " + UILobby.instance.RegionCalculator.zonedata[zone].spawnPoints[position].position);
    //            GameObject ZombieScreaming = Instantiate(Zombie,lootSpawnPos, Quaternion.identity);
    //            NetworkServer.Spawn(ZombieScreaming);
    //            if (ZombieScreaming.GetComponent<NPC>() != null)
    //            {
    //                ZombieScreaming.GetComponent<NPC>().newCode = code;
    //            }




    //}
    [Command]
    public void CmdDestroyWeapon(NetworkIdentity netid)
    {
        Debug.Log("Gun destroyed on server = " + netid.gameObject.name);
        RpcDestroyWeapon(netid);
    }

    [ClientRpc]
    public void RpcDestroyWeapon(NetworkIdentity netid)
    {
        Debug.Log("Gun destroyed on client = " + netid.gameObject.name);
        Destroy(netid.transform.GetChild(2).gameObject);
    }


    //public void SpawnExtractions(string code, List<ExtractionPosition> extractions, List<Color> colors, Player player)
    //{
    //    foreach (ExtractionPosition extraction in extractions)
    //    {
    //        Vector3 spawnPosExtraction1 = UILobby.instance.extractionsPositionParent.GetChild(extraction.posId1).position;
    //        Vector3 spawnPosExtraction2 = UILobby.instance.extractionsPositionParent.GetChild(extraction.posId2).position;
    //        GameObject extraction1 = Instantiate(extractionPrefab, spawnPosExtraction1, Quaternion.identity);
    //        GameObject extraction2 = Instantiate(extractionPrefab, spawnPosExtraction2, Quaternion.identity);
    //        NetworkServer.Spawn(extraction1);
    //        NetworkServer.Spawn(extraction2);       
    //        AddToServerRoom(extraction1, player, code);
    //        AddToServerRoom(extraction2, player, code);        
    //        extraction1.GetComponent<Extraction>().color = colors[extraction.team - 1];
    //        extraction1.GetComponent<Extraction>().team = extraction.team;
    //        extraction2.GetComponent<Extraction>().color = colors[extraction.team - 1];
    //        extraction2.GetComponent<Extraction>().team = extraction.team;

    //    }
    //}
    //public void SpawnDrones(string code, List<DronePosition> drones, Player player)
    //{
    //    foreach (DronePosition drone in drones)
    //    {
    //        Vector3 spawnPosDrone = UILobby.instance.dronesPositionParent.GetChild(drone.posId).position;
    //        GameObject droneObj = Instantiate(HypernavAgent, spawnPosDrone, Quaternion.identity);
    //        NetworkServer.Spawn(droneObj);
    //        AddToServerRoom(droneObj, player, code);
         
    //    }
    //}
    IEnumerator SpawnGunChildWithDelay(string path, GameObject lootBox)
    {
        yield return new WaitForSeconds(2);
      ///  RpcSpawnGunAschild(path, lootBox.transform);
    }

    //[Command]
    //public void CmdSpawnMap(string code, List<LootPosition> loots, List<DronePosition> drones, List<ExtractionPosition> extractions, List<Color> colors, Player player)
    //{
    //    SpawnDrones(code, drones, player);
    //    SpawnExtractions(code, extractions, colors, player);
    //    foreach (LootPosition loot in loots)
    //    {
    //        string path = "Static/" + 1 + "/" + 1;
    //        Debug.Log("NETWORK  " + NetworkServer.active + "  " + path);
    //        Vector3 lootSpawnPos = UILobby.instance.lootPositionParent.GetChild(loot.posId).position;
    //        GameObject lootBox = Instantiate(Zombie, lootSpawnPos, Quaternion.identity);
    //        NetworkServer.Spawn(lootBox);
    //        StartCoroutine(SpawnGunChildWithDelay(path, lootBox));
    //        AddToServerRoom(lootBox, player, code);
            
    //    }
    //}


    void AddToServerRoom(GameObject g, Player player, string code)
    {
        player.currentMatch.AddNpc(g);
        NetworkMatch networkMatch = g.GetComponent<NetworkMatch>();
        if (networkMatch)
            networkMatch.matchId = code.ToGuid();
    }

    [ClientRpc]
    //public void RpcSpawnGunAschild(string path, Transform lootbox)
    //{
    //    if (lootbox == null)
    //    {
    //        Debug.Log("LOOTBOX NOT FOUND");
    //        Debug.Log("LOOTBOX NOT FOUND " + path);
    //    }
    //    else
    //    {
    //        GameObject w = Instantiate((Resources.Load(path, typeof(GameObject))) as GameObject, Vector3.zero, Quaternion.identity);
    //        w.transform.SetParent(lootbox.transform);
    //        w.transform.parent = lootbox.transform;
    //        weapon _weapon = w.GetComponent<weapon>();
    //        w.transform.localPosition = _weapon.Position;
    //        w.transform.eulerAngles = _weapon.Rotation;
    //        w.transform.localScale = _weapon.scale;
    //        Debug.Log("Loot name = " + w.name + "Loot box name 2" + lootbox.name);
    //    }
    //}



    //private void Shoot()
    //{
    //    readytoshoot = false;

    //    //Find the exact hit position 
    //    Ray ray = FPSCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    //    RaycastHit hit;



    //    //check if ray hits something
    //    Vector3 TargetPoint;
    //    if (Physics.Raycast(ray, out hit, 100f, BulletRayhitlayer))
    //    {
    //        Debug.Log("Player hit = " + hit.transform.gameObject.name);
    //        GameObject hitgameobject = hit.transform.gameObject;


    //        transform.GetComponent<BulletHitSwap>().BulletHitObjectSound(hit.point, hitgameobject, ray.origin);
    //        Vector3 normalofangle = hit.normal;
    //        Debug.Log("hit point vector = " + hit.point);
    //        Debug.Log("Look rotation vector =" + hit.normal);


    //        CmdSpawnBulletHole(hit.point, hit.normal, RoomManager.Instance.matchID);

    //        //Debug.Log("Player hit = " + hit.point);
    //        //PlayHitSound(hit);
    //        if (hit.collider != null)
    //        {
    //            //Debug.LogError("HIT  " + hit.transform.name + "__" + RoomManager.Instance.matchID);
    //            // Debug.Log(" Drone id = " + hit.collider.transform.GetComponentInParent<NPC>().newCode);
    //            if (hit.collider.tag.Equals("NPC"))
    //            {
    //                //hit.transform.GetComponentInChildren<npchealth>().DealDamage(5);
    //                CmdFireNPC(hit.transform.GetComponentInParent<NetworkIdentity>());
    //            }
    //            else if (hit.collider.tag.Equals("Player") && hit.transform.GetComponentInParent<NetworkIdentity>() != GetComponent<NetworkIdentity>())
    //            {
    //                Debug.Log("Hit Target name = " + hit.transform.gameObject.name);
    //                // if (hit.collider.GetComponent<MirrorBasics.Player>().team != MirrorBasics.Player.localPlayer.team)
    //                CmdShootsTheEnmeny(hit.transform.GetComponentInParent<NetworkIdentity>(), GetComponent<NetworkIdentity>());
    //            }
    //            else if (hit.collider.tag.Equals("PlayerHead") && hit.transform.GetComponentInParent<NetworkIdentity>() != GetComponent<NetworkIdentity>())
    //            {
    //                Debug.Log("Hit Player Head");
    //                //  if (hit.collider.GetComponentInParent<MirrorBasics.Player>().team != MirrorBasics.Player.localPlayer.team)
    //                CmdShootThePlayerHead(hit.transform.GetComponentInParent<NetworkIdentity>(), GetComponent<NetworkIdentity>());
    //            }
    //            else if (hit.collider.tag.Equals("Ragdoll"))
    //            {
    //                Vector3 explosionPoint = Vector3.zero;
    //                explosionPoint = GetBehindPosition(RayOriginPoint.position, hit.point, 0.9f);
    //                Vector3 GetBehindPosition(Vector3 start, Vector3 end, float percent)
    //                {
    //                    return (start + percent * (end - start));
    //                }
    //                Debug.Log("Hit point and collided with the Ragdoll =" + explosionPoint);

    //                CmdhitRagdoll(hit.transform.GetComponentInParent<NetworkIdentity>(), explosionPoint);

    //                //hit.collider.transform.gameObject.GetComponentInParent<AnimController>().OnDie(explosionPoint);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        TargetPoint = ray.GetPoint(75);
    //    }

        

    //}

    #region PlayerControllerReferences

    public void HitDrone(Transform npc)
    {
        Debug.Log("Hit Drone");
        CmdFireNPC(npc.GetComponentInParent<NetworkIdentity>());
    }

    public void hitEnemy(Transform enemy , Vector3 hit)
    {
        Debug.Log("Hit Target name = " + enemy.gameObject.name);
        // if (hit.collider.GetComponent<MirrorBasics.Player>().team != MirrorBasics.Player.localPlayer.team)
        Vector3 explosionPoint = Vector3.zero;
        explosionPoint = GetBehindPosition(RayOriginPoint.position, hit, 0.9f);
        Vector3 GetBehindPosition(Vector3 start, Vector3 end, float percent)
        {
            return (start + percent * (end - start));
        }
        CmdShootsTheEnmeny(enemy.GetComponentInParent<NetworkIdentity>(), GetComponent<NetworkIdentity>(), explosionPoint);
    }

    public void HeadShot(Transform head, Vector3 hit)
    {
        Vector3 explosionPoint = Vector3.zero;
        explosionPoint = GetBehindPosition(RayOriginPoint.position, hit, 0.9f);
        Vector3 GetBehindPosition(Vector3 start, Vector3 end, float percent)
        {
            return (start + percent * (end - start));
        }
        Debug.Log("Hit Player Head");
        //  if (hit.collider.GetComponentInParent<MirrorBasics.Player>().team != MirrorBasics.Player.localPlayer.team)
        CmdShootThePlayerHead(head.GetComponentInParent<NetworkIdentity>(), GetComponent<NetworkIdentity>(), explosionPoint);
    }

    [Command]
    private void CmdFireNPC(NetworkIdentity sender)
    {
        if (sender != null)
        {
            Debug.Log("Id of Drone sent to server" + sender);
          //  sender.GetComponentInParent<npchealth>().DealDamage(5);

        }

    }

    [Command]
    private void CmdShootsTheEnmeny(NetworkIdentity sender, NetworkIdentity id, Vector3 hitdirection)
    {
        if (sender.GetComponent<Player>() != null)
        {
            Debug.Log("Sender id =" + sender + " " + "Player id = " + id);
            sender.GetComponent<Player>().Health -= bulletDamageAmount;
            Debug.Log("Player killed status on server=" + sender.GetComponent<Player>().Killed);
            if (!sender.GetComponent<Player>().Killed && sender.GetComponent<Player>().Health < 0)
            {
                
                sender.GetComponent<Player>().Killed = true;
                RpcDamageEnemy(sender, id, hitdirection);
            }
        }
        else
        {
            Debug.Log("Sender id =" + sender + " " + "Player id = " + id);
          //  sender.GetComponent<DummyPlayer>().DummyplayerHealth -= bulletDamageAmount;
          //  Debug.Log("Player killed status on server=" + sender.GetComponent<DummyPlayer>().DummyKilled);
           // if (!sender.GetComponent<DummyPlayer>().DummyKilled && sender.GetComponent<DummyPlayer>().DummyplayerHealth < 0)
            {
               
           //     sender.GetComponent<DummyPlayer>().DummyKilled = true;
            //    RpcDamageDummy(id, sender, hitdirection);
            //    NetworkServer.Destroy(sender.gameObject);
            }
        }
        //RpcDamage(sender, id);
    }

    [Command]
    private void CmdShootThePlayerHead(NetworkIdentity sender, NetworkIdentity id, Vector3 hitdirection)
    {
        Debug.Log("Sender id =" + sender + " " + "Player id = " + id);
        sender.GetComponent<Player>().Health = 0;
        Debug.Log("Player killed status on server=" + sender.GetComponent<Player>().Killed);

        //TargetRpcHeadshoot(sender, id);
        if (!sender.GetComponent<Player>().Killed && sender.GetComponent<Player>().Health <= 0)
        {
            
            sender.GetComponent<Player>().Killed = true;
            RpcDamageEnemy(sender, id, hitdirection);
        }

        //RpcDamage(sender, id);
    }


    [Command]
    public void CmdturnoffRemoteWeapons()
    {
        RpcTurnOffAllRemoteWeapons();
        Debug.Log("TP Gun Turn Off Called on Server");
    }

    [ClientRpc]
    void RpcTurnOffAllRemoteWeapons()
    {
        Debug.Log("TP Gun Turn Off Received on Client");
        //id.GetComponent<shootbullet>().TurnOffRemoteWeaponsRpcRecived();
        RemoteAK74GunObj.SetActive(false);
        RemoteXM5GunObj.SetActive(false);
        RemoteKnifeObj.SetActive(false);
        RemoteGrenadeObj.SetActive(false);
    }


    [Command]
    public void CmdTurnOnRemoteWeapon(int weaponid)
    {
        RpcTurnOnRemoteWeapon( weaponid);
        Debug.Log("TP Gun Turn ON Called on Server");
    }


    [ClientRpc]
    void RpcTurnOnRemoteWeapon(int ID)
    {
        if(isLocalPlayer)
        {
            return;
        }
        else
        {
            Debug.Log("TP Gun Turn ON Received on Client");
            if (ID == 1)
            {
                RemoteAK74GunObj.SetActive(true);
            }
            else if (ID == 2)
            {
                RemoteXM5GunObj.SetActive(true);
            }
            else if (ID == 3)
            {
                //RemoteXM5GunObj.SetActive(true);
            }
            else if (ID == 4)
            {
                //RemoteXM5GunObj.SetActive(true);
            }
            else if (ID == 5)
            {
                //RemoteXM5GunObj.SetActive(true);
            }
            else if (ID == 6)
            {
                //RemoteXM5GunObj.SetActive(true);
            }
            else if (ID == 7)
            {
                RemoteKnifeObj.SetActive(true);
            }
            else if (ID == 8)
            {
                RemoteGrenadeObj.SetActive(true);
            }
        }
       
    }

    [Command]
    public void CmdturnONRemoteWater()
    {
        RpcturnONRemoteWater();
    }

    [ClientRpc]
    public void RpcturnONRemoteWater()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            RemoteWaterBottle.SetActive(true);
        }
        
    }

    [Command]
    public void CmdturnOFFRemoteWater()
    {
        RpcturnOFFRemoteWater();
    }

    [ClientRpc]
    public void RpcturnOFFRemoteWater()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            RemoteWaterBottle.SetActive(false);
        }
        
    }

    [Command]
    public void CmdturnONRemoteMRE()
    {
        RpcturnONRemoteMRE();
    }

    [ClientRpc]
    public void RpcturnONRemoteMRE()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            RemoteMRE.SetActive(true);
        }
        
    }

    [Command]
    public void CmdturnOFFRemoteMRE()
    {
        RpcturnOFFRemoteMRE();
    }

    [ClientRpc]
    public void RpcturnOFFRemoteMRE()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            RemoteMRE.SetActive(false);
        }
        
    }

    //

    [Command]
    public void CmdturnONRemoteInjector()
    {
        RpcturnONRemoteInjector();
    }

    [ClientRpc]
    public void RpcturnONRemoteInjector()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            RemoteInjector.SetActive(true);
        }
       
    }

    [Command]
    public void CmdturnOFFRemoteInjector()
    {
        RpcturnOFFRemoteInjector();
    }

    [ClientRpc]
    public void RpcturnOFFRemoteInjector()
    {
        if (isLocalPlayer)
        {
            return;
        }
        else
        {
            RemoteInjector.SetActive(false);
        }
    }

    #endregion



    void SetMatchId(NetworkIdentity i, string matchId)
    {
        Debug.Log("SERVER MATCHID  " + matchId);

    }


    [Command]
    public void CmdSpawnBulletHole(Vector3 hitloc, Vector3 hitnormal, string roomcode)
    {
        Debug.Log("Hit location = " + hitloc + " hitnormal =" + hitnormal);
        GameObject BulletHole = Instantiate(BulletHoleVFX, hitloc + new Vector3(0, 0, -0.05f), Quaternion.LookRotation(-hitnormal));
        NetworkServer.Spawn(BulletHole);
        NetworkMatch networkMatch = BulletHole.GetComponent<NetworkMatch>();
        if (networkMatch)
            networkMatch.matchId = roomcode.ToGuid();
    }


    



    

    

    [Command]
    public void CmdSpawnFlybysPrefab()
    {
        //Debug.Log( "Bullet spawend at this point =" +SpawnPos);
        Debug.Log("****************************************" + AttackPoint.transform.position);
        GameObject Flybysobject = Instantiate(Flybysgameobject, AttackPoint.transform.position, AttackPoint.transform.rotation);
        NetworkServer.Spawn(Flybysobject);
    }

    [Command]
    public void CmdhitRagdoll(NetworkIdentity sender, Vector3 hitdirection)
    {
        Debug.Log("Ragdoll Reached at the Server");
        // if(sender.GetComponent<AnimController>() != null)
        {
            Debug.Log("Ragdoll component found on server");
        }
        // else
        {
            Debug.Log("Ragdoll compoenent not found on server");
        }

        RpcTurnonRagdoll(sender, hitdirection);
    }


    #region LATEST SPAWNS

    [Command]
    //public void CmdSpawnLoot(List<SpawnLoot> spawndata, MirrorBasics.Player player, string code, Vector3 pos,
    //    string prefab)
    //{
    //    GameObject crate;
    //    if (spawndata[0].itemLength==1)
    //    {
    //        crate = Resources.Load("Items/" + spawndata[0].name, typeof(GameObject)) as GameObject;
    //    }
    //    else {
    //        crate = Resources.Load("Crates/" + prefab, typeof(GameObject)) as GameObject;
    //    }

       
    //    if (crate != null)
    //    {
    //        Debug.Log("PREFAB INITIATED 1 " + crate.name);
    //        GameObject lootBox = Instantiate(crate, pos, Quaternion.identity);
    //        NetworkServer.Spawn(lootBox);
    //        AddToServerRoom(lootBox, player, code);
    //        lootBox.GetComponent<LootCrate>().SetLoot(spawndata);
    //        Debug.Log("PREFAB INITIATED 2");
    //    }
    //}

    //[Command]
    //public void CmdSpawnDrone(Spawndrones spawndata, MirrorBasics.Player player, string code, Vector3 pos)
    //{

    //    GameObject drone = null ;
    //    switch (spawndata.clusterType)
    //    {

    //        case "small":
    //             drone = Instantiate(HypernavAgent, pos, Quaternion.identity);

    //            break;
    //        case "medium":
    //             drone = Instantiate(HypernavAgent, pos, Quaternion.identity);
    //            break;
    //        case "boss":

    //             drone = Instantiate(HypernavAgent, pos, Quaternion.identity);
    //            break;

    //    }
    //    NetworkServer.Spawn(drone);
    //    AddToServerRoom(drone, player, code);
    //    foreach (DroneData d in StaticData.instance.alldata.dronesData.data)
    //    {
    //        if (d.name.Equals(spawndata.droneType))
    //        {
    //            Debug.Log(spawndata.droneType + "   "+d);
    //            drone.GetComponent<AgentController>().droneData = d;
    //            break;
    //        }
    //    }

    //}

    //[Command]
    //public void CmdSpawnExtractions(string code, List<ExtractionPosition> extractions, List<Color> colors, Player player)
    //{
    //    foreach (ExtractionPosition extraction in extractions)
    //    {
    //        Vector3 spawnPosExtraction1 = UILobby.instance.extractionsPositionParent.GetChild(extraction.posId1).position;
    //        Vector3 spawnPosExtraction2 = UILobby.instance.extractionsPositionParent.GetChild(extraction.posId2).position;
    //        GameObject extraction1 = Instantiate(extractionPrefab, spawnPosExtraction1, Quaternion.identity);
    //        GameObject extraction2 = Instantiate(extractionPrefab, spawnPosExtraction2, Quaternion.identity);
    //        NetworkServer.Spawn(extraction1);
    //        NetworkServer.Spawn(extraction2);
    //        AddToServerRoom(extraction1, player, code);
    //        AddToServerRoom(extraction2, player, code);
    //        extraction1.GetComponent<Extraction>().color = colors[extraction.team - 1];
    //        extraction1.GetComponent<Extraction>().team = extraction.team;
    //        extraction2.GetComponent<Extraction>().color = colors[extraction.team - 1];
    //        extraction2.GetComponent<Extraction>().team = extraction.team;

    //    }
  //  }
    #endregion

    //[TargetRpc]
    //public void TargetRpcDamageEnemy(NetworkIdentity target, NetworkIdentity imposter, Vector3 hitdirection)
    //{
    //    RagdollGameobject.GetComponent<RagdollManager>().OnDie(hitdirection);
    //    RagdollGameobject.transform.parent = null;
    //    Debug.Log("Target ID =" + target.GetComponent<Player>().userId + "  imposter id =" + imposter.GetComponent<Player>().userId);
    //    RoomContoller.SocketMaster.instance.AddEventData(target.GetComponent<Player>().userId, imposter.GetComponent<Player>().userId, "headshoot", 1);

    //}

    [ClientRpc]
    public void RpcDamageEnemy(NetworkIdentity target, NetworkIdentity imposter, Vector3 hitdirection)
    {
      //  target.GetComponent<shootbullet>().RagdollGameobject.GetComponent<RagdollManager>().OnDie(hitdirection);
        //target.GetComponent<shootbullet>().RagdollGameobject.transform.parent = null;
      //  Debug.Log("Target ID =" + target.GetComponent<Player>().userId + "  imposter id =" + imposter.GetComponent<Player>().userId);
      //  RoomContoller.SocketMaster.instance.AddEventData(target.GetComponent<Player>().userId, imposter.GetComponent<Player>().userId, "headshoot", 1);

    }

    //[TargetRpc]
    //public void TargetRpcDamageDummy(NetworkIdentity imposter, NetworkIdentity target, Vector3 hitdirection)
    //{
    //    RagdollGameobject.GetComponent<RagdollManager>().OnDie(hitdirection);
    //    RagdollGameobject.transform.parent = null;
    //    Debug.Log("Target ID =" + target.GetComponent<DummyPlayer>().DummyId + "  imposter id =" + imposter.GetComponent<Player>().userId);
    //    RoomContoller.SocketMaster.instance.AddEventData(target.GetComponent<DummyPlayer>().DummyId, imposter.GetComponent<Player>().userId, "headshoot", 1);

    //}

    [ClientRpc]
    public void RpcDamageDummy(NetworkIdentity imposter, NetworkIdentity target, Vector3 hitdirection)
    {
      //  RagdollGameobject.GetComponent<RagdollManager>().OnDie(hitdirection);
        //RagdollGameobject.transform.parent = null;
      //  Debug.Log("Target ID =" + target.GetComponent<DummyPlayer>().DummyId + "  imposter id =" + imposter.GetComponent<Player>().userId);
     //   RoomContoller.SocketMaster.instance.AddEventData(target.GetComponent<DummyPlayer>().DummyId, imposter.GetComponent<Player>().userId, "headshoot", 1);

    }


    [ClientRpc]
    public void RpcTurnonRagdoll(NetworkIdentity senderu, Vector3 hitdirectionu)
    {
        Debug.LogWarning("****************************************************************");
       // senderu.GetComponent<AnimController>().OnDie(hitdirectionu);
    }



    [ClientRpc]
    public void RpcDamage(NetworkIdentity sender, NetworkIdentity id)
    {
        Player P = sender.GetComponent<Player>();
        Debug.Log("SHOOTING RPC" + sender.netId + "__" + sender.GetComponent<Player>().Health);
        if (sender == MirrorBasics.Player.localPlayer.GetComponent<NetworkIdentity>())
        {
            if (P.Health <= 20 && P.Health > 10)
            {
           //     RoomContoller.SocketMaster.instance.AddEventData(P.userId, id.GetComponent<Player>().userId, "headshoot", 1);
                //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                //GameObject spectatorlist = GameObject.Find("SpectatorList");
                //spectatorlist.GetComponent<SpectatorList>().RemovefromSpectatorList(P.transform.gameObject);
                Debug.Log("Removed itself from list");
            }
            Debug.Log("Hit My Self");
        }
        else
        {
            if (P.Health <= 20 && P.Health > 10)
            {
                //GameObject spectatorlist = GameObject.Find("SpectatorList");
                //spectatorlist.GetComponent<SpectatorList>().RemovefromSpectatorList(P.transform.gameObject);
                //Debug.Log("Removed itself from list");
                //sender.gameObject.SetActive(false);
            }
            Debug.Log("Opponent");
            //sender.gameObject.SetActive(false);
        }
    }



    private void ResetShot()
    {
        readytoshoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloadig = true;
        Invoke("ReloadFinished", reloadtime);
    }

    private void ReloadFinished()
    {
        bulletLeft = magzieSize;
        reloadig = false;
    }


    [Command]
    public void CmdUpdateNameofPlayer(string _Playername)
    {
        SetDisplayName(_Playername);
    }

    public void SetDisplayName(string newDisplayname)
    {
        // displayname = newDisplayname;
    }

    private void HandleDisplayName(string oldname, string newname)
    {
        // displaynameText.text = newname;
    }



    





}
