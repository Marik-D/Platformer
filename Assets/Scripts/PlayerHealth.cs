using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 2;

    public Text hpText;
    
    // Start is called before the first frame update
    void Start()
    {
        hpText.text = $"{health} HP";
    }

    public void TakeDamage(int damage)
    {
        health = Math.Max(0, health - damage);
        hpText.text = $"{health} HP";

        if (health == 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.transform.Rotate(Vector3.forward, 90f);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
