using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
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

    public string howTo;
    public Text howToText;
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

        howToText = backgroundBox.GetComponent<Text>();
        howTo = "";
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= nextTime * 1.0f && flag1){
            flag1 = false;
            flag2 = true;
            howTo = howTo + "1. Every worker has two Action points\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 2.0f && flag2){
            flag2 = false;
            flag3 = true;
            howTo = howTo + "2. Worker can do move and build, each actions cost 1 action point\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 3.0f && flag3){
            flag3 = false;
            flag4 = true;
            howTo = howTo + "3. You can build Farm and Water pump to feed stupid Anti vaxxers in dome tile\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 4.0f && flag4){
            flag4 = false;
            flag5 = true;
            howTo = howTo + "4. You can build mine to get metal\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 5.0f && flag5){
            flag5 = false;
            flag6 = true;
            howTo = howTo + "5. You can build factory and set zone to pollute\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 6.0f && flag6){
            flag6 = false;
            flag7 = true;
            howTo = howTo + "6. You can build landfill to pollute nearby area\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 7.0f && flag7){
            flag7 = false;
            flag8 = true;
            howTo = howTo + "7. You must put your worker on the building to make it work\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 8.0f && flag8){
            flag8 = false;
            flag9 = true;
            howTo = howTo + "8. If a tile polluted a lot, something bad will happen on it\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 9.0f && flag9){
            flag9 = false;
            flag10 = true;
            howTo = howTo + "9. You can consume worker to purify and reduce pollution\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 10.0f && flag10){
            flag10 = false;
            flag11 = true;
            howTo = howTo + "10. Maybe Anti vaxxer will do some stupid action, but you need to protect them\n";
            howToText.text = howTo;
        }else if(time >= nextTime * 11.0f && flag11){
            flag11 = false;
            howTo = howTo + "Advice: Build Mine before you consume all starting metal\n***You can move screen by mouse right click!!***";
            howToText.text = howTo;
        }
    }

    public void startGame(){
        SceneManager.LoadScene("Playtest2");
    }
}
