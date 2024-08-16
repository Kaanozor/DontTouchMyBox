using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("PlayerSettings")]
    GameObject player1;
    public Image player1HealthBar;
    float player1Health = 100;

    GameObject player2;
    public Image player2HealthBar;
    float player2Health = 100;

    [Header("PrizeSettings")]
    bool hasStarted = false;
    int prizeAmount = 5;
    float standbyTime = 20;
    int prizeCounter;

    public GameObject[] points;

    bool isGameOver;

    PhotonView pw;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }

    IEnumerator SpawnHeartPrize()
    {
        prizeCounter = 0;


        while (hasStarted)
        {
            if (prizeCounter == prizeAmount)
                hasStarted = false;

            yield return new WaitForSeconds(standbyTime);

            int randomNumber = Random.Range(0, 5);
            PhotonNetwork.Instantiate("Prize", points[randomNumber].transform.position, points[randomNumber].transform.rotation, 0, null);

            prizeCounter++;
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    void Winner(int value)
    {
        if (!isGameOver)
        {
            GameObject.FindWithTag("Player1").GetComponent<Player>().Result(value);
            GameObject.FindWithTag("Player2").GetComponent<Player>().Result(value);

            isGameOver = true;
        }
    }

    [PunRPC]
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            hasStarted = true;
            StartCoroutine(SpawnHeartPrize());
        }
    }

    [PunRPC]
    public void GetDamage(string playerName, float damage)
    {

        switch (playerName)
        {
            case "Player1":

                player1Health -= damage;
                player1HealthBar.fillAmount = player1Health / 100;

                if (player1Health <= 0)
                {
                    foreach (GameObject myObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (myObject.gameObject.CompareTag("GameOverPanel"))
                        {
                            myObject.gameObject.SetActive(true);
                            GameObject.FindGameObjectWithTag("GameResult").GetComponent<TextMeshProUGUI>().text = "Player2 Won!";
                        }
                    }

                    Winner(2);

                    /*
                    player1 = GameObject.FindGameObjectWithTag("Player1");
                    player2 = GameObject.FindGameObjectWithTag("Player2");

                    player1.GetComponent<PhotonView>().RPC("Lost", RpcTarget.All);
                    player2.GetComponent<PhotonView>().RPC("Won", RpcTarget.All);
                    */
                }

                break;

            case "Player2":

                player2Health -= damage;
                player2HealthBar.fillAmount = player2Health / 100;

                if (player2Health <= 0)
                {
                    foreach (GameObject myObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (myObject.gameObject.CompareTag("GameOverPanel"))
                        {
                            myObject.gameObject.SetActive(true);
                            GameObject.FindGameObjectWithTag("GameResult").GetComponent<TextMeshProUGUI>().text = "Player1 Won!";
                        }
                    }

                    Winner(1);

                    /*
                    player1 = GameObject.FindGameObjectWithTag("Player1");
                    player2 = GameObject.FindGameObjectWithTag("Player2");

                    player2.GetComponent<PhotonView>().RPC("Lost", RpcTarget.All);
                    player1.GetComponent<PhotonView>().RPC("Won", RpcTarget.All);
                    */
                }

                break;
        }

    }

    [PunRPC]
    public void GetHeart(string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                player1Health += 30;

                if (player1Health >= 100)
                {
                    player1Health = 100;
                    player1HealthBar.fillAmount = 1;
                }
                else
                {
                    player1HealthBar.fillAmount = player1Health / 100;
                }

                break;

            case "Player2":
                player2Health += 30;

                if (player2Health >= 100)
                {
                    player2Health = 100;
                    player2HealthBar.fillAmount = 1;
                }
                else
                {
                    player2HealthBar.fillAmount = player2Health / 100;
                }

                break;
        }
    }
}
