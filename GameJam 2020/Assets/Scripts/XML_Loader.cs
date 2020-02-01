using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

using System.IO;

using DialogueTree;

[XmlRoot("MonsterCollection")]
public class XML_Loader: MonoBehaviour
{
    public Dialogue dia; 

    public int num_nodes;

    public void Button_Load(string file_name)
    {
        //mon = MonsterContainer.Load(Path.Combine(Application.dataPath, "monsters.xml"));
        //monsters = mon.Monsters;
        XmlSerializer serz = new XmlSerializer(typeof(Dialogue));
        StreamReader reader = new StreamReader(Path.Combine(Application.dataPath, file_name));

        dia = (Dialogue)serz.Deserialize(reader);

        num_nodes = dia.Nodes.Count;
    }

    public void Button_Save()
    {
        //mon.Save(Path.Combine(Application.persistentDataPath, "monsters.xml"));
    }
}
