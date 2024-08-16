using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject firstPanel;
    public TMP_InputField usernameText;

    public GameObject secondPanel;
    public TMP_Text username;
    GameObject randomButton;
    GameObject createButton;

    public TMP_Text[] scores;

    void Start()
    {
        if (!PlayerPrefs.HasKey("Username"))
        {
            PlayerPrefs.SetInt("TotalMatches", 0);
            PlayerPrefs.SetInt("TotalWins", 0);
            PlayerPrefs.SetInt("TotalLoses", 0);
            PlayerPrefs.SetInt("TotalPoints", 0);

            firstPanel.SetActive(true);

            SetScores();
        }
        else
        {
            secondPanel.SetActive(true);

            username.text = PlayerPrefs.GetString("Username");

            SetScores();
        }
    }

    public void SetUsername()
    {
        PlayerPrefs.SetString("Username", usernameText.text);

        firstPanel.SetActive(false);
        secondPanel.SetActive(true);

        randomButton = GameObject.FindGameObjectWithTag("RandomButton");
        createButton = GameObject.FindGameObjectWithTag("CreateButton");

        randomButton.gameObject.GetComponent<Button>().interactable = true;
        createButton.gameObject.GetComponent<Button>().interactable = true;

        username.text = PlayerPrefs.GetString("Username");
    }

    void SetScores()
    {
        scores[0].text = PlayerPrefs.GetInt("TotalMatches").ToString();
        scores[1].text = PlayerPrefs.GetInt("TotalWins").ToString();
        scores[2].text = PlayerPrefs.GetInt("TotalLoses").ToString();
        scores[3].text = PlayerPrefs.GetInt("TotalPoints").ToString();
    }
}
