using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestManager : MonoBehaviour
{
    public BookBehavior bh;
    public DialogueManager dlm;
    public DialogueTrigger dlt;
    public Dialogue dl;
    
    public GameObject npc1;
    public GameObject npc2;
    public GameObject npc3;
    public GameObject npc4;
    public GameObject npc5;

    public GameObject Goblin;
    public GameObject Barbarian; 

    public int goblinskilled = 0;
    public int barbarianskilled = 0;

    public BookBehavior BookHandler;

    //player object designed with locational purposes 
    // player object dealing with goblins 

    // Quest number
    int questnumber1 = 1;
    int questnumber2 = 2;
    int questnumber3 = 3;
    int questnumber4 = 4;
    int questnumber5 = 5;

    // Quest titles
    string Quest1Title = "Quest 1";
    string Quest2Title = "Quest 2";
    string Quest3Title = "Quest 3";
    string Quest4Title = "Quest 4";
    string Quest5Title = "Quest 5";


    // Description of quests. Will use dummy testables for right now 
    string Quest1Des = "Help Andre get to the road.";
    string Quest2Des = "Protect Andre from gblins. Kill 5 Goblins.";
    string Quest3Des = "Help Dustin to the road.";
    string Quest4Des = "Protect Dustin from Barabarians. Kill 5 Barabarians ";
    string Quest5Des = "Arrive at the Port ";

    QuestFinder[] ListOfQuests = new QuestFinder[5];

    // Start is called before the first frame update
    void Start()
    {
        ListOfQuests[0] = new QuestFinder(Quest1Title, Quest1Des, questnumber1, npc1);
        ListOfQuests[1] = new QuestFinder(Quest2Title, Quest2Des, questnumber2, npc2);
        ListOfQuests[2] = new QuestFinder(Quest3Title, Quest3Des, questnumber3, npc3);
        ListOfQuests[3] = new QuestFinder(Quest4Title, Quest4Des, questnumber4, npc4);
        ListOfQuests[4] = new QuestFinder(Quest5Title, Quest5Des, questnumber5, npc5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public class QuestFinder
    {
        public QuestFinder(string Title, string description, int questNum, GameObject NPC)
        {
            QuestTitle = Title;
            QuestDes = description;
            active = false;
            questNumber = questNum;
            this.NPC = NPC;
            slotNumber = 1;
            completed = false;
        }

        string QuestTitle;
        string QuestDes;
        bool active;
        int questNumber;
        GameObject NPC;
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

        public GameObject getNPC()
        {
            return NPC;
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

    // quest 1-----------------------------------------------------------------

    // start first quest when called
    public void startQuest1()
    {
        if (BookHandler.questSlotAvailable() == true)
        {
            // start quest
            ListOfQuests[0].setSlot(BookHandler.firstFreeQuestSlot());
            BookHandler.setQuest(Quest1Title, Quest1Des);
            ListOfQuests[0].setActive(true);
        }
        else
        {
            // dont allow quest to start
        }
    }

    // this function checks if quest 1 can be completed or not
    // if it can be completed, it is marked as complete
    public void Quest1Check()
    {
        if (ListOfQuests[0].getActive() == true)
        {
            ListOfQuests[0].setCompleted();
            ListOfQuests[0].setActive(false);
            // call dialog manager for quest 1 complete dialog
            BookHandler.CompleteQuest(ListOfQuests[0].getSlot());
            // unlock spell as reward
        }
    }



    // quest 2-----------------------------------------------------------------

    public void startQuest2()
    {
        if (BookHandler.questSlotAvailable() == true)
        {
            // start quest
            ListOfQuests[1].setSlot(BookHandler.firstFreeQuestSlot());
            BookHandler.setQuest(Quest2Title, Quest2Des);
            ListOfQuests[1].setActive(true);
        }
        else
        {
            // dont allow quest to start
        }
    }

    public void Quest2Check()
    {
        if (ListOfQuests[1].getActive() == true)
        {
            goblinskilled++;
            if (goblinskilled == 5)
            {
                ListOfQuests[1].setCompleted();
                ListOfQuests[1].setActive(false);
                // Will have a UI to say quest completed.
                BookHandler.CompleteQuest(ListOfQuests[1].getSlot());
                // unlock spell as reward

            }
            else
            {
                // Have to continue to kill gobins --> UI Will show a screen that will tell user to keep going 
                // Could have a kill count to document the amount of kills
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
            BookHandler.setQuest(Quest1Title, Quest3Des);
            ListOfQuests[2].setActive(true);
        }
        else
        {
            // dont allow quest to start
        }


    }
    public void Quest3Check()
    {
        if (ListOfQuests[2].getActive() == true)
        {
            ListOfQuests[2].setCompleted();
            ListOfQuests[2].setActive(false);
            // call dialog manager for quest 1 complete dialog
            BookHandler.CompleteQuest(ListOfQuests[2].getSlot());
            // unlock spell as reward
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
    public void Quest4Check()
    {
        if (ListOfQuests[3].getActive() == true)
        {
            barbarianskilled++;
            if (barbarianskilled == 5)
            {
                ListOfQuests[3].setCompleted();
                ListOfQuests[3].setActive(false);
                // call dialog manager for quest 1 complete dialog
                BookHandler.CompleteQuest(ListOfQuests[3].getSlot());
                // unlock spell as reward
            }
            else
            {
                //Keep killing Barabarians 
            }
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
    public void Quest5Check()
    {
        if (ListOfQuests[4].getActive() == true)
        {
            ListOfQuests[4].setCompleted();
            ListOfQuests[4].setActive(false);
            // call dialog manager for quest 1 complete dialog
            BookHandler.CompleteQuest(ListOfQuests[4].getSlot());
            // unlock spell as reward
        }
    }

}
