using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePanel : MonoBehaviour
{
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private Image northImage;
    [SerializeField] private Image southImage;
    [SerializeField] private Image eastImage;
    [SerializeField] private Image westImage;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        northImage.enabled = false;
        southImage.enabled = false;
        eastImage.enabled = false;
        westImage.enabled = false;
    }

    public void ReceiveDamage()
    {
        if (gameManager.LastDamagePosition.Position == Vector3.zero)
        {
            StartCoroutine(StartFlash());
        }
        else
        {
            var damageDirection = gameManager.LastDamagePosition.Position - PlayerContext.instance.transform.position;
            var playerDamageDirection = PlayerContext.instance.transform.InverseTransformDirection(damageDirection);
            var damageAngle = Vector2.SignedAngle(Vector2.up, new Vector2(playerDamageDirection.x, playerDamageDirection.z));
            Debug.Log(playerDamageDirection);
            Debug.Log(damageAngle);
            if (damageAngle >= -67.5f && damageAngle <= 67.5f)
            {
                StartCoroutine(FlashNorth());
            }
            if (damageAngle >= 22.5f && damageAngle <= 157.5f || damageAngle >= -337.5f && damageAngle <= -202.5f)
            {
                StartCoroutine(FlashWest());
            }
            if (damageAngle >= 112.5f && damageAngle <= 247.5f || damageAngle >= -247.5f && damageAngle <= -112.5f)
            {
                StartCoroutine(FlashSouth());
            }
            if (damageAngle >= 202.5f && damageAngle <= 337.5f || damageAngle >= -157.5f && damageAngle <= -22.5f)
            {
                StartCoroutine(FlashEast());
            }
        }
    }

    private IEnumerator StartFlash()
    {
        northImage.enabled = true;
        southImage.enabled = true;
        eastImage.enabled = true;
        westImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        northImage.enabled = false;
        southImage.enabled = false;
        eastImage.enabled = false;
        westImage.enabled = false;
    }

    private IEnumerator FlashNorth()
    {
        northImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        northImage.enabled = false;
    }

    private IEnumerator FlashEast()
    {
        eastImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        eastImage.enabled = false;
    }

    private IEnumerator FlashSouth()
    {
        southImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        southImage.enabled = false;
    }

    private IEnumerator FlashWest()
    {
        westImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        westImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
