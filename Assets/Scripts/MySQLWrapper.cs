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

    //TODO all coroutine accessers

    public void ClearData()
    {
        m_UserState = UserState.Disconnected;
        m_Username = "";
        m_UserMD5 = "";
    }

    public IEnumerator UsernameAvailabilityCoroutine(string Username, System.Action<bool> Callback)
    {
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
    }

    public IEnumerator CreateUserCoroutine(string Username, string Passcode)
    {
        string _CheckHash = Md5Sum(Md5Sum(Username + Passcode) + m_SecretKey);

        string _PostURL = m_CreateUserURL + "Username=" + WWW.EscapeURL(Username) + "&UserMD5=" + WWW.EscapeURL(Md5Sum(Username + Passcode)) + "&CheckHash=" + _CheckHash;

        WWW _WWW = new WWW(_PostURL);
        yield return _WWW;

        if (_WWW.error != null)
        {
            MessageBoxBehaviour.INSTANCE.SetMessageTest("Error creating user: please try again.");
        }
        else
        {
            MessageBoxBehaviour.INSTANCE.SetMessageTest("User successfully created. You can now login to start using your own project manager.");
        }
    }

    private IEnumerator CheckUserCoroutine(string Username, string Passcode)
    {
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
    }

    private IEnumerator GetProjectsCoroutine(string UserMD5)
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

    private IEnumerator SetProjectCoroutine(string UserMD5, string Name, System.DateTime StartDate, int TimeUntits)
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

    private IEnumerator DeleteProjectCoroutine(string UserMD5, int ID)
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

    private IEnumerator GetTodosCoroutine(string UserMD5)
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

    private IEnumerator SetTodoCoroutine(string UserMD5, string Name)
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

    private IEnumerator DeleteTodoCoroutine(string UserMD5, int ID)
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