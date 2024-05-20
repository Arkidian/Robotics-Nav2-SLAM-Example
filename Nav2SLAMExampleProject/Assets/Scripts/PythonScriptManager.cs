using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class PythonScriptManager
{
    const string scriptDirectory = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts";

    public static string CallPythonScript(string pythonScriptName)
    {
        // 创建进程启动信息对象
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = "cmd.exe";
        // processInfo.WorkingDirectory = scriptDirectory; // 设置工作目录为 Python 脚本所在路径
        processInfo.Arguments = "/c cd /d " + scriptDirectory + " && python " + pythonScriptName; // 使用 cd 命令切换目录，然后执行 Python 脚本
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;

        // 启动进程
        using (Process process = Process.Start(processInfo))
        {
            process.WaitForExit();

            // 读取 Python 脚本的输出结果
            string result = process.StandardOutput.ReadToEnd();
            UnityEngine.Debug.Log(result);
            return result;
        }
    }
}
