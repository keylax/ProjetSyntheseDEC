using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using Assets.Scripts.Game_Objects;

public class LoadMap : MonoBehaviour
{
    private const string REF_PATH = "Assets/Maps/";
    private string path;
    private Map map;
    public string xmlFileName;
    public PrefabFactory factory;

    // Use this for initialization
    void Start()
    {
        path = REF_PATH + xmlFileName;
        XmlSerializer serializer = new XmlSerializer(typeof(Map));

        StreamReader reader = new StreamReader(path);
        map = (Map)serializer.Deserialize(reader);
        reader.Close();

        factory.CreateMap(map);
    }
}
