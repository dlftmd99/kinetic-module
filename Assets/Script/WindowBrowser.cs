using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class WindowBrowser : MonoBehaviour
{
    public static WindowBrowser instance;
    [DllImport("MFCLibrary")]
    public extern static void ShowFileDialog();
    [DllImport("MFCLibrary")]
    public extern static void ShowFolderDialog();
    [DllImport("MFCLibrary")]
    public extern static IntPtr GetFilePath();
    [DllImport("MFCLibrary")]
    public extern static IntPtr GetFolderPath();

    public string ShowFileDlg()
    {
        ShowFileDialog();
        IntPtr inptr_filePath = GetFilePath();
        string string_filePath = Marshal.PtrToStringUni(inptr_filePath);
        return string_filePath;
    }

    public string ShowFolderDlg()
    {
        ShowFolderDialog();
        IntPtr input_folderPath = GetFolderPath();
        string string_folderPath = Marshal.PtrToStringUni(input_folderPath);
        return string_folderPath;
    }
}
