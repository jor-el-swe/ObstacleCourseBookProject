using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 8) {
            SceneManager.LoadScene("main");
        }
    }
}
