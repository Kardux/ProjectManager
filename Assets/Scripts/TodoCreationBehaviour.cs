#region Author
/************************************************************************************************************
Author: BODEREAU Roy
Website: http://roy-bodereau.fr/
GitHub: https://github.com/Kardux
LinkedIn: be.linkedin.com/in/roybodereau
************************************************************************************************************/
#endregion

#region Copyright
/************************************************************************************************************
CC-BY-SA 4.0
http://creativecommons.org/licenses/by-sa/4.0/
Cette oeuvre est mise a disposition selon les termes de la Licence Creative Commons Attribution 4.0 - Partage dans les Memes Conditions 4.0 International.
************************************************************************************************************/
#endregion

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class TodoCreationBehaviour : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////
    //STATIC
    //////////////////////////////////////////////////////////////////////////
    public static TodoCreationBehaviour INSTANCE;
    //////////////////////////////////////////////////////////////////////////

	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private InputField[] m_TodosInputFields;

    [SerializeField]
    private GameObject m_AddButton;

    private bool m_ValuesChanged;

    private string[] m_Todos;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        INSTANCE = this;
        m_ValuesChanged = false;
	}

	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public void InputFieldsValuesChanged()
    {
        for (int i = 0; i < m_TodosInputFields.Length; i++)
        {
            m_TodosInputFields[i].gameObject.SetActive(i == 0 || !m_TodosInputFields[i - 1].text.Equals(""));
        }

        m_AddButton.SetActive(!m_TodosInputFields[0].text.Equals(""));

        m_ValuesChanged = true;
    }

    public void AddTodos()
    {
        m_AddButton.SetActive(false);

        List<string> _StringList = new List<string>();

        for (int i = 0; i < m_TodosInputFields.Length; i++)
        {
            if (!m_TodosInputFields[i].text.Equals(""))
            {
                _StringList.Add(m_TodosInputFields[i].text);
            }
        }

        m_Todos = new string[_StringList.Count];
        m_Todos = _StringList.ToArray();

        Clear();

        MenuController.INSTANCE.ChangeCanvas(4);
    }

    public string[] GetTodosToAdd()
    {
        if (m_ValuesChanged)
        {
            m_ValuesChanged = false;
            return m_Todos;
        }
        else
            return null;
    }

    public void Clear()
    {
        for (int i = 0; i < m_TodosInputFields.Length; i++)
        {
            m_TodosInputFields[i].text = "";
            m_TodosInputFields[i].gameObject.SetActive(i == 0);
        }
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////