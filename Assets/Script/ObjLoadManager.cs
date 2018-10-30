using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ObjLoadManager : MonoBehaviour
{
    //Delegate
    public delegate void LoadStart();
    public event LoadStart loadStart;
    public delegate void ProgressDialogNumber(float val);
    public event ProgressDialogNumber progressDialogNumber;
    public delegate void ProgressDialogText(string tex);
    public event ProgressDialogText progressDialogText;
    public delegate void LoadFail(string errMsg);
    public event LoadFail loadFail;
    public delegate void LoadComplete(string dataUid);
    public event LoadComplete loadComplete;

    //Class
    public MainController mainController;
    public RayManager rayManager;
    public FileControlManager fileControlManager;

    //Loader Obj
    private GameObject loaderObj;
    public List<GameObject> objList = new List<GameObject>();
    private ArrayList _vertexArrayList = new ArrayList();
    private ArrayList _normalArrayList = new ArrayList();
    private ArrayList _uvArrayList = new ArrayList();
    private ArrayList _facesVertNormUV = new ArrayList();
    private List<Mesh> meshList;
    private Vector3[] _vertexArray;
    private Vector3[] _normalArray;
    private Vector2[] _uvArray;
    private int[] _triangleArray;
    public Transform createObjParent;
    private Mesh mesh;



    private void Awake()
    {
        objList = mainController.objList;
    }

    public void loadObjectFiles(string fileUrl)
    {
        if (fileUrl.Length == 0)
        {
            if (loadFail != null)
                loadFail("선택을 취소하셨습니다.");
            return;
        }
        else if (fileControlManager.getFileExtension(fileUrl) != "obj")
        {
            if (loadFail != null)
                loadFail("선택된 파일의 확장자가 .obj가 아닙니다.");
            return;
        }
        if (loadStart != null)
            loadStart();

        DirectoryInfo directoryInfo = new DirectoryInfo(fileControlManager.getFolderPath(fileUrl));
        FileInfo[] files = directoryInfo.GetFiles();
        //실직적 로드 시작.
        StartCoroutine(loadObjectFiles_coroutine(files, fileUrl));
    }

    private IEnumerator loadObjectFiles_coroutine(FileInfo[] fileArr, string fileUrl)
    {
        newCreateObject(fileControlManager.getfolderName(fileUrl));
        newCreateMeshList();

        DataInfo datainfo = saveDataInfo(fileArr);
        //오브젝트 로드
        for (int a = 0; a < fileArr.Length; a++)
        {
            yield return StartCoroutine(loader(fileArr[a].FullName));



            //로드중 플레이
            loaderObj.GetComponent<MeshCollider>().sharedMesh = meshList[a];
            //레이 활성화
            rayManager.startRay();
            //yield return new WaitForSeconds(0.5f);
            yield return null;
            //yield return null;
            //모델 움직임




            float[] heightValues = rayManager.getHeightValue();
            movingObject(heightValues);
            //움직임값 저장
            saveheightInfo(datainfo, heightValues);
            //로드 바 표현
            if (progressDialogNumber != null)
                progressDialogNumber((float)a / (float)fileArr.Length);
            if (progressDialogText != null)
                progressDialogText(fileArr[a].FullName);
        }

        rayManager.stopRay();
        loaderObj.SetActive(false);
        removeDatas();

        if (loadComplete != null)
            loadComplete(datainfo.dataUid);

    }

    private DataInfo saveDataInfo(FileInfo[] fileInfo)
    {
        DataInfo datainfo = new DataInfo();
        datainfo.dataName = fileControlManager.getfolderName(fileInfo[0].FullName);
        datainfo.dataFps = 30;
        datainfo.dataLed = "off";
        datainfo.dataNumberofFrame = fileInfo.Length;
        datainfo.dataUid = UidCreater.Instance.createUid();
        datainfo.dataUrl = Application.streamingAssetsPath + "/Data/" + datainfo.dataName + ".txt";
        DB_Data.Instance.dataInfoList.Add(datainfo);
        return datainfo;
    }

    private void saveheightInfo(DataInfo datainfo, float[] heightValues)
    {
        Height height = new Height();
        for (int a = 0; a < heightValues.Length; a++)
            height.floatLIst.Add(heightValues[a]);
        datainfo.heightList.Add(height);
    }

    private void movingObject(float[] heightVelue)
    {
        Debug.Log(objList.Count+" / "+heightVelue.Length);
        for (int a = 0; a < objList.Count; a++)
        {
            Vector3 pos = objList[a].transform.position;
            objList[a].transform.position = new Vector3(pos.x, pos.y, heightVelue[a]);
        }
    }

    private IEnumerator loader(string filePath)
    {
        newCreateLists();
        newCreateMesh();
        mesh.name = fileControlManager.getFileName(filePath);
        WWW www3d = new WWW("file://" + filePath);

        yield return www3d;

        if (www3d.isDone)
        {
            //Convert the text data to 3d mesh based on Unity
            string s = www3d.text;
            s = s.Replace("  ", " ");
            s = s.Replace("  ", " ");
            s = s.Replace(".", ",");
            LoadFile(s);

            //set the vertices and triangles to the unity mesh
            mesh.vertices = _vertexArray;
            mesh.triangles = _triangleArray;
            if (_uvArrayList.Count > 0)
                mesh.uv = _uvArray;
            if (_normalArrayList.Count > 0)
                mesh.normals = _normalArray;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            meshList.Add(mesh);
        }
        www3d.Dispose();
    }

    public void LoadFile(string s)
    {
        string[] lines = s.Split("\n"[0]);
        foreach (string item in lines)
        {
            ReadLine(item);
        }

        _vertexArray = new Vector3[_facesVertNormUV.Count];
        _uvArray = new Vector2[_facesVertNormUV.Count];
        _normalArray = new Vector3[_facesVertNormUV.Count];
        _triangleArray = new int[_facesVertNormUV.Count];

        int i = 0;
        foreach (Vector3 item in _facesVertNormUV)
        {
            _vertexArray[i] = (Vector3)_vertexArrayList[(int)item.x - 1];
            if (_uvArrayList.Count > 0)
            {
                try
                {
                    Vector3 tVec = (Vector3)_uvArrayList[(int)item.y - 1];
                    _uvArray[i] = new Vector2(tVec.x, tVec.y);
                }
                catch
                {
                    _uvArray[i] = new Vector2(0, 0);
                }
            }
            if (_normalArrayList.Count > 0)
            {
                _normalArray[i] = (Vector3)_normalArrayList[(int)item.z - 1];
            }
            _triangleArray[i] = i;
            i++;
        }
    }

    public void ReadLine(string s)
    {
        char[] charsToTrim = { ' ', '\n', '\t', '\r' };
        s = s.TrimEnd(charsToTrim);
        string[] words = s.Split(" "[0]);
        foreach (string item in words) item.Trim();
        if (words[0] == "v")
        {
            _vertexArrayList.Add(new Vector3(System.Convert.ToSingle(words[1]),
                System.Convert.ToSingle(words[2]), System.Convert.ToSingle(words[3])));
        }
        else if (words[0] == "vn")
        {
            _normalArrayList.Add(new Vector3(System.Convert.ToSingle(words[1]),
                System.Convert.ToSingle(words[2]), System.Convert.ToSingle(words[3])));
        }
        else if (words[0] == "vt")
        {
            _uvArrayList.Add(new Vector3(System.Convert.ToSingle(words[1]),
                System.Convert.ToSingle(words[2])));
        }
        else if (words[0] == "f")
        {
            ArrayList temp = new ArrayList();
            ArrayList triangleList = new ArrayList();
            for (int j = 1; j < words.Length; ++j)
            {
                Vector3 indexVector = new Vector3(0, 0);
                string[] indices = words[j].Split("/"[0]);
                indexVector.x = System.Convert.ToInt32(indices[0]);
                if (indices.Length > 1)
                {
                    if (indices[1] != "")
                        indexVector.y = System.Convert.ToInt32(indices[1]);
                }
                if (indices.Length > 2)
                {
                    if (indices[2] != "")
                        indexVector.z = System.Convert.ToInt32(indices[2]);
                }
                temp.Add(indexVector);
            }
            for (int i = 1; i < temp.Count - 1; ++i)
            {
                triangleList.Add(temp[0]);
                triangleList.Add(temp[i]);
                triangleList.Add(temp[i + 1]);
            }

            foreach (Vector3 item in triangleList)
            {
                _facesVertNormUV.Add(item);
            }
        }
    }

    private void newCreateObject(string folderName)
    {
        GameObject tempObj = new GameObject();
        tempObj.transform.parent = createObjParent;
        tempObj.layer = LayerMask.NameToLayer("AniObject");
        //tempObj.transform.position = new Vector3(0, 0f, 0);
        //tempObj.transform.eulerAngles = new Vector3(0, 0, 0);
        tempObj.transform.localScale = new Vector3(-1e-08f, 1e-08f, 1e-08f);
        tempObj.name = folderName;
        tempObj.AddComponent<MeshRenderer>();
        tempObj.GetComponent<MeshRenderer>().enabled = false;
        tempObj.AddComponent<MeshFilter>();
        tempObj.AddComponent<MeshCollider>();
        tempObj.AddComponent<Rigidbody>();
        tempObj.GetComponent<Rigidbody>().useGravity = false;
        tempObj.GetComponent<Rigidbody>().isKinematic = true;
        tempObj.SetActive(true);
        loaderObj = tempObj;
    }

    //일회성 메쉬 생성
    private void newCreateMeshList()
    {
        List<Mesh> tempMeshList = new List<Mesh>();
        meshList = tempMeshList;
    }

    //일회성 데이터 삭제
    private void removeDatas()
    {
        //Debug.Log("Garbage Collection");
        loaderObj = null;
        mesh = null;
        _vertexArray = new Vector3[0];
        _uvArray = new Vector2[0];
        _normalArray = new Vector3[0];
        _triangleArray = new int[0];
        if (meshList != null)
            meshList.RemoveRange(0, meshList.Count);
        if (_vertexArrayList != null)
            _vertexArrayList.RemoveRange(0, _vertexArrayList.Count);
        if (_normalArrayList != null)
            _normalArrayList.RemoveRange(0, _normalArrayList.Count);
        if (_uvArrayList.Count != null)
            _uvArrayList.RemoveRange(0, _uvArrayList.Count);
        if (_facesVertNormUV.Count != null)
            _facesVertNormUV.RemoveRange(0, _facesVertNormUV.Count);
    }

    //일회성 데이터 생성
    void newCreateLists()
    {
        _uvArrayList = new ArrayList();
        _normalArrayList = new ArrayList();
        _vertexArrayList = new ArrayList();
        _facesVertNormUV = new ArrayList();
    }

    private void newCreateMesh()
    {
        mesh = new Mesh();
    }

    private string chackingName(string fileName)
    {
        string name = fileName;
        int nameCount = 0;
        bool isNameCheck = false;
        while (!isNameCheck)
        {
            if (DB_Data.Instance.dataInfoList.Count == 0)
                isNameCheck = true;
            for (int a = 0; a < DB_Data.Instance.dataInfoList.Count; a++)
            {
                if (name == DB_Data.Instance.dataInfoList[a].dataName)
                {
                    name = fileName + "(" + (++nameCount) + ")";
                }
                if (a == (DB_Data.Instance.dataInfoList.Count - 1))
                {
                    if (name != DB_Data.Instance.dataInfoList[a].dataName)
                        isNameCheck = true;
                }
            }
        }
        return name;
    }



}
