using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class TodoItem
{
    public int Id;

    public StringReactiveProperty Content;

    public BoolReactiveProperty Completed;
}

public class TodoList
{

    public List<TodoItem> TodoItems = new List<TodoItem>()
    {
        new TodoItem
        {
            Id = 0,
            Content = new StringReactiveProperty("拿快递"),
            Completed = new BoolReactiveProperty(false)
        },
        new TodoItem
        {
            Id = 1,
            Content = new StringReactiveProperty("戴口罩"),
            Completed = new BoolReactiveProperty(false)
        },
    };

    public void Save()
    {
        PlayerPrefs.SetString("model",JsonUtility.ToJson(this));
    }

    internal static TodoList Load()
    {
        var jsonContent = PlayerPrefs.GetString("model", string.Empty);

        if (string.IsNullOrEmpty(jsonContent))
        {
            return new TodoList();
        }
        else
        {
            return JsonUtility.FromJson<TodoList>(jsonContent);
        }
     
    }
}