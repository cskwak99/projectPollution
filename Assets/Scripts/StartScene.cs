using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    public GameObject backgroundBox;
    public float time;
    public float nextTime;
    public bool flag1;
    public bool flag2;
    public bool flag3;
    public bool flag4;
    public bool flag5;
    public bool flag6;
    public bool flag7;
    public bool flag8;
    public bool flag9;
    public bool flag10;
    public bool flag11;

    public string storyString;
    public Text backgroundText;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        nextTime = 1.0f;
        flag1 = true;
        flag2 = false;
        flag3 = false;
        flag4 = false;
        flag5 = false;
        flag6 = false;
        flag7 = false;
        flag8 = false;
        flag9 = false;
        flag10 = false;
        flag11 = false;

        backgroundText = backgroundBox.GetComponent<Text>();
        storyString = "";
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= nextTime * 1.0f && flag1){
            flag1 = false;
            flag2 = true;
            storyString = storyString + "The previous civilization is gone, leaving only ruins behind.\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 2.0f && flag2){
            flag2 = false;
            flag3 = true;
            storyString = storyString + "But nobody really cares about that, the priority is survival in the desolate land left with so few resources.\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 3.0f && flag3){
            flag3 = false;
            flag4 = true;
            storyString = storyString + "Some information managed to pass through time, however. \n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 4.0f && flag4){
            flag4 = false;
            flag5 = true;
            storyString = storyString + "Something about vaccines causing autism, as such the population divided themselves.\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 5.0f && flag5){
            flag5 = false;
            flag6 = true;
            storyString = storyString + "One where the Noble class, the Intellectual. \n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 6.0f && flag6){
            flag6 = false;
            flag7 = true;
            storyString = storyString + "They live forever in a dome so as to not get sick and claim that such a sacrifice is rewarded by intellectual superiority, as said by the ancient text.\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 7.0f && flag7){
            flag7 = false;
            flag8 = true;
            storyString = storyString + "The others are the workers, \"not the brightest\" said the Noble, \"but necessary to gather resources for us chosen.\"\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 8.0f && flag8){
            flag8 = false;
            flag9 = true;
            storyString = storyString + "People now live in city-state,\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 9.0f && flag9){
            flag9 = false;
            flag10 = true;
            storyString = storyString + "But conflict is bound to happen in a world with so few resources left.\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 10.0f && flag10){
            flag10 = false;
            flag11 = true;
            storyString = storyString + "However one of the thing that all Nobles agree is that armed conflict is likely the reason why the previous civilization disappeared.\n";
            backgroundText.text = storyString;
        }else if(time >= nextTime * 11.0f && flag11){
            flag11 = false;
            storyString = storyString + "As such a new mean of conflict has been developed.";
            backgroundText.text = storyString;
        }
    }

    public void startGame(){
        SceneManager.LoadScene("Playtest2");
    }
}
