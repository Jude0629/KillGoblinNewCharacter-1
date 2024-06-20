using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlayHandler : MonoBehaviourPunCallbacks
{
    public static GamePlayHandler Instance;
   // [SerializeField] TimeHandler timeHandler;
    [SerializeField] int id = 0;
    public GameObject playerPrefab;
    public Transform[] playerPoses;

    public int kills = 0;
    public int score = 0;

    

    public int[] allPlayerKills;


    public GameObject winPanel;
    public GameObject failPanel;
    public GameObject winScorePrefab;
    public Transform winScoreContainer;

    public GameObject deathParticle;

    public ThirdPersonShooterController thirdPersonShooterController;

   
    public GameObject[] buttons;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI scoreOriginalTxt;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
        PhotonNetwork.Instantiate(playerPrefab.name, playerPoses[((PhotonNetwork.LocalPlayer.ActorNumber-1)%playerPoses.Length)].position, Quaternion.identity);
       
        id = PhotonNetwork.LocalPlayer.ActorNumber;
        for(int i=0;i<buttons.Length;i++)
        {
            if(PlayerPrefs.HasKey("B" + i.ToString()))
            {
                buttons[i].transform.position = StringToVector3(PlayerPrefs.GetString("B" + i.ToString()));            }

                
        }
        scoreTxt.text = "";
        AddScore(0);
    }
    public void AddScore(int val)
    {
        score+=val;
        scoreOriginalTxt.text = "Score: " + score;
    }
    private void Update()
    {
        scoreTxt.text = "";
        for (int i=0;i<allPlayerKills.Length;i++)
        {
            if (allPlayerKills[i]!=-1)
            {
                scoreTxt.text += "Player" + (i + 1) + ": " + allPlayerKills[i] + "\n";
            }
            
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
    private void OnApplicationFocus(bool focus)
    {
        PlayerPrefs.Save();
    }
    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.Save();
    }
    public void Home()
    {
        PhotonNetwork.Disconnect();
    }
    public void AddHealth(float val=10)
    {
        thirdPersonShooterController.gameObject.GetComponent<PlayerHandler>().HealthEffect(val);
    }
    public Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeWeapon()
    {
        thirdPersonShooterController.ChangeWeapon();
    } public void DropWeapon()
    {
        thirdPersonShooterController.dropWeapon();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
       
    }
    [PunRPC]
    public void GameEnd(bool isRpc)
    {
        if(!isRpc)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                this.GetComponent<PhotonView>().RPC("GameEnd", RpcTarget.AllBuffered, true);
           
            }
        }
        else
        {

            //timeHandler.time = 0f;

            for(int i= 0;i<8;i++)
            {
                if(i==id-1)
                {
                    GameObject go = Instantiate(winScorePrefab, winScoreContainer);
                    go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "YOU";
                    go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = kills.ToString();
                }
                else
                {
                    if(allPlayerKills[i] != -1)
                    {
                        GameObject go = Instantiate(winScorePrefab, winScoreContainer);
                        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player "+(i+1).ToString();
                        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = allPlayerKills[i].ToString();
                    }
                }
            }
            winPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            if(PhotonNetwork.IsMasterClient)
            {
               
                Invoke("LeaveRoom", 3f);
            }
            else
            {
                LeaveRoom();
            }
        }
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
  
    
}
