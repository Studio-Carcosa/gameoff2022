using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunQuoteManager : MonoBehaviour
{
    public Quote[] quotes;
    private AudioSource source;
    private Text UItext;
    public KeyCode talkKey = KeyCode.F;
    public float quoteDelay = 10;
    private float curTimer = 0;
    private int random;
    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0, quotes.Length);
        curTimer = quoteDelay;
        source = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        UItext = GameObject.Find("GunQuote").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        curTimer = curTimer + Time.deltaTime;
        Debug.Log(curTimer);
        if (Input.GetKey(talkKey) & curTimer >= quoteDelay) {
            PlayQuote();
            curTimer = 0;
        }
        if (curTimer > quotes[random].displayTime) {
            UItext.text = " ";
        }
    }

    void PlayQuote() {

        random = Random.Range(0, quotes.Length);
        UItext.text = quotes[random].quote;
        UItext.fontSize = quotes[random].fontSize;
        source.clip = quotes[random].clip;
        source.Play();
    }
}
