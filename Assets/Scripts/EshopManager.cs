using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class EshopManager : MonoBehaviour
{
    public TMP_Text sumText;
    [SerializeField] public Transform container;
    [SerializeField] public GameObject prefab;
    [SerializeField] public Transform cartContainer;
    [SerializeField] public GameObject cartPrefab;
    [SerializeField]
    private List<EshopItem> cartItems = new List<EshopItem>();

    // Define a class to represent the data structure
    public class Phone
    {
        public string Name { get; set; }
        public double Price { get; set; }
    }
    string json = @"
    [
      {
        ""Name"": ""Apple iphone 11"",
        ""Price"": 500.0
      },
      {
        ""Name"": ""Apple iphone 13 mini"",
        ""Price"": 700.0
      },
      {
        ""Name"": ""Huawei Nova 5"",
        ""Price"": 422.2
      },
      {
        ""Name"": ""Huawei Nova Y90"",
        ""Price"": 350.0
      },
      {
        ""Name"": ""Iphone 12 Pro Max"",
        ""Price"": 800.0
      },
      {
        ""Name"": ""Iphone 14 Pro"",
        ""Price"": 1000.0
      },
      {
        ""Name"": ""Iphone 15 Pro"",
        ""Price"": 1500.0
      },
      {
        ""Name"": ""Lenovo P2"",
        ""Price"": 250.0
      },
      {
        ""Name"": ""Razer Phone"",
        ""Price"": 500.0
      },
      {
        ""Name"": ""Samsung Galaxy A12"",
        ""Price"": 650.0
      },
      {
        ""Name"": ""Samsung Galaxy S21"",
        ""Price"": 900.0
      },
      {
        ""Name"": ""Huawei P30 lite"",
        ""Price"": 460.0
      },
      {
        ""Name"": ""Xiaomi Mi 9 Pro"",
        ""Price"": 700.0
      },
      {
        ""Name"": ""Xiaomi Mi 10 Pro"",
        ""Price"": 500.0
      },
      {
        ""Name"": ""Xiaomi Redmi 11 Pro"",
        ""Price"": 650.0
      }
    ]";


    // Start is called before the first frame update
    void Start()
    {
        populateContainer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItem(string name, string price, string iconName)
    {
        GameObject item = Instantiate(prefab, container);
        if (item.TryGetComponent<EshopItem>(out EshopItem eshopItem))
        {
            eshopItem.setName(name);
            eshopItem.setPrice(price);
            eshopItem.setImage(iconName);
            eshopItem.setCartList(cartItems);
        }
    }

    public void populateContainer()
    {
        //List<Phone> phones = JsonConvert.DeserializeObject<List<Phone>>("Assets/Images/Eshop/eshop.json");
        List<Phone> phones = JsonConvert.DeserializeObject<List<Phone>>(json);
        int i = 0;
        foreach (Phone phone in phones)
        {
            addItem(phone.Name, phone.Price.ToString(), "Assets/Images/Eshop/Images/" + i.ToString() + ".png");
            i++;
        }
    }

    public void showCartItems()
    {
        EmptyContainer(cartContainer);
        // Count the occurrences of each EshopItem
        var itemOccurrences = cartItems
            .GroupBy(item => item)
            .Select(group => new
            {
                Item = group.Key,
                Count = group.Count(),
                Price = group.Key.getPrice()
            });

        float sum = 0.0f;

        // Display the results
        foreach (var occurrence in itemOccurrences)
        {
            GameObject item = Instantiate(cartPrefab, cartContainer);
            if (item.TryGetComponent<CartItem>(out CartItem cartItem))
            {
                cartItem.setNameText(occurrence.Item.getName());
                cartItem.setCountText(occurrence.Count.ToString());
                cartItem.setPriceText((occurrence.Price * occurrence.Count).ToString());
                sum += occurrence.Price * occurrence.Count;
            }
        }
        sumText.text = sum.ToString() + " $";
    }

    public void EmptyContainer(Transform container)
    {
        // Iterate through each child of the container
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            // Destroy the child game object
            Destroy(child.gameObject);
        }
    }
}
