using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UITodoList : MonoBehaviour
{
    private UITodoItem mTodoItemPrototype;
    private InputField mInoutField;
    private Button mBtnAdd;

    private TodoList Model;

    [SerializeField] Transform contentObj;
    void Awake()
    {
        mTodoItemPrototype = transform.Find("TodoItemPrototype").GetComponent<UITodoItem>();
        mInoutField = transform.Find("InputContent").GetComponent<InputField>();
        mBtnAdd = transform.Find("BtnAdd").GetComponent<Button>();

    }

    // Start is called before the first frame update
    void Start()
    {
        Model = TodoList.Load();
        Debug.Log(JsonUtility.ToJson(Model));

        mBtnAdd.OnClickAsObservable().Subscribe(_ =>
        {
            if (!string.IsNullOrEmpty(mInoutField.text))
            {
                Model.TodoItems.Add(new TodoItem()
                {
                    Id = 3,
                    Content = new StringReactiveProperty(mInoutField.text),
                    Completed = new BoolReactiveProperty(false)
                });

                mInoutField.text = string.Empty;
            }
        });

        mInoutField.OnValueChangedAsObservable().Select(content => !string.IsNullOrEmpty(content))
            .SubscribeToInteractable(mBtnAdd);

//        mInoutField.OnValueChangedAsObservable().Subscribe(str =>
//        {
//            if (string.IsNullOrEmpty(str))
//            {
//                mBtnAdd.interactable = false;
//            }
//            else
//            {
//                mBtnAdd.interactable = true;
//            }
//        });


        Model.TodoItems.ObserveEveryValueChanged(item=>item.Count).Subscribe(_ =>
        {
            OnDataChanged();
        });

        OnDataChanged();
    }

    void OnDataChanged()
    {
        var childCount = contentObj.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(contentObj.GetChild(i).gameObject);
        }

        Model.TodoItems.Where(todoItem => !todoItem.Completed.Value).ToList().ForEach(todoItem =>
        {
            todoItem.Completed.Subscribe(completed =>
            {
                if (completed)
                    OnDataChanged();
            });

            var uitodoitem = Instantiate(mTodoItemPrototype);
            uitodoitem.transform.SetParent(contentObj);
            uitodoitem.transform.localScale = Vector3.one;
            uitodoitem.gameObject.SetActive(true);

            uitodoitem.SetModel(todoItem);

            Model.Save();
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
