using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartItem : MonoBehaviour
{
    [SerializeField]
    public TMP_Text nameText, countText, priceText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setNameText(string name)
    {
        nameText.text = name;
    }

    public void setCountText(string count)
    {
        countText.text = count;
    }
    public void setPriceText(string price)
    {
        priceText.text = price;
    }
}
