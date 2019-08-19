using Photon.Pun;
using System;
using System.IO;
using UnityEngine;
using Photon.Realtime;
public class GameSetupController : MonoBehaviour
{
    // This script will be added to any multiplayer scene
    System.Random random;
    GameObject parentObject;

    void Start()
    {
        random = new System.Random();
        CreatePlayer(); //Create a networked player object for each player that loads into the multiplayer scenes.
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating Player");
        parentObject = (GameObject)PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), RandomPosition(), Quaternion.identity);


    }

    private Vector3 RandomPosition()
    {
        return new Vector3(random.Next(-5, 5), 0, 2);
    }
}