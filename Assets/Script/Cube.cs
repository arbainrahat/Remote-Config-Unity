using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cube : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public Material blue;
    public Text nameTxt;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ApplyRemoteChanges());
    }

    private IEnumerator ApplyRemoteChanges()
    {
        Debug.Log("Set Localize");
        yield return new WaitForSeconds(5f);
        if (ApplyRemoteConfigSettings.Instance.color)
        {
            print("change mat");
            meshRenderer.material = blue;
        }

        nameTxt.text = ApplyRemoteConfigSettings.Instance._name;

        meshRenderer.transform.localScale = new Vector3(ApplyRemoteConfigSettings.Instance.characterSize, ApplyRemoteConfigSettings.Instance.characterSize, ApplyRemoteConfigSettings.Instance.characterSize);
    }

    //public void SetLocalize()
    //{
    //    Debug.Log("Set Localize");
    //    if (ApplyRemoteConfigSettings.Instance.color)
    //    {
    //        print("change mat");
    //        meshRenderer.material = blue;
    //    }

    //    nameTxt.text = ApplyRemoteConfigSettings.Instance._name;

    //    meshRenderer.transform.localScale = new Vector3(ApplyRemoteConfigSettings.Instance.characterSize, ApplyRemoteConfigSettings.Instance.characterSize, ApplyRemoteConfigSettings.Instance.characterSize);
    //}

}
