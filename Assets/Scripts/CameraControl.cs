using UnityEngine;
using FishNet.Object;

public class CameraControl : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            gameObject.SetActive(true);
        }
    } 
}