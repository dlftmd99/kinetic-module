using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class FileControlManager : MonoBehaviour
{    
    //Delegate
    public delegate void CreateFolderComplete(bool result, string errMsg);
    public event CreateFolderComplete createFolderComplete;
  

    public bool checkExists(string url, FILEFOLDER_TYPE type)
    {
        if(type == FILEFOLDER_TYPE.file)
        {
            FileInfo fileinfo = new FileInfo(url);
            if (fileinfo.Exists)
                return true;
            else
                return false;
        }
        else if(type == FILEFOLDER_TYPE.folder)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(url);
            if (directoryInfo.Exists)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public void createFolder(string url)
    {
        url = convertUrl(url);
        DirectoryInfo directoryInfo = new DirectoryInfo(url);
       
        if (directoryInfo.Exists == false)
        {
            directoryInfo.Create();
            StartCoroutine(checkCreateFolder(url));
        }
        else
        {
            if (createFolderComplete != null)
                createFolderComplete(false, "폴더가 이미 존재 합니다.");
        }
    }

    public void deleteFile(string fileUrl)
    {
        bool isExists = checkExists(fileUrl, FILEFOLDER_TYPE.file);
        if (isExists)
            File.Delete(fileUrl);
    }

    public void writerText(string textUrl, StringBuilder sb)
    {
        StreamWriter sw = new StreamWriter(textUrl);
        sw.WriteLine(sb.ToString()); //라인단위로 쓰기
        sw.Flush(); // 파일 쓰기 반드시 해준다.
        sw.Close(); // 파일 쓰기 반드시 해준다.
    }

    //API
    IEnumerator checkCreateFolder(string url)
    {
        float time =0.01f;
        bool state = false;
        while (!state)
        {
            yield return new WaitForSecondsRealtime(time + 0.01f);
            DirectoryInfo directoryInfo = new DirectoryInfo(url);
            url = convertUrl(url);
            if (directoryInfo.Exists == true)
            {
                if (createFolderComplete != null)
                {
                    state = true;
                    createFolderComplete(true, string.Empty);
                }
            }
            else
            {
                if (time > 5)
                {
                    state = true;
                    createFolderComplete(false, "생성시간 초과.");
                }
            }

        }
    }

    public string convertUrl(string url)
    {
        string[] str = url.Split('/');
        string newUrl = string.Empty;
        for (int a = 0; a < str.Length; a++)
        {
            if (a == 0)
                newUrl = str[a];
            if (a > 0)
                newUrl = newUrl + "/" + str[a];
        }
        return newUrl;
    }

    //폴더 정보 읽어오기
    public DirectoryInfo getDirectoryInfo(string folderUrl)
    {
        if(checkExists(folderUrl, FILEFOLDER_TYPE.folder))
        {
            DirectoryInfo di = new DirectoryInfo(folderUrl);
            return di;
        }
        else
        {
            return null;
        }


    }


    //파일 경로를 가지고 확장자 추출기(소문자로)
    public string getFileExtension(string fileUrl)
    {
        string extension = "";
        string[] splits_0 = fileUrl.Split('\\');
        string[] splits_1 = splits_0[splits_0.Length - 1].Split('.');
        extension = splits_1[splits_1.Length - 1];
        //Debug.Log("extension :: " + extension);
        return extension.ToLower();
    }

    //파일 경로를 가지고 폴더 경로 추출기
    public string getFolderPath(string fileUrl)
    {
        string folderPath = "";
        string[] splits_0 = fileUrl.Split('\\');
        for (int a = 0; a < (splits_0.Length - 1); a++)
        {
            if (a < (splits_0.Length - 2))
                folderPath += splits_0[a] + "\\";
            else
                folderPath += splits_0[a];
        }
        return folderPath;
    }

    //파일 경로를 가지고 폴더이름 추출기
    public string getfolderName(string fileUrl)
    {
        string[] splits_0 = fileUrl.Split('\\');
        return splits_0[splits_0.Length - 2];
    }

    //파일 경로를 가지고 파일이름 추출기
    public string getFileName(string filePath)
    {
        string[] splits_0 = filePath.Split('\\');
        string[] splits_1 = splits_0[splits_0.Length - 1].Split('.');
        return splits_1[0];
    }

    //파일 이름 순서대로 정렬
    public List<string> SortNumericallyFileUrl(List<string> urlList)
    {
        List<string> urlList_temp = new List<string>();
        string[] sorts = new string[urlList.Count];
        for (int a = 0; a < urlList.Count; a++)
            sorts[a] = urlList[a];
        Array.Sort(sorts, new AlphanumComparatorFast());
        for (int b = 0; b < sorts.Length; b++)
            urlList_temp.Add(sorts[b]);
        return urlList_temp;
    }
}




public class AlphanumComparatorFast : IComparer
{
    public int Compare(object x, object y)
    {
        string s1 = x as string;
        if (s1 == null)
        {
            return 0;
        }
        string s2 = y as string;
        if (s2 == null)
        {
            return 0;
        }

        int len1 = s1.Length;
        int len2 = s2.Length;
        int marker1 = 0;
        int marker2 = 0;

        // Walk through two the strings with two markers.
        while (marker1 < len1 && marker2 < len2)
        {
            char ch1 = s1[marker1];
            char ch2 = s2[marker2];

            // Some buffers we can build up characters in for each chunk.
            char[] space1 = new char[len1];
            int loc1 = 0;
            char[] space2 = new char[len2];
            int loc2 = 0;

            // Walk through all following characters that are digits or
            // characters in BOTH strings starting at the appropriate marker.
            // Collect char arrays.
            do
            {
                space1[loc1++] = ch1;
                marker1++;

                if (marker1 < len1)
                {
                    ch1 = s1[marker1];
                }
                else
                {
                    break;
                }
            } while (char.IsDigit(ch1) == char.IsDigit(space1[0]));

            do
            {
                space2[loc2++] = ch2;
                marker2++;

                if (marker2 < len2)
                {
                    ch2 = s2[marker2];
                }
                else
                {
                    break;
                }
            } while (char.IsDigit(ch2) == char.IsDigit(space2[0]));

            // If we have collected numbers, compare them numerically.
            // Otherwise, if we have strings, compare them alphabetically.
            string str1 = new string(space1);
            string str2 = new string(space2);

            int result;

            if (char.IsDigit(space1[0]) && char.IsDigit(space2[0]))
            {
                int thisNumericChunk = int.Parse(str1);
                int thatNumericChunk = int.Parse(str2);
                result = thisNumericChunk.CompareTo(thatNumericChunk);
            }
            else
            {
                result = str1.CompareTo(str2);
            }

            if (result != 0)
            {
                return result;
            }
        }
        return len1 - len2;
    }
}
