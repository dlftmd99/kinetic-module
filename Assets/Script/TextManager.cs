using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    //Class
    public FileControlManager fileControlManager;
    //
    public string streamingAssetsUrl;



    private void Start()
    {
        streamingAssetsUrl = Application.streamingAssetsPath;
    }

    public void LoadText(string textUrl)
    {
        StreamReader sr = new StreamReader(textUrl);
        List<string> readLineDataList = new List<string>();
        string lineText;
        DataInfo dataInfo = new DataInfo();
        dataInfo.dataName = fileControlManager.getFileName(textUrl);    //getFileNamebyFileUrl(fileUrl);
        dataInfo.dataUid = UidCreater.Instance.createUid();
        dataInfo.dataUrl = textUrl;
        
        //text파일 라인별로 읽어와 리스트 만들기
        while (true)
        {
            string readLineData = sr.ReadLine();
            if (readLineData != null)
            {
                readLineDataList.Add(readLineData);
            }
            else
            {
                // text파일 읽기 종료
                sr.Close();
                break;
            }
        }
       
        for (int a = 0; a < readLineDataList.Count; a++)
        {
            if (a == 0)
            {
                //Fps
                dataInfo.dataFps = int.Parse(readLineDataList[a]);
            }
            else if (a == 1)
            {
                //Pole count
                dataInfo.dataNumberofFrame = int.Parse(readLineDataList[a]);
            }
            else
            {
                Height height = new Height();
                int index;
                lineText = readLineDataList[a].Remove(0, readLineDataList[a].IndexOf(";") + 1);
                while (true)
                {
                    index = lineText.IndexOf(",");
                    if (index != -1)
                    {
                        float float_ = float.Parse(lineText.Substring(0, index));
                        float_ = (float_ / 1000f);// + 0.4363f;
                        height.floatLIst.Add((float_));
                        lineText = lineText.Remove(0, lineText.IndexOf(",") + 1);
                    }
                    else
                    {//문자열 끝에 도달.
                        index = lineText.IndexOf(";");
                        float float_ = float.Parse(lineText.Substring(0, index));
                        float_ = (float_ / 1000f);// + 0.4363f;
                        height.floatLIst.Add((float_));
                        break;
                    }
                }
                dataInfo.heightList.Add(height);
            }
        }
        DB_Data.Instance.dataInfoList.Add(dataInfo);
    }


    //로드한 obj높이값을 txt파일로 저장
    public void saveHeightListData(string dataUid)
    {
        DataInfo dataInfo = DB_Data.Instance.getData_sceneDataByUid(dataUid);
        StringBuilder sb = new StringBuilder();
        sb.Append(dataInfo.dataFps.ToString());
        sb.AppendLine();
        sb.Append(dataInfo.heightList.Count.ToString());
        sb.AppendLine();
        for (int a = 0; a < dataInfo.heightList.Count; a++)
        {
            sb.Append(a.ToString()).Append(";");
            for (int b = 0; b < dataInfo.heightList[a].floatLIst.Count; b++)
            {
                if (dataInfo.heightList[a].floatLIst.Count - 1 > b)
                    sb.Append(setValue(dataInfo.heightList[a].floatLIst[b]).ToString("N3")).Append(",");
                else
                    sb.Append(setValue(dataInfo.heightList[a].floatLIst[b]).ToString("N3")).Append(";");
            }
            if (dataInfo.heightList.Count - 1 > a)
                sb.Append("\n");
        }
        string textUrl = streamingAssetsUrl + "/Data/" + dataInfo.dataName + ".txt";
        fileControlManager.writerText(textUrl, sb);       
    }






        private float setValue(float value)
    {
        float value_ = value * 1000;// (value - 0.4363f) * 1000;
        value_ = (float)Math.Round(value_ / .00001f) * .00001f;
        value_ = (float)Math.Round(value_ / .0001f) * .0001f;
        value_ = (float)Math.Round(value_ / .001f) * .001f;
        value_ = (float)Math.Round(value_ / .01f) * .01f;
        value_ = (float)Math.Round(value_ / .1f) * .1f;
        return value_;
    }

 



}
