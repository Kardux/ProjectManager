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

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class MySQLWrapper : MonoBehaviour
{
	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    private string m_SecretKey = "YOUR_SECRET_KEY";

    [SerializeField]
    private string m_CheckUserURL = "http://roy-bodereau.fr/ProjectManager/check_user.php?";
    [SerializeField]
    private string m_GetProjectsURL = "http://roy-bodereau.fr/ProjectManager/get_projects.php?";
    [SerializeField]
    private string m_SetProjectURL = "http://roy-bodereau.fr/ProjectManager/add_project.php?";
    [SerializeField]
    private string m_DeleteProjectURL = "http://roy-bodereau.fr/ProjectManager/delete_project.php?";
    [SerializeField]
    private string m_GetTodosURL = "http://roy-bodereau.fr/ProjectManager/get_todos.php?";
    [SerializeField]
    private string m_SetTodoURL = "http://roy-bodereau.fr/ProjectManager/add_todo.php?";
    [SerializeField]
    private string m_DeleteTodoURL = "http://roy-bodereau.fr/ProjectManager/delete_todo.php?";
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{

	}
	
	void Update()
	{
	
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    private IEnumerator CheckUser(string Username, string Password)
    {
        string _PostURL = m_CheckUserURL + "Username=" + WWW.EscapeURL(Username) + "&UserMD5=" + WWW.EscapeURL(Md5Sum(Username + Password));

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while checking user:\n" + _WWW.error);
        }
        else
        {
            Debug.Log(_WWW.text);
        }
    }

    private IEnumerator GetProjects(string UserMD5)
    {
        string _PostURL = m_GetProjectsURL + "UserMD5=" + WWW.EscapeURL(UserMD5);

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while retrieving projects:\n" + _WWW.error);
        }
        else
        {
            Debug.Log(_WWW.text);
        }
    }

    private IEnumerator SetProject(string UserMD5, string Name, System.DateTime StartDate, int TimeUntits)
    {
        string _CheckHash = Md5Sum(UserMD5 + m_SecretKey);

        string _PostURL = m_SetProjectURL + "UserMD5=" + WWW.EscapeURL(UserMD5) + "&Name=" + WWW.EscapeURL(Name) + "&StartDate=" + WWW.EscapeURL(StartDate.ToString("yyyy-MM-dd")) + "&TimeUnits=" + WWW.EscapeURL(TimeUntits.ToString()) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while adding project:\n" + _WWW.error);
        }
        else
        {
            Debug.Log("Project successfully added.");
        }
    }

    private IEnumerator DeleteProject(string UserMD5, int ID)
    {
        string _CheckHash = Md5Sum(UserMD5 + m_SecretKey);

        string _PostURL = m_DeleteProjectURL + "UserMD5=" + WWW.EscapeURL(UserMD5) + "&ID=" + WWW.EscapeURL(ID.ToString()) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while deleting project:\n" + _WWW.error);
        }
        else
        {
            Debug.Log("Project successfully deleted.\n" + _WWW.text);
        }
    }

    private IEnumerator GetTodos(string UserMD5)
    {
        string _PostURL = m_GetTodosURL + "UserMD5=" + WWW.EscapeURL(UserMD5);

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while retrieving todos:\n" + _WWW.error);
        }
        else
        {
            Debug.Log(_WWW.text);
        }
    }

    private IEnumerator SetTodo(string UserMD5, string Name)
    {
        string _CheckHash = Md5Sum(UserMD5 + m_SecretKey);

        string _PostURL = m_SetTodoURL + "UserMD5=" + WWW.EscapeURL(UserMD5) + "&Name=" + WWW.EscapeURL(Name) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while adding todo:\n" + _WWW.error);
        }
        else
        {
            Debug.Log("Todo successfully added.");
        }
    }

    private IEnumerator DeleteTodo(string UserMD5, int ID)
    {
        string _CheckHash = Md5Sum(UserMD5 + m_SecretKey);

        string _PostURL = m_DeleteTodoURL + "UserMD5=" + WWW.EscapeURL(UserMD5) + "&ID=" + WWW.EscapeURL(ID.ToString()) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while deleting todo:\n" + _WWW.error);
        }
        else
        {
            Debug.Log("Todo successfully deleted.");
        }
    }

    //////////////////////////////////////////////////////////////////////////
    private string Md5Sum(string InitialString)
    {
        System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
        byte[] _Bytes = _Encoding.GetBytes(InitialString);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider _MDProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] _HashedBytes = _MDProvider.ComputeHash(_Bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string _HashedString = "";

        for (int i = 0; i < _HashedBytes.Length; i++)
        {
            _HashedString += System.Convert.ToString(_HashedBytes[i], 16).PadLeft(2, '0');
        }

        return _HashedString.PadLeft(32, '0');
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////