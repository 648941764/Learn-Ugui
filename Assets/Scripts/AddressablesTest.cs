using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;

public class AddressablesTest : MonoBehaviour
{

    [SerializeField] private List<AssetReference> _assetReferences = new List<AssetReference>();
    [SerializeField] private List<Button> _buttons;
    [SerializeField] private Button _btnOpenPanel;

    private void Awake()
    {
        InitButtons();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadAsset<TestPanel>();
        }
    }

    private void InitButtons()
    {
        for (int i = 0;  i < _buttons.Count; i++)
        {
            int seceneIndex = i;
            _buttons[i].onClick.AddListener(() => LoadScene(seceneIndex));
        }

        _btnOpenPanel.onClick.AddListener(OnBtnOpenPanelClicked);
    }

    private void LoadScene(int index)
    {
        AsyncOperationHandle<SceneInstance> sceneHandle = Addressables.LoadSceneAsync(_assetReferences[index]/*.RuntimeKey.ToString()*/, LoadSceneMode.Additive);
        sceneHandle.Completed += OnSceneLoaded; 
    }

    private void LoadAsset<T>() where T : BaseUI
    {
        AsyncOperationHandle<GameObject> objHandle = Addressables.LoadAssetAsync<GameObject>("Assets/UIPanel/TestPanel.prefab");
        objHandle.Completed += OnAssetLoaded;
    }

    private void OnAssetLoaded<T>(AsyncOperationHandle<T> objHandle)
    {
        if (objHandle.Status == AsyncOperationStatus.Succeeded)
        {
            T asset = objHandle.Result;
            Debug.Log($"<color=#CCFF00>��Դ���سɹ�</color>");
            //Instantiate(asset, GameObject.Find("Canvas").transform);
        }

        else
        {
            Debug.Log($"<color=#FF0000>��Դ����ʧ��</color>");
        }
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"<color=green>�������سɹ�</color>");
        }
    }

    //����ר��

    public async void  OnBtnOpenPanelClicked()
    {
       await UIManager.Instance.OpenPanel<TestPanel>();
    }
}
