using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    // һά List���ڴ洢�����Ŷ�������
    private List<string> doorNames = new List<string>();

    // һά List���ڴ洢�����ŵĿ���״̬
    private List<bool> doorStatus = new List<bool>();

    public float interactionDistance = 5f; // ��������
    public KeyCode interactKey = KeyCode.F; // ������

    void Start()
    {
        // ��ʼ�� DoorGroup �µ�ֱ���Ӷ���
        InitializeDoorNames(transform);

        // ��ӡһά List ���ݣ���ѡ��
        // PrintObjectNames();
    }

    void Update()
    {
        // ��� F ���Ƿ񱻰���
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log("Button F is pressed");
            // ���߼�⣬�����������һ������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���߼��
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Debug.Log("Within Distance");

                // ��⵽���߻������Ű�
                if (hit.collider.CompareTag("Door"))
                {
                    Debug.Log("Hit Door Collider");

                    // ��ȡ�Ŷ���
                    GameObject door = hit.collider.gameObject;

                    if (door != null)
                    {
                        // ���ÿ����ź���
                        ToggleDoor(door);
                    }
                }
            }
        }
    }

    // ��ʼ�� DoorGroup �µ�ֱ���Ӷ���
    void InitializeDoorNames(Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            // ����Ӷ������Ƶ� List ��
            doorNames.Add(child.name);
            doorStatus.Add(false);
        }
    }

    void ToggleDoor(GameObject door)
    {
        if (door != null) {
            Transform doorShift = door.transform.parent;
            string doorName = doorShift.transform.parent.name;
            // ������ŵ�״̬�ǹرյģ���򿪸��ţ���֮��Ȼ
            if (!doorStatus[doorNames.IndexOf(doorName)]) {
                UnityEngine.Debug.Log("Opening Door");
                OpenDoor(doorShift.gameObject);
                doorStatus[doorNames.IndexOf(doorName)] = true;
            }
            else {
                UnityEngine.Debug.Log("Closing Door");
                CloseDoor(doorShift.gameObject);
                doorStatus[doorNames.IndexOf(doorName)] = false;
            }
        }
    }

    // ִ�п��Ŷ�������
    void OpenDoor(GameObject doorShift)
    {
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", true);
    }

    // ִ�й��Ŷ�������
    void CloseDoor(GameObject doorShift)
    {
        doorShift.GetComponent<Animator>().SetBool("IsDoorOpen", false);
    }

    // �������������ڴ�ӡ��ά List ���ݣ���ѡ��
    private void PrintObjectNames()
    {
        Debug.Log("Door Names:");
        foreach (string name in doorNames)
        {
            Debug.Log(name);
        }
    }

}
