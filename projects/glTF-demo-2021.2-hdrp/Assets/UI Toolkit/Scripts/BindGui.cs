using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BindGui : MonoBehaviour
{
    [System.Serializable]
    public class ToggleBinding
    {
        public string name;
        public GameObject target;
    }

    public List<ToggleBinding> toggles;
    
    // Start is called before the first frame update
    void Start()
    {
        var document = GetComponent<UIDocument>();
        foreach (var t in toggles)
        {
            var tg = document.rootVisualElement.Q<Toggle>(t.name);
            tg.RegisterValueChangedCallback(evt => t.target.SetActive(evt.newValue));
            tg.value = t.target.activeSelf;
        }
    }
}
