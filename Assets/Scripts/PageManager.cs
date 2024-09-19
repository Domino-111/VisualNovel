using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    //Track the current page we're on
    public int currentPage;

    //Remember previous page we were on
    public int previousPage;

    //Text which displays the current page
    public Text pageDisplay;

    //The transform to parent our buttons onto
    public Transform buttonPanel;

    //Prefab for our button
    public ButtonEvent buttonPrefab;

    //Create an instance of our inventory to track our items
    public Inventory inventory = new Inventory();

    void Start()
    {
        //Load the first page
        LoadPage(0);
    }

    public void LoadPage(int pageNumber)
    {
        //Remember what page we were just on
        previousPage = currentPage;
        
        //Update to the page we're on now
        currentPage = pageNumber;

        //Load the page we're on now from resources
        TextAsset newPage = Resources.Load<TextAsset>("page" + pageNumber.ToString());

        //Pass the text from our file into the function
        UnpackPage(newPage.text);
    }

    private void UnpackPage(string pageContents)
    {
        //string.Split() will seperate a string into an array, using the provided character
        string[] unpackedPage = pageContents.Split('~');
        //Here, unpackedPage[0] is the display text and unpackedPage[1] is our button info

        pageDisplay.text = unpackedPage[0];

        //Split the button information into seperate buttons
        string[] buttons = unpackedPage[1].Split('_');

        //Pass those buttons into our function
        SetButtons(buttons);
    }

    private string RewritePage(string pageContents)
    {
        return "";
    }

    private void SetButtons(string[] buttonInfo)
    {
        //For as long as 'i' (starts at 0) is less than the number of child objects in buttonPanel
        for (int i = 0; i < buttonPanel.childCount; i ++)
        {
            //Destroy the child gameobject
            Destroy(buttonPanel.GetChild(i).gameObject);

            //Automatically add 1 to 'i' and check the condition again
        }

        //Do something with every string contained in buttonInfo
        foreach (string currentButtonInfo in buttonInfo)
        {
            //Here we break our currentButtonInfo into 3 strings
            string[] buttonDetail = currentButtonInfo.Split('|');

            //Instantiate means create an instance (aka a copy)
            ButtonEvent currentButton = Instantiate(buttonPrefab, buttonPanel);
            currentButton.Initialise(buttonDetail[0], buttonDetail[1], buttonDetail[2], this);
        }
    }
}
