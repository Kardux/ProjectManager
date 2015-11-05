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
using System.Xml;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class MySQLWrapper : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////
    //STATIC
    //////////////////////////////////////////////////////////////////////////
    public static MySQLWrapper INSTANCE;
    public delegate void UserLogin();
    public static event UserLogin UserLoggedIn;
    public enum UserState { Disconnected, Unknown, Logged };
    //////////////////////////////////////////////////////////////////////////

	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    private string m_SecretKey = "YOUR_SECRET_KEY";

    [SerializeField]
    private string m_CheckUserURL = "http://roy-bodereau.fr/ProjectManager/check_user.php?";
    [SerializeField]
    private string m_UsernameAvailabilityURL = "http://roy-bodereau.fr/ProjectManager/username_availability.php?";
    [SerializeField]
    private string m_CreateUserURL = "http://roy-bodereau.fr/ProjectManager/create_user.php?";
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

    private UserState m_UserState;
    private string m_Username;
    private string m_UserMD5;

    private bool m_QueryRunning;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        INSTANCE = this;
        m_UserState = UserState.Disconnected;
        m_Username = "";
        m_UserMD5 = "";

        m_QueryRunning = false;
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public UserState GetUserState()
    {
        return m_UserState;
    }

    public string GetUsername()
    {
        return m_Username;
    }

    public void CheckUser(string Username, string Password)
    {
        StartCoroutine(CheckUserCoroutine(Username, Password));
    }

    public bool IsQueryRunning()
    {
        return m_QueryRunning;
    }

    public void ClearData()
    {
        m_UserState = UserState.Disconnected;
        m_Username = "";
        m_UserMD5 = "";
    }

    public IEnumerator UsernameAvailabilityCoroutine(string Username, System.Action<bool> Callback)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _PostURL = m_UsernameAvailabilityURL + "Username=" + WWW.EscapeURL(Username);

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while checking username availability:\n" + _WWW.error);
        }
        else
        {
            Debug.Log(_WWW.text);
            if (_WWW.text.Equals("true"))
            {
                Callback(true);
            }
            else
            {
                Callback(false);
            }
        }

        m_QueryRunning = false;
    }

    public IEnumerator CreateUserCoroutine(string Username, string Passcode)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _CheckHash = Md5Sum(Md5Sum(Username + Passcode) + m_SecretKey);

        string _PostURL = m_CreateUserURL + "Username=" + WWW.EscapeURL(Username) + "&UserMD5=" + WWW.EscapeURL(Md5Sum(Username + Passcode)) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            MessageBoxBehaviour.INSTANCE.SetMessageText("Error creating user: please try again.");
        }
        else
        {
            MessageBoxBehaviour.INSTANCE.SetMessageText("User successfully created. You can now login to start using your own project manager.");
        }

        m_QueryRunning = false;
    }

    private IEnumerator CheckUserCoroutine(string Username, string Passcode)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        m_Username = Username;
        m_UserMD5 = Md5Sum(Username + Passcode);

        string _PostURL = m_CheckUserURL + "Username=" + WWW.EscapeURL(m_Username) + "&UserMD5=" + WWW.EscapeURL(m_UserMD5);

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            Debug.LogError("Error while checking user:\n" + _WWW.error);
        }
        else
        {
            if (_WWW.text.Equals("true"))
            {
                m_UserState = UserState.Logged;

                if (UserLoggedIn != null)
                    UserLoggedIn();
            }
            else
            {
                m_Username = "";
                m_UserMD5 = "";
                m_UserState = UserState.Unknown;
            }
        }

        m_QueryRunning = false;
    }

    public IEnumerator GetProjectsCoroutine()
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _PostURL = m_GetProjectsURL + "UserMD5=" + WWW.EscapeURL(m_UserMD5);

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            MessageBoxBehaviour.INSTANCE.SetMessageText("An error happend while retrieving projects.");
            Debug.LogError("Error while retrieving projects:\n" + _WWW.error);
        }
        else
        {
            XmlDocument _XML = new XmlDocument();
            _XML.LoadXml(_WWW.text);

            XmlNode _GlobalNode = _XML.SelectSingleNode("GET_PROJECTS_RESULTS");
            XmlNodeList _ProjectsNodes = _GlobalNode.SelectNodes("PROJECT");

            string[] _Projects = new string[_ProjectsNodes.Count];
            string[] _StartDates = new string[_ProjectsNodes.Count];
            int[] _TimeUnits = new int[_ProjectsNodes.Count];
            int[] _IDs = new int[_ProjectsNodes.Count];

            for (int i = 0; i < _ProjectsNodes.Count; i++)
            {
                _Projects[i] = _ProjectsNodes.Item(i).SelectSingleNode("Name").InnerText;
                _StartDates[i] = _ProjectsNodes.Item(i).SelectSingleNode("StartDate").InnerText;
                int.TryParse(_ProjectsNodes.Item(i).SelectSingleNode("TimeUnits").InnerText, out _TimeUnits[i]);
                int.TryParse(_ProjectsNodes.Item(i).SelectSingleNode("ID").InnerText, out _IDs[i]);
            }

            ProjectCalendarBehaviour.INSTANCE.SetProjects(_Projects, _StartDates, _TimeUnits, _IDs);
        }

        m_QueryRunning = false;
    }

    private IEnumerator SetProjectCoroutine(string Name, System.DateTime StartDate, int TimeUntits)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _CheckHash = Md5Sum(m_UserMD5 + m_SecretKey);

        string _PostURL = m_SetProjectURL + "UserMD5=" + WWW.EscapeURL(m_UserMD5) + "&Name=" + WWW.EscapeURL(Name) + "&StartDate=" + WWW.EscapeURL(StartDate.ToString("yyyy-MM-dd")) + "&TimeUnits=" + WWW.EscapeURL(TimeUntits.ToString()) + "&CheckHash=" + _CheckHash;

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

        m_QueryRunning = false;
    }

    private IEnumerator DeleteProjectCoroutine(int ID)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _CheckHash = Md5Sum(m_UserMD5 + m_SecretKey);

        string _PostURL = m_DeleteProjectURL + "UserMD5=" + WWW.EscapeURL(m_UserMD5) + "&ID=" + WWW.EscapeURL(ID.ToString()) + "&CheckHash=" + _CheckHash;

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

        m_QueryRunning = false;
    }

    public IEnumerator GetTodosCoroutine()
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _PostURL = m_GetTodosURL + "UserMD5=" + WWW.EscapeURL(m_UserMD5);

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            MessageBoxBehaviour.INSTANCE.SetMessageText("An error happend while retrieving elements.");
            Debug.LogError("Error while retrieving todos:\n" + _WWW.error);
        }
        else
        {
            XmlDocument _XML = new XmlDocument();
            _XML.LoadXml(_WWW.text);
            
            XmlNode _GlobalNode = _XML.SelectSingleNode("GET_TODOS_RESULTS");
            XmlNodeList _TodosNodes = _GlobalNode.SelectNodes("TODO");

            string[] _Todos = new string[_TodosNodes.Count];
            int[] _IDs = new int[_TodosNodes.Count];

            for (int i = 0; i < _TodosNodes.Count; i ++)
            {
                _Todos[i] = _TodosNodes.Item(i).SelectSingleNode("Name").InnerText;
                int.TryParse(_TodosNodes.Item(i).SelectSingleNode("ID").InnerText, out _IDs[i]);
            }

            TodosListBehaviour.INSTANCE.Populate(_Todos, _IDs);
        }

        m_QueryRunning = false;
    }

    public IEnumerator SetTodoCoroutine(string Name)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _CheckHash = Md5Sum(m_UserMD5 + m_SecretKey);

        string _PostURL = m_SetTodoURL + "UserMD5=" + WWW.EscapeURL(m_UserMD5) + "&Name=" + WWW.EscapeURL(Name) + "&CheckHash=" + _CheckHash;

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

        m_QueryRunning = false;
    }

    public IEnumerator DeleteTodoCoroutine(int ID)
    {
        while (m_QueryRunning)
        {
            yield return new WaitForEndOfFrame();
        }
        m_QueryRunning = true;

        string _CheckHash = Md5Sum(m_UserMD5 + m_SecretKey);

        string _PostURL = m_DeleteTodoURL + "UserMD5=" + WWW.EscapeURL(m_UserMD5) + "&ID=" + WWW.EscapeURL(ID.ToString()) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            MessageBoxBehaviour.INSTANCE.SetMessageText("An error happend while deleting element.");
            Debug.LogError("Error while deleting todo:\n" + _WWW.error);
        }
        else
        {
            Debug.Log("Todo successfully deleted.");
        }

        m_QueryRunning = false;
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