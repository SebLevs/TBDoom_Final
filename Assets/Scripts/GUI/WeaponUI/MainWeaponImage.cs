using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MainWeaponImage : MonoBehaviour
{
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private Vector3 reloadOffset;
    [SerializeField] private float shakeAmount = 30f;
    [SerializeField] private float shakeTime = 0.1f;
    [SerializeField] private float movementHorizontalAmplitude;
    [SerializeField] private float movementHorizontalFrequency;
    [SerializeField] private float movementVerticalAmplitude;
    [SerializeField] private float movementVerticalFrequency;

    private float shakeTimer = 0;
    private float movementTimer = 0;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private Vector3 currentOffset;
    private CinemachineTransposer m_transposer;
    private Vector3 initialCameraFollowOffset;
    private Vector3 currentCameraFollowOffset;
    private Image weaponImage;

    private Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = PlayerContext.instance.GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
        currentPosition = initialPosition;
        m_transposer = PlayerContext.instance.Camera.GetCinemachineComponent<CinemachineTransposer>();
        initialCameraFollowOffset = m_transposer.m_FollowOffset;
        currentCameraFollowOffset = initialCameraFollowOffset;
        weaponImage = GetComponent<Image>();
        UpdateWeaponImage();
    }

    public void UpdateWeaponImage()
    {
        weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponPlayerSprite;
    }

    public void ShakeWeapon()
    {
        weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponFiringPlayerSprite;
        currentPosition = initialPosition;
        shakeTimer = shakeTime;
    }

    public void MoveGunDown()
    {
        weaponImage.color = Color.grey;
        currentOffset = -reloadOffset;
        //transform.localPosition = currentPosition;
    }

    public void MoveGunUp()
    {
        weaponImage.color = Color.white;
        currentOffset = Vector3.zero;
        transform.localPosition = currentPosition + currentOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector3.zero;
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            var shakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.localPosition = currentPosition + shakeOffset * Time.timeScale;
            var cameraShakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            m_transposer.m_FollowOffset = currentCameraFollowOffset + cameraShakeOffset * 0.005f * Time.timeScale;
        }
        else
        {
            if (weaponImage.sprite != weaponInventory.EquippedMainWeapon.WeaponPlayerSprite)
            {
                weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponPlayerSprite;
            }

            // var testVector = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z);
            if (playerRigidbody.velocity.magnitude > 0.1f && PlayerContext.instance.CurrState is PlayerStateGrounded)
            {
                movementTimer += Time.deltaTime;
                weaponImage.sprite = weaponInventory.EquippedMainWeapon.WeaponPlayerSprite;
                var x = Mathf.Sin(movementTimer * movementHorizontalFrequency) * movementHorizontalAmplitude;
                var y = Mathf.Sin(movementTimer * movementVerticalFrequency) * movementVerticalAmplitude;
                currentPosition = initialPosition + new Vector3(x, y, 0);
                transform.localPosition = currentPosition + currentOffset;
                currentCameraFollowOffset = initialCameraFollowOffset + new Vector3(-x, y, 0) * 0.005f;
                m_transposer.m_FollowOffset = currentCameraFollowOffset;
            }
            else
            {
                currentPosition = initialPosition + currentOffset;
                transform.localPosition = currentPosition;
                currentCameraFollowOffset = initialCameraFollowOffset;
                m_transposer.m_FollowOffset = initialCameraFollowOffset;
                movementTimer = 0;
            }
        }
    }
}
