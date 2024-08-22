using UnityEngine;
using UnityEngine.UIElements;

public abstract class UtkBase : MonoBehaviour
{
    private UIDocument _document;
    
    protected VisualElement rootElement;
    
    public virtual void Awake()
    {
        _document = GetComponent<UIDocument>();
        rootElement = _document.rootVisualElement.Q<VisualElement>("Root");
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