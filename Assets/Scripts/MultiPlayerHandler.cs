using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Animations.Rigging;
using StarterAssets;
using UnityEngine.SceneManagement;

public class MultiPlayerHandler : MonoBehaviourPunCallbacks,IPunObservable
{
    [SerializeField]private PlayerHandler playerHandler;
    [SerializeField] private ThirdPersonController thirdPersonController;
    [SerializeField] private ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] private Object[] destroyAbleObjects;
    [SerializeField] GameObject aimPointer;
    [SerializeField] Rig rig;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] gunObject;
    [SerializeField] private List<GameObject> gunUnlocked;
    [SerializeField] int id = 0;
    [SerializeField] int gunSelected = 0;

  


  


 
    [SerializeField] private float bulletlife;
    [SerializeField] private float bulletSpeed;
    [SerializeField] Transform[] shootPoint;
    [SerializeField] GameObject bulletPrafab;
    [SerializeField] private ParticleSystem muzzleFlash;
    public int currentShootPoint = 0;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            GamePlayHandler.Instance.allPlayerKills[id - 1] = GamePlayHandler.Instance.kills;

            stream.SendNext(GamePlayHandler.Instance.kills);
            stream.SendNext(thirdPersonShooterController.GetIsAmingState());
            if (thirdPersonShooterController.GetIsAmingState())
            {
                stream.SendNext(aimPointer.transform.position);
                stream.SendNext(rig.weight);
              
            }
           
        }
        else if (stream.IsReading)
        {
            GamePlayHandler.Instance.allPlayerKills[id - 1] = (int)stream.ReceiveNext();
            if ((bool)stream.ReceiveNext())
            {
               aimPointer.transform.position = (Vector3)stream.ReceiveNext();
                rig.weight = (float)stream.ReceiveNext();
            }
            
        }
    }

    private void Awake()
    {
        playerHandler.SetMultiPlayerState(true, photonView.IsMine);
        if(!photonView.IsMine)
        {
            foreach (Object obj in destroyAbleObjects) { Destroy(obj); }
        }
        else
        {
            this.GetComponent<PhotonView>().RPC("SetID", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        
    }
    [PunRPC]
    void Shoot()
    {
      
                    Vector3 aimDirection = (aimPointer.transform.position - shootPoint[currentShootPoint].position).normalized;
                
                    GameObject bullet = Instantiate(bulletPrafab, shootPoint[currentShootPoint].position, Quaternion.LookRotation(aimDirection, Vector3.up));
                    bullet.GetComponent<Bullet>().shoot(bulletSpeed, bulletlife);
                    if (muzzleFlash != null) muzzleFlash.Play();
      
    }
    public void HealthEffect(float val)
    {
        this.GetComponent<PhotonView>().RPC("ChangeHealth", RpcTarget.All, val);
    }
    [PunRPC]
    public void ChangeHealth(float val)
    {
        playerHandler.ChangeHealth(val);
    }
    [PunRPC]
    public void SetID(int val)
    {
        id = val;
    }
    [PunRPC]
    public void ChangeGunState(bool val)
    {
        gunUnlocked[gunSelected].SetActive(val);
    }
    [PunRPC]
    public void ChangeSelectedGunState(int val)
    {
        gunSelected = val;
    }
    [PunRPC]
    public void AddGunState(int val)
    {
        gunUnlocked.Add(gunObject[val]);
    }
   
    public void dead()
    {
       
            Invoke("death", 3f);
        
    }

    public void death()
    {
        
        Destroy(this.gameObject);

    }
    public void SetAddGun(int val)
    {
        this.GetComponent<PhotonView>().RPC("AddGunState", RpcTarget.Others, val);
    }
    public void SetSelectedGun(int val)
    {
        this.GetComponent<PhotonView>().RPC("ChangeSelectedGunState", RpcTarget.Others, val);
    }
    public void SetGun(bool gunState)
    {
        this.GetComponent<PhotonView>().RPC("ChangeGunState", RpcTarget.Others,gunState);
    }
}
