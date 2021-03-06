using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestManager : MonoBehaviour
{
    //public BookBehavior bh;
   //public DialogueManager dlm;
   //public DialogueTrigger dlt;
    //public Dialogue dl;

    [HideInInspector]
    public BookBehavior BookHandler;

    [HideInInspector]
    public EnemyKillQuest killQuest1;
    [HideInInspector]
    public RetrievalQuest retrievalQuest1;

    //player object designed with locational purposes 
    // player object dealing with goblins 

    // Quest number
    int questnumber1 = 1;
    int questnumber2 = 2;
    int questnumber3 = 3;
    int questnumber4 = 4;
    int questnumber5 = 5;

    // Quest titles
    [HideInInspector]
    public string Quest1Title = "Follow Road";
    [HideInInspector]
    public string Quest2Title = "Goblin Siege";
    [HideInInspector]
    public string Quest3Title = "Help Dustin";
    [HideInInspector]
    public string Quest4Title = "Protect";
    [HideInInspector]
    public string Quest5Title = "Welcome Home";


    // Description of quests. Will use dummy testables for right now 
    //any quest that is a kill enemies quest will have to be formated with (0/amountOfEnemies) at the end of the string
    [HideInInspector]
    public string Quest1Des = "Help Andre get to the road.";
    [HideInInspector]
    public string Quest2Des = "Protect Andre from goblins. Kill 3 Goblins. (0/3)";
    [HideInInspector]
    public string Quest3Des = "Get back the bow that was stolen from dustin by the archer goblins";
    [HideInInspector]
    public string Quest4Des = "Protect Dustin from Barabarians. Kill 5 Barabarians ";
    [HideInInspector]
    public string Quest5Des = "Arrive at the Port ";

    QuestFinder[] ListOfQuests = new QuestFinder[5];
    List<string> questList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        killQuest1 = GetComponent<EnemyKillQuest>();
        retrievalQuest1 = GetComponent<RetrievalQuest>();
        BookHandler = GameObject.FindGameObjectWithTag("BookHandler").GetComponent<BookBehavior>();
        ListOfQuests[0] = new QuestFinder(Quest1Title, Quest1Des, questnumber1);
        ListOfQuests[1] = new QuestFinder(Quest2Title, Quest2Des, questnumber2);
        ListOfQuests[2] = new QuestFinder(Quest3Title, Quest3Des, questnumber3);
        ListOfQuests[3] = new QuestFinder(Quest4Title, Quest4Des, questnumber4);
        ListOfQuests[4] = new QuestFinder(Quest5Title, Quest5Des, questnumber5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string getDes(int questNum)
    {
        switch (questNum)
        {
            case 1:
                return Quest1Des;
            case 2:
                return Quest2Des;
            case 3:
                return Quest3Des;
            case 4:
                return Quest4Des;
            case 5:
                return Quest5Des;
        }
        return "";
    }


    public class QuestFinder
    {
        public QuestFinder(string Title, string description, int questNum)
        {
            QuestTitle = Title;
            QuestDes = description;
            active = false;
            questNumber = questNum;
            slotNumber = 1;
            completed = false;
        }

        string QuestTitle;
        string QuestDes;
        bool active;
        int questNumber;
        int slotNumber; // -1 if no slot
        bool completed;

        // getters
        public string getTitle()
        {
            return QuestTitle;
        }

        public string getDes()
        {
            return QuestDes;
        }


        public bool getActive()
        {
            return active;
        }

        public int getNumber()
        {
            return questNumber;
        }

        public int getSlot()
        {
            return slotNumber;
        }

        public bool getCompleted()
        {
            return completed;
        }

        // setter
        public void setActive(bool act)
        {
            active = act;
        }

        public void setSlot(int slot)
        {
            slotNumber = slot;
        }

        public void setCompleted()
        {
            completed = true;
        }

    }

    public void StartQuest(string num)
    {
        
        switch (num)
        {
            case "1":
                startQuest1();
                break;
            case "2":
                startQuest2();
                killQuest1.enabled = true;
                break;
            case "3":
                startQuest3();
                retrievalQuest1.enabled = true;
                break;
            case "4":
                startQuest4();
                break;
            case "5":
                startQuest5();
                break;

        }
    }

    // quest 1-----------------------------------------------------------------

    // start first quest when called
    public void startQuest1()
    {
        if (!questList.Contains(Quest1Title))
        {
            if (BookHandler.questSlotAvailable() == true)
            {
                // start quest
                ListOfQuests[0].setSlot(BookHandler.firstFreeQuestSlot());
                BookHandler.setQuest(Quest1Title, Quest1Des);
                ListOfQuests[0].setActive(true);
                questList.Add(Quest1Title);
            }
            else
            {
                // dont allow quest to start
            }
        }
    }

    // this function checks if quest 1 can be completed or not
    // if it can be completed, it is marked as complete
    public void QuestCheck(int numQuest, int unlockNum)
    {
        numQuest = numQuest - 1;
        if (ListOfQuests[numQuest].getActive() == true)
        {
            ListOfQuests[numQuest].setCompleted();
            ListOfQuests[numQuest].setActive(false);
            // call dialog manager for quest 1 complete dialog
            BookHandler.CompleteQuest(ListOfQuests[numQuest].getSlot());
            // unlock spell as reward
            BookHandler.unLockAll();





        }

    }


    // quest 2-----------------------------------------------------------------

    public void startQuest2()
    {
        if (!questList.Contains(Quest2Title))
        {
            if (BookHandler.questSlotAvailable() == true)
            {
                // start quest
                ListOfQuests[1].setSlot(BookHandler.firstFreeQuestSlot());
                BookHandler.setQuest(Quest2Title, Quest2Des);
                ListOfQuests[1].setActive(true);
                questList.Add(Quest2Title);
            }
            else
            {
                // dont allow quest to start
            }
        }
    }


    // quest 3-----------------------------------------------------------------
    public void startQuest3()
    {
        if (BookHandler.questSlotAvailable() == true)
        {
            // start quest
            ListOfQuests[2].setSlot(BookHandler.firstFreeQuestSlot());
            BookHandler.setQuest(Quest3Title, Quest3Des);
            ListOfQuests[2].setActive(true);
        }
        else
        {
            // dont allow quest to start
        }


    }

    // quest 4-----------------------------------------------------------------
    public void startQuest4()
    {

        if (BookHandler.questSlotAvailable() == true)
        {
            // start quest
            ListOfQuests[3].setSlot(BookHandler.firstFreeQuestSlot());
            BookHandler.setQuest(Quest4Title, Quest3Des);
            ListOfQuests[3].setActive(true);
        }
        else
        {
            // dont allow quest to start
        }
    }

    // quest 5-----------------------------------------------------------------
    public void startQuest5()
    {
        if (BookHandler.questSlotAvailable() == true)
        {
            // start quest
            ListOfQuests[4].setSlot(BookHandler.firstFreeQuestSlot());
            BookHandler.setQuest(Quest5Title, Quest3Des);
            ListOfQuests[4].setActive(true);
        }
        else
        {
            // dont allow quest to start
        }
    }

}
