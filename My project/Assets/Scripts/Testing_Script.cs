using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing_Script : MonoBehaviour
{
    [SerializeField] private Slider hpBar;

    private int hp;

    [SerializeField] private Image handle;

    [SerializeField] private Sprite[] heart;

    [SerializeField] private TMPro.TextMeshProUGUI textnumber;

    private int number;

    bool checkOne;
    bool checkTwo;
    bool checkThree;

    // Start is called before the first frame update
    void Start()
    {
        hpBar.maxValue = 4;
        hpBar.minValue = 0;
        hp = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            hp++;
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            hp--;
        }

        hpBar.value = hp;
        changeImage();

        if(number != 10)
        {
            textnumber.text = number.ToString();
        }
        else
        {
            textnumber.text = "Unlocked!!!";
        }
    }

    void changeImage()
    {
        if(hp == 4)
        {
            handle.sprite = heart[1];
        }
        else if (hp == 3)
        {
            handle.sprite = heart[2];
        }
        else if (hp == 2)
        {
            handle.sprite = heart[3];
        }
        else if (hp == 1)
        {
            handle.sprite = heart[4];
        }
        else
        {
            handle.sprite = heart[5];
        }
    }

    public void increase()
    {
        number++;
    }

    public void decrease()
    {
        number--;
    }
}
