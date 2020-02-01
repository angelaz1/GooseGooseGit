using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

using System.IO;

using DialogueTree;

public class npc_test : MonoBehaviour
{
    public Dialogue dia;

    public GameObject dialogue_window;

    public GameObject option1;
    public GameObject option2;
    public GameObject option3;

    public Image portrait;

    public GameObject exit;
    public GameObject next;

    public GameObject dia_text;

    public string file_path;
    
    public int selected_option = -2;

    Coroutine dialogue_run;

    public Sprite player_icon;
    public Sprite patient_icon;

    public bool show_options = false;

    public int minigame_scene_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        XmlSerializer serz = new XmlSerializer(typeof(Dialogue));
        StreamReader reader = new StreamReader(Path.Combine(Application.dataPath + "/Dialogue", file_path));

        dia = (Dialogue)serz.Deserialize(reader);

        exit.GetComponent<Button>().onClick.AddListener(delegate { EndDialogue(); });
        next.GetComponent<Button>().onClick.AddListener(delegate { Show_Options(); });

        dialogue_window.SetActive(false);
        //RunDialogue();
    }

    public void RunDialogue()
    {
        dialogue_run = StartCoroutine(run(true));
    }

    public void SetSelectedOption(int x)
    {
        selected_option = x;
    }

    public void Show_Options()
    {
        show_options = true;
    }

    public void EndDialogue()
    {
        StopCoroutine(dialogue_run);
        selected_option = -2;
        dialogue_window.SetActive(false);
    }

    public IEnumerator run(bool is_minigame)
    {
        dialogue_window.SetActive(true);

        //start at the beggining
        int node_id = 0;

        //loop until reach the end of the tree
        while (node_id != -1)
        {
            //get node
            DialogueNode node = dia.Nodes[node_id];

            show_options = false;

            //hide all options
            option1.SetActive(false);
            option2.SetActive(false);
            option3.SetActive(false);

            portrait.sprite = patient_icon;

            dia_text.SetActive(true);

            next.SetActive(false);

            //typewriter effect
            string _text = "";
            for (int i = 0; i < node.Text.Length; i++)
            {
                dia_text.GetComponent<Text>().text = _text;

                _text += node.Text[i];

                yield return new WaitForSeconds(0.02f);
            }

            next.SetActive(true);

            while (!show_options)
            {
                yield return new WaitForSeconds(0.1f);
            }

            next.SetActive(false);

            portrait.sprite = player_icon;

            dia_text.SetActive(false);

            //show buttons
            for (int i = 0; i < node.Options.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        set_option_button(option1, node.Options[i]);
                        break;
                    case 1:
                        set_option_button(option2, node.Options[i]);
                        break;
                    case 2:
                        set_option_button(option3, node.Options[i]);
                        break;
                }
            }

            //wait for button click
            selected_option = -2;

            while (selected_option == -2)
            {
                yield return new WaitForSeconds(0.25f);
            }

            //next node
            node_id = selected_option;
        }

        dialogue_window.SetActive(false);

        if (is_minigame)
        {
            Debug.Log("Load Scene: " + minigame_scene_index);
        }
    }

    public void set_option_button(GameObject button, DialogueOption opt)
    {
        button.SetActive(true);
        button.GetComponentInChildren<Text>().text = opt.Text;
        button.GetComponent<Button>().onClick.AddListener(delegate { SetSelectedOption(opt.DestinationNodeID); });
    }
}
