using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class textClick : MonoBehaviour
{

    public enum TestEnum { main, option1, option2, start };

    //This is what you need to show in the inspector.
    GameObject manager;
    public TestEnum choices;
    void Awake() {
        manager= GameObject.Find("/game manager");
    }


    void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("press down");
        switch (choices)
        {
            case TestEnum.main:
                Debug.Log("MAIN");
                manager.GetComponent<manager>().continueText();
                break;
            case TestEnum.option1:
                Debug.Log("OPTION1");
                manager.GetComponent<manager>().continueTextOption1();
                break;
            case TestEnum.option2:
                Debug.Log("OPTION2");
                manager.GetComponent<manager>().continueTextOption2();
                break;
            case TestEnum.start:
                Debug.Log("START");
                SceneManager.LoadScene(1);
                break;
            default:
                Debug.Log("NOTHING");
                break;
        }

    }

}
