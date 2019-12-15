using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
/// dialouge class, stores short text , longText, speaker, line ID, default options in case of timers, options or a list of other lines to look at next given choice, portrait transitions in case want more than one  
/// basic setup, modify as necessary for your game and what information you want to be stored for every dialouge scene, could add things like time for timer etc
/// </summary>

public class Dialogue
{
    public string
        text,//connected text for each line of dialouge
        speaker,//who is speaking 
        textEvent; //code for event that will be triggered at this dialogue 
    public int ID,//id of text
        defaultOption;//the unique identifier for this diaglogue object
    public List<int> options;//IDs of other Dialogue entries that are options in response to this dialogue.


    public Dialogue(int id, Dictionary<string, object> input)
    {
        
        text = (string)input["text"];
        speaker = (string)input["speaker"];
        textEvent = (string)input["event"];
        ID = id + 2;//the spreadsheet is offset by 2 from the true value so we store it as its marked
        defaultOption = (int)input["default"];
        //building options list
        int n = 0;
        options = new List<int>();
        //options are formatted with "a#,#" with pound being the number to look for in the script for the option text, a being a delimeter, inside a string parenthesis 
        string s = (string)input["options"];
        foreach (string st in s.Replace("a", "").Split(','))
        {
            ++n;
            options.Add(int.Parse(st.Trim()));
        }

        
    
    }
}