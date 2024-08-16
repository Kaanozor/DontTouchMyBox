using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherBoxes : MonoBehaviour
{
    float health = 100;
    public Image healthImage;

    public GameObject healthCanvas;
    AudioSource explosionSound;

    PhotonView pw;

    GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        explosionSound = GetComponent<AudioSource>();
        pw = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void GetHit(float damage)
    {
        if (pw.IsMine)
        {
            health -= damage;
            healthImage.fillAmount = health / 100;

            if (health <= 0)
            {
                //gameManager.GetComponent<GameManager>().PlayEffects("box", gameObject);

                explosionSound.Play();
                PhotonNetwork.Instantiate("DrillAirHit", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);

            }
            else
            {
                StartCoroutine(ShowCanvas());
            }
        }
    }

    IEnumerator ShowCanvas()
    {
        if (!healthCanvas.activeInHierarchy)
        {
            healthCanvas.SetActive(true);
            yield return new WaitForSeconds(2f);
            healthCanvas.SetActive(false);
        }
    }
}
