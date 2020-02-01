using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace DialogueTree
{
    public class DialogueNode
    {
        public int NodeID = -1;
        public string Text;
        public List<DialogueOption> Options;

        public DialogueNode()
        {
            Options = new List<DialogueOption>();
        }

        public DialogueNode(string text)
        {
            Text = text;
            Options = new List<DialogueOption>();
        }
    }
}
