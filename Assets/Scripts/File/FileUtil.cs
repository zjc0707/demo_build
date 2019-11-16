using System.Linq;
using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
public static class FileUtil
{
    private const string suffix = ".txt";
    public static void Save(string name, string content)
    {
        string filePath = Application.persistentDataPath + "/" + name + suffix;
        Debug.Log(filePath);
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, content);
        }
        byte[] rs1 = System.Text.Encoding.Default.GetBytes(content);
        Debug.Log(File.ReadAllText(filePath));
        Debug.Log(rs1.Count());
        FileStream fileStream = File.OpenRead(filePath);
        BinaryReader r = new BinaryReader(fileStream);
        r.BaseStream.Seek(0, SeekOrigin.Begin);
        byte[] rs = r.ReadBytes((int)r.BaseStream.Length);
        Debug.Log(rs.Count());
        string filePath2 = @"/Users/jcz/test.txt";
        File.WriteAllBytes(filePath2, rs1);
        Debug.Log(File.ReadAllText(filePath2));
    }
}