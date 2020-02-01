using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace DialogueTree
{
    public class Dialogue
    {
        public List<DialogueNode> Nodes;

        public void AddNode(DialogueNode node)
        {
            if (node == null) return;

            //Add node to list of dialogue nodes
            Nodes.Add(node);
            //Give it an ID
            node.NodeID = Nodes.IndexOf(node);
        }

        public void AddOption(string text, DialogueNode node, DialogueNode dest)
        {
            if (!Nodes.Contains(dest))
            {
                AddNode(dest);
            }

            if (!Nodes.Contains(node))
            {
                AddNode(node);
            }

            DialogueOption opt;

            if(dest == null)
            {
                opt = new DialogueOption(text, -1);
            }
            else
            {
                opt = new DialogueOption(text, dest.NodeID);
            }

            node.Options.Add(opt);
        }

        public Dialogue()
        {
            Nodes = new List<DialogueNode>();
        }
    }
}
