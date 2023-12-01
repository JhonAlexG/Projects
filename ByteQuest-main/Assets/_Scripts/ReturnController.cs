using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnController : MonoBehaviour
{
    public Vector3 returnPositionPlayer, returnPositionCodeBlock;

    private void OnTriggerEnter(Collider other) {
            other.gameObject.transform.position = returnPositionPlayer;
    }
}
