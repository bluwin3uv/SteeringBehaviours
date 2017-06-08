using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

[RequireComponent(typeof(Spawner))]
public class SpawnerXML : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
    public class SpawnData
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    [XmlRoot]
    public class XMLContainer
    {
        [XmlArray]
        public SpawnData[] data;
    }
    public string fileName;
    private Spawner spawner;
    private string fullPath;

    private XMLContainer xmlContainer;

    void SaveToPath(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(XMLContainer));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, xmlContainer);
        }
    }

    XMLContainer Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(XMLContainer));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as XMLContainer;
        }
    }

    void Start()
    {
        spawner = GetComponent<Spawner>();
        fullPath = Application.dataPath + "/" + fileName + ".xml";
        if(fullPath != null)
        {
            xmlContainer = Load(fullPath);
        }
    }

    public void Save()
    {
        objects = spawner.objects; 
        xmlContainer = new XMLContainer();
        xmlContainer.data = new SpawnData[objects.Count];
        for(int i = 0; i < objects.Count;i++)
        {
            SpawnData  data = new SpawnData();
            GameObject item = objects[i];
            data.position = item.transform.position;
            data.rotation = item.transform.rotation;
            xmlContainer.data[i] = data;
        }
        SaveToPath(fullPath);
    }

    void Apply()
    {
       SpawnData[] data = xmlContainer.data;
        for(int i = 0; i < data.Length;i++)
        {
            SpawnData d = data[i];
            spawner.Spawn(d.position, d.rotation);
        }
    }
}
