using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject bomb;
    public GameObject bombSpawnPoint;
    public ParticleSystem bombFireEffect;
    float fireDirection;
    bool fireEnabled = true;

    public AudioSource bombFireSound;

    [Header("PowerBarSettings")]
    Image powerBarImage;
    float powerCounter;
    bool hasEnded = false;

    Coroutine powerBarCycle;

    PhotonView pw;

    void Start()
    {
        powerBarImage = GameObject.FindGameObjectWithTag("PowerBar").GetComponent<Image>();

        pw = GetComponent<PhotonView>();

        if (pw.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = GameObject.FindGameObjectWithTag("Player1SpawnPoint").transform.position;
                transform.rotation = GameObject.FindGameObjectWithTag("Player1SpawnPoint").transform.rotation;
                //gameObject.tag = "Player1";
                fireDirection = 1;
            }
            else
            {
                transform.position = GameObject.FindGameObjectWithTag("Player2SpawnPoint").transform.position;
                transform.rotation = GameObject.FindGameObjectWithTag("Player2SpawnPoint").transform.rotation;
                //gameObject.tag = "Player2";
                fireDirection = -1;
            }
        }

        InvokeRepeating("HasGameStarted", 0, 0.5f);
    }

    void Update()
    {
        if (pw.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && fireEnabled)
            {
                PhotonNetwork.Instantiate("HitSmokePuff", bombSpawnPoint.transform.position, bombSpawnPoint.transform.rotation, 0, null);
                bombFireSound.Play();

                GameObject theBomb = PhotonNetwork.Instantiate("Bomb", bombSpawnPoint.transform.position, bombSpawnPoint.transform.rotation, 0, null);
                theBomb.GetComponent<PhotonView>().RPC("SetTag", RpcTarget.All, gameObject.tag);
                theBomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(fireDirection, 0) * 25 * powerBarImage.fillAmount, ForceMode2D.Impulse);

                fireEnabled = false;

                StopAllCoroutines();
            }
        }
    }

    public void Result(int value)
    {
        if (pw.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {

                if (value == 1)
                {
                    PlayerPrefs.SetInt("TotalMatches", PlayerPrefs.GetInt("TotalMatches") + 1);
                    PlayerPrefs.SetInt("TotalWins", PlayerPrefs.GetInt("TotalWins") + 1);
                    PlayerPrefs.SetInt("TotalPoints", PlayerPrefs.GetInt("TotalPoints") + 5);
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatches", PlayerPrefs.GetInt("TotalMatches") + 1);
                    PlayerPrefs.SetInt("TotalLoses", PlayerPrefs.GetInt("TotalLoses") + 1);
                }
            }
            else
            {
                if (value == 2)
                {
                    PlayerPrefs.SetInt("TotalMatches", PlayerPrefs.GetInt("TotalMatches") + 1);
                    PlayerPrefs.SetInt("TotalWins", PlayerPrefs.GetInt("TotalWins") + 1);
                    PlayerPrefs.SetInt("TotalPoints", PlayerPrefs.GetInt("TotalPoints") + 5);
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatches", PlayerPrefs.GetInt("TotalMatches") + 1);
                    PlayerPrefs.SetInt("TotalLoses", PlayerPrefs.GetInt("TotalLoses") + 1);
                }
            }
        }
    }

    public void HasGameStarted()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            if (pw.IsMine)
            {
                powerBarCycle = StartCoroutine(PowerBar());
                CancelInvoke("HasGameStarted");
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void MovePowerBar()
    {
        powerBarCycle = StartCoroutine(PowerBar());
    }

    IEnumerator PowerBar()
    {
        powerBarImage.fillAmount = 0;
        hasEnded = false;

        fireEnabled = true;

        while (true)
        {
            if (powerBarImage.fillAmount < 1 && !hasEnded)
            {
                powerCounter = 0.01f;
                powerBarImage.fillAmount += powerCounter;

                yield return new WaitForSeconds(0.001f * Time.deltaTime);
            }
            else
            {
                hasEnded = true;

                powerCounter = 0.01f;
                powerBarImage.fillAmount -= powerCounter;

                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                if (powerBarImage.fillAmount == 0)
                {
                    hasEnded = false;
                }
            }
        }
    }
}
