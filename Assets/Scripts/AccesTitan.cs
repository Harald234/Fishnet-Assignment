using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Component.Animating;

public class AccesTitan : NetworkBehaviour
{

    public GameObject titan;
    public MeshRenderer body;

    public GameObject spawnedTitan;

    public bool inTitan;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {

        }
        else
        {
            GetComponent<AccesTitan>().enabled = false;    
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && spawnedTitan == null)
        {
            SpawnTitan(titan, transform, this);
        }

        if (Input.GetKeyDown(KeyCode.E) && !inTitan && spawnedTitan != null)
        {
            StartCoroutine(HandleEmbark());
        }
        else if (Input.GetKeyDown(KeyCode.E) && inTitan)
        {
            UnembarkServer(gameObject);
        }
    }

    IEnumerator HandleEmbark()
    {
        StartEmbarkServer(spawnedTitan, "Embark");

        yield return new WaitForSeconds(2.5f);

        EndEmbarkServer(gameObject);
    }


    [ServerRpc]
    public void SpawnTitan(GameObject titan, Transform player, AccesTitan script)
    {
        GameObject Titan = GameObject.Instantiate (titan, player.position + player.forward * 5f + player.up * -1f, titan.transform.rotation);
        ServerManager.Spawn(Titan);
        SetTitanObject(Titan, script);
    }

    [ObserversRpc]
    public void SetTitanObject(GameObject Titan, AccesTitan script)
    {
        script.spawnedTitan = Titan;
        Titan.transform.Rotate(0, 90f, 0);
    }


    [ServerRpc]
    public void StartEmbarkServer(GameObject titan, string animation)
    {
        StartEmbark(titan, animation);
    }

    [ServerRpc]
    public void EndEmbarkServer(GameObject player)
    {
        EndEmbark(player);
    }

    [ObserversRpc]
    public void StartEmbark(GameObject titan, string animation)
    {
        titan.GetComponent<Animator>().SetTrigger(animation);
    }

    [ObserversRpc]
    public void EndEmbark(GameObject player)
    {
        player.GetComponent<AccesTitan>().body.enabled = false;
        inTitan = true;
    }


    [ServerRpc]
    public void UnembarkServer(GameObject player)
    {
        Unembark(player);
    }

    [ObserversRpc]
    public void Unembark(GameObject player)
    {
        inTitan = false;
        player.GetComponent<AccesTitan>().body.enabled = true;
    }
}