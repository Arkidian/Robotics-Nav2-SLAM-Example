using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public static class PythonScriptManager
{
    const string scriptDirectory = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts";
    private static Dictionary<string, string> scriptParameters = new Dictionary<string, string>();

    // ���ò����ķ���
    public static void SetParameters(Dictionary<string, string> parameters)
    {
        scriptParameters = parameters;
    }

    public static string CallPythonScript(string pythonScriptName)
    {
        // ��������������Ϣ����
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = "cmd.exe";
        // processInfo.WorkingDirectory = scriptDirectory; // ���ù���Ŀ¼Ϊ Python �ű�����·��
        processInfo.Arguments = "/c cd /d " + scriptDirectory + " && python " + pythonScriptName; // ʹ�� cd �����л�Ŀ¼��Ȼ��ִ�� Python �ű�
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;

        // ��������
        using (Process process = Process.Start(processInfo))
        {
            process.WaitForExit();

            // ��ȡ Python �ű���������
            string result = process.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(result);
            return result;
        }
    }

    public static string CallPythonScriptwithPara(string pythonScriptName)
    {
        // ��������������Ϣ����
        ProcessStartInfo processInfo = new ProcessStartInfo();
        // ƴ�Ӳ���
        string arguments = string.Join(" ", scriptParameters.Select(p => $"{p.Key}={p.Value}"));
        
        processInfo.FileName = "cmd.exe";
        // processInfo.WorkingDirectory = scriptDirectory; // ���ù���Ŀ¼Ϊ Python �ű�����·��
        processInfo.Arguments = $"/c cd /d {scriptDirectory} && python {pythonScriptName} {arguments}"; // ʹ�� cd �����л�Ŀ¼��Ȼ��ִ�� Python �ű�
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;

        // ��������
        using (Process process = Process.Start(processInfo))
        {
            process.WaitForExit();

            // ��ȡ Python �ű���������
            string result = process.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(result);
            return result;
        }
    }
}
