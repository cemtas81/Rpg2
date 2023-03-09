using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Runtime.Versioning;

public class DialogueManager : MonoBehaviour
{
    const string INTRO_PATH = "Assets/Resources/intro.txt";
    const string MOUNT_PATH = "Assets/Resources/mountains_end.txt";
    const string CATA_START_PATH = "Assets/Resources/catacomb_start.txt";
    const string CATA_END_PATH = "Assets/Resources/catacomb_end.txt";
    const string BEACH_START_PATH = "Assets/Resources/beach_start.txt";
    const string BEACH_END_PATH = "Assets/Resources/beach_end.txt";
    const string TEST_PATH = "Assets/Resources/testscript.txt";
    const string TEST_PATH_ALT = "Assets/Resources/testscriptalt.txt";

    const int MAIN_MENU = 0;
    const int LEVEL_1 = 1;
    const int LEVEL_2 = 2;

    //StreamReader sr;
    public TextMeshProUGUI DialogueText;
    public Image speakerImage;
    public Button NextLineButton;
    public Image ProtImageBox;
    public TextMeshProUGUI Nameplate;

    public Sprite Athena;
    public Sprite Eleos;
    public Sprite Hermes;

    Canvas dialogueCanvas;
    int currentLine = 0;
    //public TextAsset json;
    DialogueLine[] lines;
    bool nextLevel = false;

    public static DialogueManager instance;

    private void Awake()
    {
        //if (instance != null && instance != this)
        //{
        //    Destroy(this.gameObject);
        //}
        //instance = this;
        //DontDestroyOnLoad(instance);

        dialogueCanvas = GetComponentInChildren<Canvas>();
        dialogueCanvas.enabled = false;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    [Serializable]
    public class DialogueLine
    {
        public string speaker;
        public string text;
    }

    [Serializable]
    public class RootObject
    {
        public DialogueLine[] lines;
    }

    public string getFileForScene(string scene, bool isEnd)
    {
        string path = "";
        switch (scene)
        {
            case "Level 1":
                if (isEnd)
                    path = MOUNT_PATH;
                else
                    path = INTRO_PATH;
                break;
            case "Level 2":
                if (isEnd)
                    path = CATA_END_PATH;
                else
                    path = CATA_START_PATH;
                break;
            case "Level 3":
                if (isEnd)
                    path = BEACH_END_PATH;
                else
                    path = BEACH_START_PATH;
                break;
            case "DialogueTest":
                if (isEnd)
                    path = TEST_PATH_ALT;
                else
                    path = TEST_PATH;
                break;
        }
        return path;
    }

    public DialogueLine[] loadLines(string path)
    {
        Debug.Log(path);
        string jsonString = File.ReadAllText(path);
        //List<DialogueLine> scriptLines = new List<DialogueLine>();
        RootObject root = JsonUtility.FromJson<RootObject>(jsonString);
        //scriptLines = JsonUtility.FromJson<DialogueLine>(jsonString)
        DialogueLine[] scriptLines = root.lines;
        return scriptLines;
    }

    public void startDialogue(bool EndOfScene)
    {
        //nextLevel = scene;
        //bool isEnd = (nextLevel != -1);
        nextLevel = EndOfScene;
        string path = getFileForScene(SceneManager.GetActiveScene().name, EndOfScene);

        if (!File.Exists(path))
        {
            Debug.Log("File doesn't exist, cannot read");
            return;
        }

        lines = loadLines(path);

        if (dialogueCanvas.enabled)
        {
            dialogueCanvas.enabled = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            dialogueCanvas.enabled = true;
            Time.timeScale = 0.0f;
        }

        //sr = new StreamReader(path);
        nextLine();
    }

    public void nextLine()
    {
        ProtImageBox.enabled = false;
        if (currentLine < lines.Length && lines[currentLine] != null)
        {
            DialogueText.text = lines[currentLine].text;
            Nameplate.text = lines[currentLine].speaker;
            // placeholder names throughout
            switch (lines[currentLine].speaker)
            {
                case "Alkadios":
                    speakerImage.sprite = null;
                    ProtImageBox.enabled = true;
                    break;
                case "Athena":
                    speakerImage.sprite = Athena;
                    break;
                case "Hermes":
                    speakerImage.sprite = Hermes;
                    break;
                case "Eleos":
                    speakerImage.sprite = Eleos;
                    break;
            }
            currentLine++;
        }
        else {
            currentLine = 0;
            endDialogue();
        }
    }

    public void endDialogue()
    {
        dialogueCanvas.enabled = false;
        Time.timeScale = 1.0f;
        if (nextLevel)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //NextLineButton = GetComponent<Button>();
        //DialogueText = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //testing dialogue with space bar trigger --remove for release
        //if (Input.GetKeyDown("space"))
        //        startDialogue(LEVEL_1);
    }
}