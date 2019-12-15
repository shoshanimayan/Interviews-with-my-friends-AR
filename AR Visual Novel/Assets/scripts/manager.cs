using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public Dictionary<int, Dialogue> graph;
    public static int cursor;//stores an int because all dialogues are uniquely ID by an int
    public string script; // script of dialouge to read;
    public Image mainImage;
    public Text textbox;
    public Image b1;
    public Image b2;
    public Text b1Text;
    public Text b2Text;

    public float TyperDelay = 0.01f;
    public AudioSource AS;
    private Coroutine activeTyper, activeSpeaker;
    public bool MakeSounds;
    int choice;
    bool click=false;

    //sounds
    public AudioClip person1;
    AudioClip speakerAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        speakerAudio = person1;
        click = false;
        AS = GetComponent<AudioSource>();
        graph = new Dictionary<int, Dialogue>();
        List<Dictionary<string, object>> data = CSVReader.Read(script);
        Debug.Log("lines read in: " + data.Count);
        choice = -1;
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
            yield return new WaitForSeconds(TyperDelay);
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
        cursor = graph[cursor].defaultOption - 2;
        click = true;
    }

    //option one continue, used after selecting option one
    public void continueTextOption1()
    {

        cursor = graph[cursor].options[0] - 2;
        click = true;
    }

    //option two continue, used after selecting second object
    public void continueTextOption2()
    {

        cursor = graph[cursor].options[1] - 2;
        click = true;
    }

    //switch handler for differnt events, animation changes and music 
    public void eventHandler() {
      

    }



    // Update is called once per frame
    void Update()
    {
        //effect how you continue text, either by clicking textbox or selecting option
        if (graph[cursor].options.Count <= 1)
        {
            mainImage.GetComponent<Collider2D>().enabled = true;
            b1.enabled = false;
            b2.enabled = false;
        }
        else
        {
            mainImage.GetComponent<Collider2D>().enabled = false;
            b1Text.text = graph[graph[cursor].options[0] - 2].text;
            b2Text.text = graph[graph[cursor].options[1] - 2].text;
            b1.enabled = true;
            b2.enabled = true;

        }
        //if clicked update text or end story
        if (click) {

            if (graph[cursor].speaker == "End") // end cutscene
            {
                Application.Quit();
            }
            else {
                Debug.Log(graph[cursor].speaker + ": " + graph[cursor].text);
                dialogueUpdate(graph[cursor].text);
                eventHandler();
            }
           
        }
        
    }
}
