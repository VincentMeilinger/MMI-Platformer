using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLoop : MonoBehaviour
{
    [SerializeField]
    private GameObject player;



    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (player == null) { return; }
        if (player.transform.position.y < -10) {
            player.SendMessage("Death");
            player.SendMessage("Reset");
        }
        if (Input.GetButton("Cancel")) {
            player.SendMessage("Reset");
        }
    }

}
