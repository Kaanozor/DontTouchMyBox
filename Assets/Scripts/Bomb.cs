using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    float damage = 20;

    GameObject gameManager;
    GameObject player;
    AudioSource bombExplosionSound;

    string whoAmI;

    PhotonView pw;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        bombExplosionSound = GetComponent<AudioSource>();
        pw = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void SetTag(string tag)
    {
        player = GameObject.FindGameObjectWithTag(tag);

        whoAmI = tag;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("OtherBox"))
        {
            collision.GetComponent<PhotonView>().RPC("GetHit", RpcTarget.All, damage);

            player.GetComponent<Player>().MovePowerBar();

            if (pw.IsMine)
            {
                bombExplosionSound.Play();
                PhotonNetwork.Instantiate("FireExplosion", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            player.GetComponent<Player>().MovePowerBar();

            if (pw.IsMine)
            {
                bombExplosionSound.Play();
                PhotonNetwork.Instantiate("FireExplosion", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            player.GetComponent<Player>().MovePowerBar();

            if (pw.IsMine)
            {
                bombExplosionSound.Play();
                PhotonNetwork.Instantiate("FireExplosion", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Prize"))
        {
            gameManager.GetComponent<PhotonView>().RPC("GetHeart", RpcTarget.All, whoAmI);

            player.GetComponent<Player>().MovePowerBar();

            PhotonNetwork.Destroy(collision.transform.gameObject);

            if (pw.IsMine)
            {
                bombExplosionSound.Play();
                PhotonNetwork.Instantiate("FireExplosion", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("PlayerTower1"))
        {
            gameManager.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, "Player1", damage);

            player.GetComponent<Player>().MovePowerBar();

            if (pw.IsMine)
            {
                bombExplosionSound.Play();
                PhotonNetwork.Instantiate("FireExplosion", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("PlayerTower2"))
        {
            gameManager.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All, "Player2", damage);

            player.GetComponent<Player>().MovePowerBar();

            if (pw.IsMine)
            {
                bombExplosionSound.Play();
                PhotonNetwork.Instantiate("FireExplosion", transform.position, transform.rotation, 0, null);
                PhotonNetwork.Destroy(gameObject);
            }
        }

    }
}
