using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookBehavior : MonoBehaviour
{
    public int activePage = 1;

    bool bookActive = false;

    public TextMeshProUGUI QuestText;
    public TextMeshProUGUI SpellsText;
    public TextMeshProUGUI InventoryText;

    public GameObject QuestsHandler;
    public GameObject SpellsHandler;
    public GameObject InventoryHandler;

    [Header("Quests")]
    public GameObject Quest1Handler;
    public GameObject Quest2Handler;
    public GameObject Quest3Handler;
    public GameObject Quest4Handler;
    public GameObject Quest5Handler;

    public TextMeshProUGUI Quest1TitleText;
    public TextMeshProUGUI Quest2TitleText;
    public TextMeshProUGUI Quest3TitleText;
    public TextMeshProUGUI Quest4TitleText;
    public TextMeshProUGUI Quest5TitleText;

    public TextMeshProUGUI Quest1DesText;
    public TextMeshProUGUI Quest2DesText;
    public TextMeshProUGUI Quest3DesText;
    public TextMeshProUGUI Quest4DesText;
    public TextMeshProUGUI Quest5DesText;

    string Quest1Title = "Quest 1";
    string Quest2Title = "Quest 2";
    string Quest3Title = "Quest 3";
    string Quest4Title = "Quest 4";
    string Quest5Title = "Quest 5";

    string Quest1Des = "";
    string Quest2Des = "";
    string Quest3Des = "";
    string Quest4Des = "";
    string Quest5Des = "";

    bool Quest1Free = true;
    bool Quest2Free = true;
    bool Quest3Free = true;
    bool Quest4Free = true;
    bool Quest5Free = true;

    public int AmountOfActiveQuests = 0;


    // Start is called before the first frame update
    void Start()
    {
        deactivatePages(1);
        openQuest();
        //StartCoroutine(testFunction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // just for testing the code
    IEnumerator testFunction()
    {
        openSpells();
        yield return new WaitForSeconds(2);

        openInventory();
        yield return new WaitForSeconds(2);

        openQuest();
        yield return new WaitForSeconds(2);

        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        setQuest("Hello World", "Is this thing on?");
        yield return new WaitForSeconds(2);

        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        setQuest(2, "Hello World 2", "Is this thing on too?");
        yield return new WaitForSeconds(2);

        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        setQuest("Hello World 3", "Is this thing on 3?");
        yield return new WaitForSeconds(2);

        CompleteQuest(1);
        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        yield return new WaitForSeconds(2);

        setQuest("Hello World 4", "Is this thing on 4?");

        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        setQuest("Hello World 5", "Is this thing on 5?");
        yield return new WaitForSeconds(2);

        CompleteQuest(3);
        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        yield return new WaitForSeconds(2);

        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        setQuest("Hello World 6", "Is this thing on 6?");
        yield return new WaitForSeconds(2);

        Debug.Log(firstFreeQuestSlot());
        Debug.Log(questSlotAvailable());

        setQuest("Hello World ", "Is this thing on 7?");
        yield return new WaitForSeconds(2);
    }

    // activates Quest Page
    public void openQuest()
    {
        deactivatePages(1);
        QuestText.fontSize = 50;
        QuestsHandler.SetActive(true);

    }

    // activates Spell Page
    public void openSpells()
    {
        deactivatePages(2);
        SpellsText.fontSize = 50;
        SpellsHandler.SetActive(true);

    }

    // activates Inventory Page
    public void openInventory()
    {
        deactivatePages(3);
        InventoryText.fontSize = 50;
        InventoryHandler.SetActive(true);

    }

    // returns the int corresponding to the first quest slot that is free. -1 is returned if all are full
    public int firstFreeQuestSlot()
    {
        if (Quest1Free)
        {
            return 1;
        }
        else if (Quest2Free)
        {
            return 2;
        }
        else if (Quest3Free)
        {
            return 3;
        }
        else if (Quest4Free)
        {
            return 4;
        }
        else if (Quest5Free)
        {
            return 5;
        }
        else return -1;

    }
    
    // deactivates all pages
    public void deactivatePages(int activePage)
    {
        QuestText.fontSize = 26;
        SpellsText.fontSize = 26;
        InventoryText.fontSize = 26;

        QuestsHandler.SetActive(false);
        SpellsHandler.SetActive(false);
        InventoryHandler.SetActive(false);

    }

    // deactivates all quests
    public void deactivateQuests()
    {
        Quest1Handler.SetActive(false);
        Quest2Handler.SetActive(false);
        Quest3Handler.SetActive(false);
        Quest4Handler.SetActive(false);
        Quest5Handler.SetActive(false);
    }

    // returns true if there are open quests slots and false if all slots are full
    public bool questSlotAvailable()
    {
        if (AmountOfActiveQuests == 5) return false;
        else return true;
    }

    // sets a new quest at the first open quest slot
    public void setQuest(string Title, string Description)
    {
        if (AmountOfActiveQuests <= 5)
        {
            switch (firstFreeQuestSlot())
            {
                case 1:
                    Quest1Title = Title;
                    Quest1Des = Description;
                    Quest1Handler.SetActive(true);
                    Quest1Free = false;

                    break;
                case 2:
                    Quest2Title = Title;
                    Quest2Des = Description;
                    Quest2Handler.SetActive(true);
                    Quest2Free = false;

                    break;
                case 3:
                    Quest3Title = Title;
                    Quest3Des = Description;
                    Quest3Handler.SetActive(true);
                    Quest3Free = false;

                    break;
                case 4:
                    Quest4Title = Title;
                    Quest4Des = Description;
                    Quest4Handler.SetActive(true);
                    Quest4Free = false;

                    break;
                case 5:
                    Quest5Title = Title;
                    Quest5Des = Description;
                    Quest5Handler.SetActive(true);
                    Quest5Free = false;

                    break;
            }
            AmountOfActiveQuests++;
            UdateQuestText();
        }

        else return;
    }

    // sets a new quest at the indicated quest slot 
    public void setQuest(int questNum, string Title, string Description)
    {
        if (AmountOfActiveQuests <= 5)
        {
            switch (questNum)
            {
                case 1:
                    if (Quest1Free) AmountOfActiveQuests++;
                    Quest1Title = Title;
                    Quest1Des = Description;
                    Quest1Handler.SetActive(true);
                    Quest1Free = false;

                    break;
                case 2:
                    if (Quest2Free) AmountOfActiveQuests++;
                    Quest2Title = Title;
                    Quest2Des = Description;
                    Quest2Handler.SetActive(true);
                    Quest2Free = false;

                    break;
                case 3:
                    if (Quest3Free) AmountOfActiveQuests++;
                    Quest3Title = Title;
                    Quest3Des = Description;
                    Quest3Handler.SetActive(true);
                    Quest3Free = false;

                    break;
                case 4:
                    if (Quest4Free) AmountOfActiveQuests++;
                    Quest4Title = Title;
                    Quest4Des = Description;
                    Quest4Handler.SetActive(true);
                    Quest4Free = false;

                    break;
                case 5:
                    if (Quest5Free) AmountOfActiveQuests++;
                    Quest5Title = Title;
                    Quest5Des = Description;
                    Quest5Handler.SetActive(true);
                    Quest5Free = false;

                    break;
            }
            UdateQuestText();
        }

        else return;
    }

    // completes a quest by resetting all of its associated variables and freeing the slot for other use
    public void CompleteQuest(int questNum)
    {
        switch(questNum)
        {
            case 1:
                Quest1Title = "Quest 1";
                Quest1Des = "Description";
                Quest1Handler.SetActive(false);
                Quest1Free = true;

                break;
            case 2:
                Quest2Title = "Quest 1";
                Quest2Des = "Description";
                Quest2Handler.SetActive(false);
                Quest2Free = true;

                break;
            case 3:
                Quest3Title = "Quest 1";
                Quest3Des = "Description";
                Quest3Handler.SetActive(false);
                Quest3Free = true;

                break;
            case 4:
                Quest4Title = "Quest 1";
                Quest4Des = "Description";
                Quest4Handler.SetActive(false);
                Quest4Free = true;

                break;
            case 5:
                Quest5Title = "Quest 1";
                Quest5Des = "Description";
                Quest5Handler.SetActive(false);
                Quest5Free = true;

                break;
        }
        AmountOfActiveQuests--;
        UdateQuestText();
    }

    public void UdateQuestText()
    {
        Quest1TitleText.text = Quest1Title;
        Quest2TitleText.text = Quest2Title;
        Quest3TitleText.text = Quest3Title;
        Quest4TitleText.text = Quest4Title;
        Quest5TitleText.text = Quest5Title;

        Quest1DesText.text = Quest1Des;
        Quest2DesText.text = Quest2Des;
        Quest3DesText.text = Quest3Des;
        Quest4DesText.text = Quest4Des;
        Quest5DesText.text = Quest5Des;
    }

}
