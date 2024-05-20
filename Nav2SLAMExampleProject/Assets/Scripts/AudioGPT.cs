using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PythonScriptManager;
using System.IO;

public class AudioGPT : MonoBehaviour
{
    private AudioClip recording;
    private string filename = "recording";
    private bool isRecording = false;
    private float recordStartTime;

    // ���� Python �ű�·��
    string pythonScriptName = "chatgpt_azure_script.py";
    const string scriptDirectory = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts";

    void Update()
    {
        // ��ʼ¼��
        if (Input.GetKeyDown(KeyCode.T) && !isRecording)
        {
            StartRecording();
        }
        // ֹͣ¼��������
        else if (Input.GetKeyUp(KeyCode.T) && isRecording)
        {
            StopRecording();
            // ����Python�ű�
            string result = CallPythonScript(pythonScriptName);
            Debug.Log("GPT Result: " + result);
        }
    }

    void StartRecording()
    {
        string microphoneDevice = Microphone.devices[0];
        isRecording = true;
        recording = Microphone.Start(microphoneDevice, false, 300, 44100);

        Debug.Log("Recording started...");
        recordStartTime = Time.time; // ��¼��ʼ¼����ʱ��
    }

    void StopRecording()
    {
        isRecording = false;
        Microphone.End(null);
        Debug.Log("Recording stopped");

        // ����ü���������
        int samplesToClip = (int)((Time.time - recordStartTime) * recording.frequency);
        if (samplesToClip > 0 && samplesToClip < recording.samples)
        {
            AudioClip clippedRecording = AudioClip.Create("ClippedRecording", samplesToClip, recording.channels, recording.frequency, false);
            float[] data = new float[samplesToClip];
            recording.GetData(data, 0);
            clippedRecording.SetData(data, 0);
            recording = clippedRecording;
        }

        // ����¼����ָ��·��
        SavWav.Save(filename, recording);
    }

}

