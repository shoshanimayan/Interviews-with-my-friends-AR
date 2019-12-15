using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textClick : MonoBehaviour
{

    public enum TestEnum { main, option1, option2 };

    //This is what you need to show in the inspector.
    GameObject manager;
    public TestEnum choices;
    void Awake() { manager= GameObject.Find("/game manager");
    }
    void OnMouseDown() {
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
            default:
                Debug.Log("NOTHING");
                break;
        }
    }
}
