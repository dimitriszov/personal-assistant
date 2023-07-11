using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class EshopItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text priceText;
    [SerializeField]
    public RawImage image;
    [SerializeField]
    private List<EshopItem> cartItems;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getName()
    {
        return nameText.text;
    }

    public void setName(string name)
    {
        nameText.text = name;
    }
    public float getPrice()
    {
        return float.Parse(priceText.text.Replace(" $", ""));
    }

    public void setPrice(string price)
    {
        priceText.text = price + " $";
    }
    public void setImage(string pathFile)
    {
        image.texture = Resources.Load<Texture2D>(pathFile);
    }

    public void setCartList(List<EshopItem> cartItems)
    {
        this.cartItems = cartItems;
    }

    public void addToCart() 
    {
        cartItems.Add(this);
    }

    // Add Equals and GetHashCode methods for proper grouping and counting
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        EshopItem otherItem = (EshopItem)obj;
        return getName() == otherItem.getName() && getName() == otherItem.getName();
    }

    public override int GetHashCode()
    {
        return Tuple.Create(getName(), getPrice()).GetHashCode();
    }
}
