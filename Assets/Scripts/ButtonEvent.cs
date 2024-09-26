using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonEvent : MonoBehaviour
{
    //What the button will display to the user
    public Text buttonDisplayText;

    //So we can change pages on click
    public PageManager pageManager;

    //What we should do when we click
    public string action;

    //What number or item or other thing to use
    public string key;

    //Enable us to easily set up a new button
    public void Initialise(string action, string key, string label, PageManager pageManager)
    {
        //"this" is the current script
        this.action = action;
        this.key = key;
        this.pageManager = pageManager;

        buttonDisplayText = GetComponentInChildren<Text>();
        buttonDisplayText.text = label;
    }

    public void Event()
    {
        switch (action)
        {
            case "goto":
                int pageNumber;
                if (key == "back")
                {
                    pageNumber = pageManager.previousPage;
                }
                else
                {
                    pageNumber = int.Parse(key);
                }
                pageManager.LoadPage(pageNumber);
                break;

            case "collect":
                Item item = Enum.Parse<Item>(key);
                pageManager.inventory.Collect(item);

                pageManager.LoadPage(key);

                break;

            default:
                Debug.LogWarning($"The button {this.name} failed to act, with action {action} and key {key}");
                break;
        }
    }
}
