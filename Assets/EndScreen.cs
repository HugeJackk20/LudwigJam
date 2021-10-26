using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public PlayerMovement player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.gameComplete == true)
		{
            SceneManager.LoadScene("EndScreen");
            player.gameComplete = false;
        }
    }
}
