using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UITodoItem : MonoBehaviour
{
    private Button mBtnComleted;

    private Text mContent;

    private TodoItem Model;

    void Awake()
    {
        mBtnComleted = transform.Find("BtnComplete").GetComponent<Button>();
        mContent = transform.Find("Content").GetComponent<Text>();

        mBtnComleted.OnClickAsObservable()
            .Subscribe(_ =>
            {
                Model.Completed.Value = true;
            });
    }

    public void SetModel(TodoItem model)
    {
        Model = model;
        UpdateView();
    }

    void UpdateView()
    {
        mContent.text = Model.Content.Value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
