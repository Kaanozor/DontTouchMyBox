using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    PhotonView pw;

    void Start()
    {
        pw = gameObject.GetComponent<PhotonView>();

        StartCoroutine(DestroyYourself());
    }

    IEnumerator DestroyYourself()
    {
        yield return new WaitForSeconds(10);
        if (pw.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
