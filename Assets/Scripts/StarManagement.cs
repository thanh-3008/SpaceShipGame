
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarManagement : MonoBehaviour
{
    public List<Image> starList;
    public Sprite starGoc;
    public Sprite starActive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HienThiStar(int level) 
    {
        for (int i = 0; i < starList.Count; i++)
        {
            starList[i].sprite = starGoc;
        }

        for (int i = 0; i < starList.Count; i++)
        {
            if( i <= level)
            {
                starList[i].sprite = starActive;
            }
        }
    }
}
