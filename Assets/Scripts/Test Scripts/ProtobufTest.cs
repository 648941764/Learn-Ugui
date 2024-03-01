using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using TestGoogleBuffer;
using System.IO;
using System.Threading.Tasks;

public class ProtobufTest : MonoBehaviour
{
     private void Start()
    {
        //Person person = new Person()
        //{
        //    Age = 12,
        //    Name = "Tom"
        //};

        //byte[] serializeData = Serialize(person);
        //Debug.Log(serializeData.Length);

        //Person deserializedPerson = Deserialize<Person>(serializeData);
        //Debug.Log("Deserialized Person: " + deserializedPerson.Name + ", " + deserializedPerson.Age);

        LoadAssetBundle();
    }

    private void LoadAssetBundle()
    {
        AssetBundleCreateRequest assetBundle = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/ui.unity2d");//�첽����AssetBundle��Դ��
        assetBundle.completed += AssetBundleLoaded; 
    }

    private void AssetBundleLoaded(AsyncOperation handle)
    {
        AssetBundleCreateRequest request = (AssetBundleCreateRequest) handle;
        AssetBundle bundle = request.assetBundle;
        if (bundle != null)
        {
            Debug.Log("<color=blue>��Դ���سɹ�</color>");
            AssetBundleRequest obj = bundle.LoadAssetAsync<GameObject>("CountPanel");//�첽����Bundle���������Դ
            //bundle.LoadAssetWithSubAssets("CountPanel");������Դ����������Դ��sprite�и���ͼƬΪ���ӣ����ڷ����˽⣬�˴��벻����Ч��
            GameObject.Instantiate(obj.asset as GameObject, transform);
        }

    }

    public static byte[] Serialize(IMessage message)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            message.WriteTo(stream);
            return stream.ToArray();
        }
    }

    public static T Deserialize<T>(byte[] data) where T : IMessage<T>, new()
    {
        T message = new T();
        using (MemoryStream stream = new MemoryStream(data))
        {
            message.MergeFrom(stream);
        }
        return message;
    }
}

