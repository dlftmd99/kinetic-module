using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class XmlManager : MonoBehaviour
{
    //Delegate
    public delegate void LoadXmlComplete(XmlDocument xmlDocument);
    public event LoadXmlComplete loadXmlComplete;
    public delegate void LoadXmlFail(string err);
    public event LoadXmlFail loadXmlFail;
    public delegate void WriteXmlComplete();
    public event WriteXmlComplete writeXmlComplete;
    public delegate void WriteXmlFail(string err);
    public event WriteXmlFail writeXmlFail;

    public void loadXML(string url)
    {
        if (File.Exists(url))
        {
            StreamReader sr = new StreamReader(url);
            string data = sr.ReadToEnd();
            if (data.Length > 0)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(new StringReader(data));
                if (loadXmlComplete != null)
                    loadXmlComplete(xmlDocument);
            }
            else
            {
                if (loadXmlFail != null)
                    loadXmlFail("Data length '0'");
            }
            sr.Close();
        }
        else
        {
            if (loadXmlFail != null)
                loadXmlFail("Not Exists File");
        }
    }

    public void writeXML(XmlDocument xmlDoc, string url)
    {
        File.WriteAllText(url, xmlDoc.InnerXml);
        if (writeXmlComplete != null)
            writeXmlComplete();
    }




    
      




    /*

    public void loadXML()
    {
        XMLDataStream xmlStreamer = new XMLDataStream();
        xmlStreamer.filePath = xmlPath;
        xmlStreamer.onXMLLoaded += onXMLLoaded;
        xmlStreamer.loadXML();
    }

    private void onXMLLoaded(XmlDocument xmlDoc)
    {
        parseXML(xmlDoc);
        if (onXmlLoaded != null)
            onXmlLoaded();
    }

    public void writeXml()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "euc-kr", null));   //[NOTE] euc-kr,, is it best for this?
        XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "root", string.Empty);
        xmlDoc.AppendChild(root);
        for (int a = 0; a < sceneList.Count; a++)
        {
            XmlNode character = xmlDoc.CreateNode(XmlNodeType.Element, "scene", string.Empty);
            root.AppendChild(character);
            XmlNode uid = xmlDoc.CreateNode(XmlNodeType.Element, "uid", string.Empty);
            XmlNode name = xmlDoc.CreateNode(XmlNodeType.Element, "name", string.Empty);
            XmlNode type = xmlDoc.CreateNode(XmlNodeType.Element, "type", string.Empty);
            XmlNode path = xmlDoc.CreateNode(XmlNodeType.Element, "path", string.Empty);
            XmlNode frameCount = xmlDoc.CreateNode(XmlNodeType.Element, "frameCount", string.Empty);
            XmlNode fps = xmlDoc.CreateNode(XmlNodeType.Element, "fps", string.Empty);
            XmlNode led = xmlDoc.CreateNode(XmlNodeType.Element, "led", string.Empty);
            XmlNode motion = xmlDoc.CreateNode(XmlNodeType.Element, "motion", string.Empty);
            uid.InnerText = sceneList[a].uid.ToString();
            name.InnerText = sceneList[a].name.ToString();
            type.InnerText = sceneList[a].type.ToString();
            path.InnerText = sceneList[a].path.ToString();
            frameCount.InnerText = sceneList[a].frameCount.ToString();
            fps.InnerText = sceneList[a].fps.ToString();
            led.InnerText = sceneList[a].led.ToString();
            character.AppendChild(uid);
            character.AppendChild(name);
            character.AppendChild(type);
            character.AppendChild(path);
            character.AppendChild(frameCount);
            character.AppendChild(fps);
            character.AppendChild(led);
            character.AppendChild(motion);
        }
        FileController fileController = new FileController();
        fileController.create(xmlPath, "file");
        XMLDataStream xmlStreamer = new XMLDataStream();
        xmlStreamer.filePath = xmlPath;
        xmlStreamer.saveXMLbyXML(xmlDoc);
    }




    */
}
