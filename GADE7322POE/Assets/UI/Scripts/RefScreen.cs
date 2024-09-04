using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RefScreen : UtkBase
{
    [TextArea] public string References;
    
    public UnityEvent onShowRef;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        var closeBtn = rootElement.Q<Button>("CloseBtn");
        closeBtn.clicked += () => onShowRef?.Invoke();
        var refLbl = rootElement.Q<Label>("ReferencesLbl");
        refLbl.text = References;
    }
}
