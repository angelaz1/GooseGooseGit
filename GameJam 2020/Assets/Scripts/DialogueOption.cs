using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace DialogueTree
{
    public class DialogueOption
    {
        public string Text;
        public int DestinationNodeID;

        public DialogueOption()
        {

        }

        public DialogueOption(string text, int dest)
        {
            this.Text = text;
            this.DestinationNodeID = dest;
        }
    }
}
