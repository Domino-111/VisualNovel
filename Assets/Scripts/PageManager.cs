using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

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

    public void LoadPage(string itemName)
    {
        previousPage = currentPage;
        currentPage = -1;

        TextAsset newPage = Resources.Load<TextAsset>("page" + itemName);

        UnpackPage(newPage.text);
    }

    private void UnpackPage(string pageContents)
    {
        //string.Split() will seperate a string into an array, using the provided character
        string[] unpackedPage = pageContents.Split('~');
        //Here, unpackedPage[0] is the display text and unpackedPage[1] is our button info

        //Check if our page has any conditional text
        if (unpackedPage[0].Contains("??"))
        {
            //Rewrite that text and replace it
            unpackedPage[0] = RewritePage(unpackedPage[0]);
        }

        pageDisplay.text = unpackedPage[0];

        //Split the button information into seperate buttons
        string[] buttons = unpackedPage[1].Split('_');

        //Pass those buttons into our function
        SetButtons(buttons);
    }

    private string RewritePage(string pageContents)
    {
        //Split using the new-line character
        string[] pageLines = pageContents.Split('\n');

        string rewrittenString = "";

        for (int i = 0; i < pageLines.Length; i ++)
        {
            //If the current line is empty, skip it
            if (pageLines[i] == "")
            {
                continue;
            }

            if (pageLines[i].Contains("??"))
            {
                //Break our line into the condition and the page text
                string[] conditionAndLine = pageLines[i].Split("??");

                //If the condition does not pass don't write the line
                if (!CheckCondition(conditionAndLine[0]))
                {
                    continue;
                }

                //Replace this line with the text sans-condition
                pageLines[i] = conditionAndLine[1];
            }

            //Otherwise we add the line to our rewritten string
            rewrittenString += pageLines[i];

            //If this is not the last line of the page...
            if (i + 2 < pageLines.Length)
            {
                //We should add new line breaks
                rewrittenString += "\n\n";
            }
        }

        return rewrittenString;
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

            //If the first part of our button details includes the '??' we have a condition to check
            if (buttonDetail[0].Contains("??"))
            {
                //Separate our condition and our action
                string[] conditionAndButtonAction = buttonDetail[0].Split("??");

                //Check the condition using a function - if it fails the check we shouldn't spawn this button
                if (!CheckCondition(conditionAndButtonAction[0]))
                {
                    continue; //'continue' means stop this iteration and start the next iteration
                }

                //Replace "has:Item??action" with "action"
                buttonDetail[0] = conditionAndButtonAction[1];
            }

            //Instantiate means create an instance (aka a copy)
            ButtonEvent currentButton = Instantiate(buttonPrefab, buttonPanel);
            currentButton.Initialise(buttonDetail[0], buttonDetail[1], buttonDetail[2], this);
        }
    }

    public bool CheckCondition(string condition)
    {
        //We'll use "has" to check if the player has something
        //We'll use "hasNot" to check if the player is missing something

        string[] conditionSplit = condition.Split(':');

        Item item = Enum.Parse<Item>(conditionSplit[1]);

        //If our condition is we should have the item check the inventory for it
        if (conditionSplit[0] == "has")
        {
            return inventory.Has(item);
        }

        if (conditionSplit[0] == "hasNot")
        {
            return !inventory.Has(item);
        }

        Debug.LogWarning($"Tried to check {conditionSplit[0]} which is not a vaild condition");
        return true;
    }
}
