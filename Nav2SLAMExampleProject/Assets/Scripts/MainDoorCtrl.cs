using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PythonScriptManager;
using System.Threading;
using System.Threading.Tasks;

public class MainDoorCtrl : MonoBehaviour
{
    // ���� Python �ű�·��
    string pythonScriptName = "face_recognition_script.py";

    // ���� Python �ű�����Ŀ¼
    string scriptDirectory = "D:\\Projects\\Unity\\Robotics-Nav2-SLAM-Example\\Robotics-Nav2-SLAM-Example\\Nav2SLAMExampleProject\\Assets\\Scripts\\PythonScripts";

    private int maxLoopCount = 3;
    private bool isMainDoorOpen = false;
    private bool isPlayerInArea = false;
    
    public bool autoCloseFunction = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Someone's outside the maindoor.");

            isPlayerInArea = true;
            if (transform.Find("DoorCamera") != null)
                TakePhotos();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInArea = false;
            if (autoCloseFunction)
                CloseDoor();
        }
    }

    private async void TakePhotos()
    {
        int loopCount = 0;
        bool isPassed = false;

        while (isPlayerInArea && !isPassed && loopCount < maxLoopCount)
        {
            string pythonScriptResult = "";

            // ����һ���߳������� CallPythonScript ����
            Thread thread = new Thread(() =>
            {
                pythonScriptResult = PythonScriptManager.CallPythonScript(pythonScriptName);
                Debug.Log("Python script result: " + pythonScriptResult);
            });

            // �����߳�
            thread.Start();

            // �ȴ��߳�ִ����ϣ����������̵߳�ִ��
            await Task.Run(() => thread.Join());

            // ���ݽ��ִ����Ӧ����
            isPassed = bool.Parse(pythonScriptResult);

            // ��������������˳�ѭ��
            if (isPassed)
            {
                OpenDoor();
                break;
            }

            // ����������
            loopCount++;
        }

        Debug.Log("Background thread finished.");
    }

    private void TakePhoto()
    {
        // �����߼�
        Camera doorCamera = transform.GetComponent<Camera>();
        doorCamera.Render();
        Texture2D photo = new Texture2D(doorCamera.pixelWidth, doorCamera.pixelHeight);
        photo.ReadPixels(new Rect(0, 0, doorCamera.pixelWidth, doorCamera.pixelHeight), 0, 0);
        photo.Apply();

        // ������Ƭ
        byte[] bytes = photo.EncodeToPNG();
        System.IO.File.WriteAllBytes(scriptDirectory + "\\photo.png", bytes);
    }

    // ִ�п��Ŷ�������
    void OpenDoor()
    {
        Debug.Log("Opening Main Door");
        Transform doorShift = transform.Find("DoorShift");
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", true);
        isMainDoorOpen = true;
    }

    // ִ�й��Ŷ�������
    void CloseDoor()
    {
        Debug.Log("Closing Main Door");
        Transform doorShift = transform.Find("DoorShift");
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", false);
        isMainDoorOpen = false;
    }
}
