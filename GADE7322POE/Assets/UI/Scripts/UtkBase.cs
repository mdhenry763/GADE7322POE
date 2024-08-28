using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UtkBase : MonoBehaviour
{
    public bool startHidden;
    
    private UIDocument _document;
    
    protected VisualElement rootElement;
    
    public virtual void Awake()
    {
        _document = GetComponent<UIDocument>();
        rootElement = _document.rootVisualElement.Q<VisualElement>("Root");
        
    }
    
    protected virtual void Start()
    {
        if(startHidden) Hide();
    }
    

    public virtual void Show()
    {
        rootElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
    }

    public virtual void Hide()
    {
        rootElement.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
    }
}