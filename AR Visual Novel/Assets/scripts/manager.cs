using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public Dictionary<int, Dialogue> graph;
    public static int cursor;//stores an int because all dialogues are uniquely ID by an int
    int tempCur;
    public TextAsset script; // script of dialouge to read;
    public Image mainImage;
    public Text textbox;
    public Image b1;
    public Image b2;
    public Text b1Text;
    public Text b2Text;
    public GameObject fire;
    public float TyperDelay = 0.0001f;
    private Coroutine activeTyper, activeSpeaker;
    bool click=false;
    //sounds
    public AudioSource AS;
    public AudioSource AS2;
    public AudioClip speakerAudio;
    public AudioClip fireplace;
    public AudioClip spooky;
    public bool MakeSounds;

    //animation
    public GameObject mouth;
    float destination = .6f;


    // Start is called before the first frame update
    

    void Awake()
    {
        textbox.text = "press to start";
        cursor = 0;
        
        click = false;
        AS = GetComponent<AudioSource>();
        graph = new Dictionary<int, Dialogue>();
        List<Dictionary<string, object>> data = CSVReader.Read(script);
        Debug.Log("lines read in: " + data.Count);
        for (int i = 0; i < data.Count; ++i)
        {
            graph[i] = new Dialogue(i, data[i]);
        }

    }

    IEnumerator Typer(string input)
    {
        AS.volume = 1;
        for (int i = 0; i <= input.Length; i++)
        {

            if (input.Substring(input.Length - 1, 1) == " ")
                MakeSounds = false;
            else
                MakeSounds = true;

            textbox.text = input.Substring(0, i);
            yield return new WaitForSeconds(TyperDelay*Time.deltaTime);
        }
        MakeSounds = false;
        AS.volume = 0;

    }

    //plays sound effect for typing text
    IEnumerator musicPlayer()
    {   
        while (MakeSounds == true)
        {
            AS.PlayOneShot(speakerAudio);
            yield return new WaitForSeconds(speakerAudio.length);
        }
    }

    public void dialogueUpdate(string input)
    {
        Debug.Log(graph[cursor].speaker + ": " + graph[cursor].text);

        if (activeTyper != null)
        {
            StopCoroutine(activeTyper);
        }
        activeTyper = StartCoroutine(Typer(input));
        if (activeSpeaker != null)
        {
            StopCoroutine(activeSpeaker);
        }
        activeSpeaker = StartCoroutine(musicPlayer());
    }

    //continue text
    public void continueText() {
        if (graph[cursor].options.Count <= 1)
        {
            if (cursor != 0)
            {
                tempCur = graph[cursor].defaultOption - 2;
            }
            click = true;
        }
    }

    //option one continue, used after selecting option one
    public void continueTextOption1()
    {

        tempCur = graph[graph[cursor].options[0] - 2].defaultOption - 2;
        click = true;
    }

    //option two continue, used after selecting second object
    public void continueTextOption2()
    {

        tempCur = graph[graph[cursor].options[1] - 2].defaultOption - 2;
        click = true;
    }

    //switch handler for differnt events, animation changes and music 
    public void eventHandler() {
        switch (graph[cursor].textEvent) {
            case "on":
                fire.SetActive(true);
                AS2.clip = fireplace;
                AS2.Play(0);
                break;

            case "off":
                fire.SetActive(false);
                AS2.Pause();
                break;

            case "scaryMusic":
                AS2.clip = spooky;
                AS2.Play(0);
                break;


            case "none":
                Debug.Log("nothing");
                break;

            default:
                Debug.Log("nothing");
                break;

        }

    }



    // Update is called once per frame
    void Update()
    {
        if (MakeSounds)
        {
            if (mouth.transform.localScale.x < destination)
            {
                mouth.transform.localScale = new Vector3(mouth.transform.localScale.x + .1f, mouth.transform.localScale.y, mouth.transform.localScale.z);
            }
            if (mouth.transform.localScale.x > destination)
            {
                mouth.transform.localScale = new Vector3(mouth.transform.localScale.x - .1f, mouth.transform.localScale.y, mouth.transform.localScale.z);
            }
            if (mouth.transform.localScale.x == destination)
            {
                if (destination == .4f)
                    destination = .6f;
                else
                    destination = .4f;
            }

        }
        else {
            if(mouth.transform.localScale.x>.4f)
                mouth.transform.localScale = new Vector3(mouth.transform.localScale.x - .1f, mouth.transform.localScale.y, mouth.transform.localScale.z);
        }
        //effect how you continue text, either by clicking textbox or selecting option
        if (graph[cursor].options.Count <= 1)
        {
            b1.enabled = false;
            b2.enabled = false;
            b1Text.enabled = false;
            b2Text.enabled = false;
        }
        else
        {
            b1Text.enabled = true;
            b2Text.enabled = true;
            b1Text.text = graph[graph[cursor].options[0] - 2].text;
            b2Text.text = graph[graph[cursor].options[1] - 2].text;
            b1.enabled = true;
            b2.enabled = true;

        }
        //if clicked update text or end story
        if (click) {
            click = false;
            if (graph[cursor].speaker == "End") // end cutscene
            {
#if UNITY_EDITOR

                UnityEditor.EditorApplication.isPlaying = false;

#else
                Application.Quit();
#endif
            }
            else {
                cursor = tempCur;
                tempCur++;
                dialogueUpdate(graph[cursor].text);
                eventHandler();
            }
           
        }
        
    }
}
