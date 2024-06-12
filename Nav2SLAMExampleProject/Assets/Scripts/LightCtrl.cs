using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCtrl : APIControllerBase
{
    // ��ά List ���ڴ洢���е��ݶ�������
    private List<List<string>> objectNames = new List<List<string>>();

    public float interactionDistance = 5f; // ��������
    public KeyCode interactKey = KeyCode.F; // ������

    private void Awake()
    {
        ServiceBotCtrl.Instance.RegisterAPIController("LightCtrl()", this);
        Debug.Log("LightCtrl API reported");

        // ��ʼ�� DoorGroup �µ�ֱ���Ӷ���
        InitializeLightsNames(transform);

        // ��ӡ��ά List ���ݣ���ѡ��
        // PrintObjectNames();
    }

    void Start()
    {

    }

    void Update()
    {
        // ��� F ���Ƿ񱻰���
        if (Input.GetKeyDown(interactKey)) {
            Debug.Log("Button F is pressed");
            // ���߼�⣬�����������һ������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ���߼��
            if (Physics.Raycast(ray, out hit, interactionDistance)) {
                Debug.Log("Within Distance");

                // ��⵽���߻����˵��ݿ���
                if (hit.collider.CompareTag("Switch")) {
                    Debug.Log("Hit Switch Collider");

                    // ��ȡ���ݶ���
                    GameObject lightBulb = GetCorrespondingLightBulb(hit.collider.gameObject);

                    if (lightBulb != null) {
                        // ���ÿ��صƺ���
                        ToggleLight(lightBulb);
                    }
                }
            }
        }
    }

    void InitializeLightsNames(Transform transform)
    {
        // ��ȡLightGroup�µ�����ֱ���Ӷ���
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }

        // ����ֱ���Ӷ��󣬻�ȡ���Ӷ��������
        for (int i = 0; i < children.Length; i++)
        {
            List<string> childNames = new List<string>();
            childNames.Add(children[i].name); // ��Ӹ����������

            Transform childTransform = children[i];
            for (int j = 0; j < childTransform.childCount; j++)
            {
                childNames.Add(childTransform.GetChild(j).name); // ����Ӷ��������
            }

            // ���Ӷ������ƴ��� List ��
            objectNames.Add(childNames);
        }
    }

    GameObject GetCorrespondingLightBulb(GameObject switchObject)
    {
        // ���� Switch �����ȡ��Ӧ�ĵ��ݶ���
        string lightBulbName = switchObject.name.Replace("Switch", "Light");
        return GameObject.Find(lightBulbName);
    }

    void ToggleLight(GameObject lightBulb)
    {
        GameObject lightSquareObject = lightBulb.transform.Find("LightSquare").gameObject;
        MeshRenderer lightSquareRenderer = lightSquareObject.GetComponent<MeshRenderer>();

        Light spotLight = lightBulb.GetComponentInChildren<Light>();

        if (lightSquareRenderer != null && spotLight != null)
        {
            if (!spotLight.enabled)
            {
                TurnOnLight(lightSquareRenderer, spotLight);
            }
            else
            {
                TurnOffLight(lightSquareRenderer, spotLight);
            }
        }
    }

    void TurnOnLight(MeshRenderer lightSquareRenderer, Light spotLight)
    {
        Debug.Log("Turning on Light");
        // �޸Ĳ���
        // lightSquareRenderer.material = yourOnMaterial;
        spotLight.enabled = true;
    }

    void TurnOffLight(MeshRenderer lightSquareRenderer, Light spotLight)
    {
        Debug.Log("Turning off Light");
        // �޸Ĳ���
        // lightSquareRenderer.material = yourOffMaterial;
        spotLight.enabled = false;
    }

    // ���������ʵ�ֵ���API�Ŀ����߼�
    public override void ExecuteAPI(Dictionary<string, string> parameters)
    {
        string place = parameters["place"];
        Debug.Log("Toggling light in " + place);
        List<string> lightBulbNames = new List<string>();
        foreach (var row in objectNames)
        {
            if (row.Count > 0 && row[0].Contains(place))
            {
                // ����ȥ����һ��Ԫ�غ�����б�
                lightBulbNames = row.GetRange(1, row.Count - 1);
            }
        }
        foreach (var lightBulbName in lightBulbNames)
        {
            Debug.Log("Toggling light " + lightBulbName);
            ToggleLight(GameObject.Find(lightBulbName));
        }
    }

    // �������������ڴ�ӡ��ά List ���ݣ���ѡ��
    private void PrintObjectNames()
    {
        for (int i = 0; i < objectNames.Count; i++)
        {
            Debug.Log("LightGroup: " + objectNames[i][0]);
            for (int j = 1; j < objectNames[i].Count; j++)
            {
                Debug.Log("    " + objectNames[i][j]);
            }
        }
    }
}
