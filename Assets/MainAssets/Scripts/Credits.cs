using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject Austin;
    public GameObject Alex;
    public GameObject Stephen;
    public GameObject Colby;

    public int dance;
    public int moveSpeed = 1;

    public Vector3 position1 = new Vector3(-150, 125, 0);
    public Vector3 position2 = new Vector3(150, 125, 0);
    public Vector3 position3 = new Vector3(-150, -50, 0);
    public Vector3 position4 = new Vector3(150, -50, 0);

    bool AustinInPosition = false;
    bool AlexInPosition = false;
    bool StephenInPosition = false;
    bool ColbyInPosition = false;

    public double maxX;
    public double minX;
    public double maxY;
    public double minY;

    Vector3 AustinTar;
    Vector3 AlexTar;
    Vector3 StephenTar;
    Vector3 ColbyTar;

    bool reset3 = false;
    bool reset2 = false;

    // Start is called before the first frame update
    void Start()
    {
        Austin.transform.position = transform.position + new Vector3(-500, 125, 0)*2.4f;
        Alex.transform.position = transform.position + new Vector3(500, 125, 0)*2.4f;
        Stephen.transform.position = transform.position + new Vector3(-500, -50, 0)*2.4f;
        Colby.transform.position = transform.position + new Vector3(500, -50, 0)*2.4f;

        dance = 1;

        //maxX = maxX * 2.4 + transform.position.x;
        //minX = minX * 2.4 + transform.position.x;
        //maxY = maxY * 2.4 + transform.position.y;
        //minY = minY * 2.4 + transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dance == 1)
        {
            if (AustinInPosition && AlexInPosition && StephenInPosition && ColbyInPosition)
            {
                StartCoroutine(ChooseDance(0f));
            }
            AustinInPosition = moveToPosition(Austin, position1);
            AlexInPosition = moveToPosition(Alex, position2);
            StephenInPosition = moveToPosition(Stephen, position3);
            ColbyInPosition = moveToPosition(Colby, position4);
        }

        else if (dance == 2)
        {
            if (!reset2)
            {
                StartCoroutine(ChooseDance(5f));
                reset2 = true;
            }
        }

        else if (dance == 3)
        {
            if (!reset3)
            {
                StartCoroutine(ChooseDance(7f));
                reset3 = true;
            }

            if (!AustinInPosition)
            {
                AustinInPosition = moveToPosition(Austin, AustinTar);
            }
            else
            {
                AustinTar = randomPosition();
                AustinInPosition = false;
            }

            if (!AlexInPosition)
            {
                AlexInPosition = moveToPosition(Alex, AlexTar);
            }
            else
            {
                AlexTar = randomPosition();
                AlexInPosition = false;

            }

            if (!StephenInPosition)
            {
                StephenInPosition = moveToPosition(Stephen, StephenTar);
            }
            else
            {
                StephenTar = randomPosition();
                StephenInPosition = false;

            }

            if (!ColbyInPosition)
            {
                ColbyInPosition = moveToPosition(Colby, ColbyTar);
            }
            else
            {
                ColbyTar = randomPosition();
                ColbyInPosition = false;

            }
        }
    }

    bool moveToPosition(GameObject name, Vector3 target)
    {
        if(Vector3.Distance(name.transform.position, transform.position + target * 2.4f) > .01)
        {
            name.transform.position = Vector3.MoveTowards(name.transform.position,transform.position+ target*2.4f, moveSpeed * Time.deltaTime);
            return false;
            //Vector3.Distance(name.transform.position,transform.position + target * 2.4f) > .01
        }
        return true;
    }

    Vector3 randomPosition()
    {
        float x = Random.Range((float)minX, (float)maxX);
        float y = Random.Range((float)minY, (float)maxY);
        return new Vector3(x, y, 0);
    }

    IEnumerator Wait(float wait)
    {
        yield return new WaitForSeconds(wait);
        dance = 3;
    }

    IEnumerator ChooseDance(float wait)
    {
        yield return new WaitForSeconds(wait);
        int choice = dance;
        while(choice == dance)
        {
            choice = Random.Range(1, 4);
        }
        dance = choice;
        reset2 = false;
        reset3 = false;
    }
}
