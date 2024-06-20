using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using Photon.Pun;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]private PlayerHandler playerHandler;
    [SerializeField]private MultiPlayerHandler multiPlayerHandler;
    [SerializeField]private Animator animator;
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    [SerializeField] private Rig rig;

    [Header("Weapon")]
    public GameObject[] weapons;
    public List<GameObject> weaponsUnlocked;
    public int weaponSelected = 0;


    [Header("Camera")]
    [SerializeField] public GameObject playerCameraFollowPosition;

    [Header("Aiming")]
    [SerializeField] private GameObject crossHair;
    [SerializeField] float aimCameraSensitivity, normalCameraSensitivity,characterRotateSensitivity,rayCastLength;
    bool isAiming = false;
    [SerializeField] LayerMask aimMask;
    [SerializeField] GameObject aimPointer;

    [Header("Shooting")]
    [SerializeField] bool isProjectileShooting;
    [SerializeField] GameObject shootBtn;
    [SerializeField] GameObject changeWeapon;
    [SerializeField] GameObject dropWeaponBtn;
    [SerializeField] GameObject sniperCross;
    [SerializeField] private float defaultShootInterval;
    private float currentShootInterval;
    [SerializeField] private float bulletlife;
    [SerializeField] private float bulletSpeed;
    [SerializeField] Transform[] shootPoint;
    [SerializeField] GameObject bulletPrafab;
    [SerializeField] private ParticleSystem muzzleFlash;
    public int currentShootPoint = 0;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        currentShootInterval = defaultShootInterval;
    }
    private void Start()
    {
#if ENABLE_INPUT_SYSTEM && (UNITY_IOS || UNITY_ANDROID)
        FindObjectOfType<MobileDisableAutoSwitchControls>().playerInput = GetComponent<PlayerInput>();
        FindObjectOfType<MobileDisableAutoSwitchControls>().DisableAutoSwitchControls();
#endif
        shootBtn = GameObject.Find("UI_Virtual_Button_Shoot");
        changeWeapon = GameObject.Find("WeaponChangeBtn");
        dropWeaponBtn = GameObject.Find("WeaponDropBtn");
        crossHair = GameObject.Find("Crosshair");
        sniperCross = GameObject.Find("SniperCross");
        aimVirtualCamera = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();
        aimVirtualCamera.Follow = playerCameraFollowPosition.transform;
        GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = playerCameraFollowPosition.transform;
       shootBtn.SetActive(false);
       changeWeapon.SetActive(false);
       dropWeaponBtn.SetActive(false);
        crossHair.SetActive(false);
        sniperCross.SetActive(false);
        aimVirtualCamera.gameObject.SetActive(false);
        GamePlayHandler.Instance.thirdPersonShooterController = this;
    }
    private void Update()
    {
        if (playerHandler.isDead) return;
        //if (weaponsUnlocked.Count > 0)
        //{
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        SetAim(true);
        //    }
        //    else if (Input.GetMouseButtonUp(1))
        //    {
        //        SetAim(false);
        //    }
        //    else if (!isAiming && Input.GetAxis("Mouse ScrollWheel") != 0)
        //    {
        //        weaponSelected++;
        //        if (weaponSelected >= weaponsUnlocked.Count) weaponSelected = 0;
        //        multiPlayerHandler.SetSelectedGun(weaponSelected);
        //        if (weaponsUnlocked[weaponSelected] == weapons[0]) animator.SetFloat("weapon", 1f);
        //        else animator.SetFloat("weapon", 0f);
        //        SetShootPoint();
        //    }
        //}
           
        Aim();
       
    }
    public void ChangeWeapon()
    {
        if (!isAiming)
        {
            weaponSelected++;
            if (weaponSelected >= weaponsUnlocked.Count) weaponSelected = 0;
            multiPlayerHandler.SetSelectedGun(weaponSelected);
            if (weaponsUnlocked[weaponSelected] == weapons[0]) animator.SetFloat("weapon", 1f);
            else animator.SetFloat("weapon", 0f);
            SetShootPoint();
        }
    }
    void Aim()
    {
        if (isAiming)
        {
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, rayCastLength, aimMask))
            {
                aimPointer.transform.position = raycastHit.point;
            }
            Vector3 worldAimTarget = aimPointer.transform.position;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * characterRotateSensitivity);
            Shoot();
        }
    }
    public void SetShootPoint()
    {
        if (weaponsUnlocked[weaponSelected] == weapons[0]) currentShootPoint = 0;
        else if (weaponsUnlocked[weaponSelected] == weapons[1]) currentShootPoint = 1;
        else if (weaponsUnlocked[weaponSelected] == weapons[2]) currentShootPoint = 2;
    }
    void Shoot()
    {
        if (starterAssetsInputs.shoot)
        {
            currentShootInterval -= Time.deltaTime;
            if(currentShootInterval<=0)
            {
                if(isProjectileShooting)
                {
                    GetComponent<PhotonView>().RPC("Shoot", RpcTarget.Others);
                    Vector3 aimDirection = (aimPointer.transform.position - shootPoint[currentShootPoint].position).normalized;
                    currentShootInterval = defaultShootInterval;
                    GameObject bullet = Instantiate(bulletPrafab, shootPoint[currentShootPoint].position, Quaternion.LookRotation(aimDirection, Vector3.up));
                    bullet.GetComponent<Bullet>().shoot(bulletSpeed, bulletlife);
                    if (muzzleFlash != null) muzzleFlash.Play();
                }
                else
                {

                }
                
            }
        }
        else
        {
            currentShootInterval = 0f;
        }
    }
    public void SetAim(bool isAim)
    {
        if (weaponsUnlocked.Count > 0)
        {
            if (playerHandler.isDead) return;
            isAiming = isAim;
            changeWeapon.GetComponent<Button>().interactable = !isAim;
            dropWeaponBtn.GetComponent<Button>().interactable = !isAim;
            crossHair.SetActive(isAim);
            aimVirtualCamera.gameObject.SetActive(isAim);
            aimPointer.SetActive(isAim);
            shootBtn.SetActive(isAim);
            thirdPersonController.setAimState(isAim);
            animator.SetBool("Aim", isAim);
            weaponsUnlocked[weaponSelected].SetActive(isAim);
            if (weaponsUnlocked[weaponSelected] == weapons[2])
            {
                sniperCross.SetActive(isAim);
                if (isAim)
                {
                    aimVirtualCamera.m_Lens.FieldOfView = 15;
                }
                else
                {
                    aimVirtualCamera.m_Lens.FieldOfView = 30;
                }
            }
            if (isAim)
            {
                rig.gameObject.SetActive(true);
                rig.weight = 1f;
                thirdPersonController.setCameraSensitivity(aimCameraSensitivity);
            }
            else
            {
                rig.weight = 0f;
                rig.gameObject.SetActive(false);
                thirdPersonController.setCameraSensitivity(normalCameraSensitivity);
            }
            if (playerHandler.GetIsMultiPlayer())
            {
                multiPlayerHandler.SetGun(isAim);
            }
        }
    }
    public bool GetIsAmingState()
    {
        return isAiming;
    }
    public void removeAimOnDead()
    {
        isAiming = false;
        crossHair.SetActive(false);
        aimVirtualCamera.gameObject.SetActive(false);
        aimPointer.SetActive(false);
        shootBtn.SetActive(false);
        thirdPersonController.setAimState(false);
    }
    public void dropWeapon()
    {
        weaponsUnlocked.RemoveAt(0);
        if (weaponsUnlocked.Count <= 0)
        {
            dropWeaponBtn.SetActive(false);
            changeWeapon.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Weapon"&& weaponsUnlocked.Count<2)
        {
            if (!weaponsUnlocked.Contains(weapons[other.gameObject.GetComponent<Item>().itemNo]))
            {
                weaponsUnlocked.Add(weapons[other.gameObject.GetComponent<Item>().itemNo]);
                multiPlayerHandler.SetAddGun(other.gameObject.GetComponent<Item>().itemNo);
                if (weaponsUnlocked[weaponSelected] == weapons[0]) animator.SetFloat("weapon", 1f);
                else animator.SetFloat("weapon", 0f);
                SetShootPoint();
                changeWeapon.SetActive(true);
                dropWeaponBtn.SetActive(true);
            }
            Destroy(other.gameObject);
        }
       

    }
}
