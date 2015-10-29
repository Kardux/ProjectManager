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
using UnityEngine.EventSystems;

//////////////////////////////////////////////////////////////////////////
//CLASS
//////////////////////////////////////////////////////////////////////////
public class TodosListBehaviour : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////
    //STATIC
    //////////////////////////////////////////////////////////////////////////
    public static TodosListBehaviour INSTANCE;
    //////////////////////////////////////////////////////////////////////////
	#region Variables
	//////////////////////////////////////////////////////////////////////////
	//VARIABLES
	//////////////////////////////////////////////////////////////////////////
    [SerializeField]
    private GameObject m_TodoMockUp;

    [SerializeField]
    private Scrollbar m_Scrollar;

    [SerializeField]
    private GameObject m_MovingObject;

    [SerializeField]
    private float m_MovingSpeed;

    private int[] m_TodosIDs;

    private bool m_MouseDown;
    private float m_MouseDownY;
    private float m_MouseMaxDistance;
    private float m_MouseDownScrollbar;
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Handlers
	//////////////////////////////////////////////////////////////////////////
	//HANDLERS
	//////////////////////////////////////////////////////////////////////////
	void Start()
	{
        INSTANCE = this;

        m_MouseDown = false;
	}
	
	void Update()
	{
        if (!m_MouseDown)
        {
            if (Input.GetMouseButtonDown(0) && m_MovingObject.GetComponent<PointerHoveringIndicator>().IsHovered())
            {
                m_MouseDown = true;
                m_MouseDownY = Input.mousePosition.y;
                m_MouseDownScrollbar = m_Scrollar.value;
                m_MouseMaxDistance = 0.0f;
            }
        }
        else
        {
            if (Mathf.Abs(Input.mousePosition.y - m_MouseDownY) > m_MouseMaxDistance)
            {
                m_MouseMaxDistance = Mathf.Abs(Input.mousePosition.y - m_MouseDownY);
            }

            m_Scrollar.value = Mathf.Clamp(m_MouseDownScrollbar + (Input.mousePosition.y - m_MouseDownY) * m_MovingSpeed * 0.01f, 0.0f, 1.0f);

            if (Input.GetMouseButtonUp(0) || !m_MovingObject.GetComponent<PointerHoveringIndicator>().IsHovered())
            {
                m_MouseDown = false;
            }
        }

        if (m_TodosIDs != null)
        {
            m_MovingObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, Mathf.Clamp((m_TodosIDs.Length * 0.15f - 1.0f) * (1.0f - m_Scrollar.value), 0.0f, 9999.0f));
            m_MovingObject.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, Mathf.Clamp((m_TodosIDs.Length * 0.15f - 1.0f) * (1.0f - m_Scrollar.value) + 1.0f, 1.0f, 9999.0f));
            m_MovingObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            m_MovingObject.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        }
	}
	//////////////////////////////////////////////////////////////////////////
	#endregion

	#region Methods
	//////////////////////////////////////////////////////////////////////////
	//METHODS
	//////////////////////////////////////////////////////////////////////////
    public void TodoClicked(int TodoIndex)
    {
        if (m_MouseDown && m_MouseMaxDistance > Screen.height * 0.025f)
            return;

        StartCoroutine(TodoClickedCoroutine(TodoIndex));
    }
    
    public void Populate(string[] Todos, int[] IDs)
    {
        m_TodoMockUp.SetActive(true);

        foreach (Transform _Child in m_TodoMockUp.transform.parent)
        {
            if (_Child != m_TodoMockUp.transform)
            {
                Destroy(_Child.gameObject);
            }
        }

        m_TodosIDs = new int[IDs.Length];
        m_TodosIDs = IDs;

        for (int i = 0; i < Todos.Length; i ++)
        {
            GameObject _Todo = Instantiate(m_TodoMockUp);
            _Todo.transform.SetParent(m_TodoMockUp.transform.parent);
            _Todo.GetComponent<RectTransform>().anchorMin = new Vector2(0.0f, 0.85f - 0.15f * i);
            _Todo.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f - 0.15f * i);
            _Todo.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            _Todo.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            _Todo.name = "Todo_" + i.ToString();
            int _i = i;
            _Todo.GetComponent<Button>().onClick.AddListener(() => TodoClicked(_i));
            _Todo.GetComponentInChildren<Text>().text = "> " + Todos[i];
        }

        m_Scrollar.size = Mathf.Clamp(1.0f/(Todos.Length * 0.15f), 0.0f, 1.0f);

        m_TodoMockUp.SetActive(false);
    }

    private IEnumerator TodoClickedCoroutine(int TodoIndex)
    {
        QuestionBoxBehaviour.INSTANCE.SetQuestionText("Do you really want to delete this element?");

        yield return new WaitForSeconds(0.5f);

        while (QuestionBoxBehaviour.INSTANCE.IsDisplayed())
        {
            yield return new WaitForEndOfFrame();
        }

        if (QuestionBoxBehaviour.INSTANCE.GetAnswer())
        {
            GetComponent<CustomCanvasGroup>().FadeOut();
            while (GetComponent<CustomCanvasGroup>().IsFading())
            {
                yield return new WaitForEndOfFrame();
            }

            MenuController.INSTANCE.GetLoadingCanvas().FadeIn();
            while (MenuController.INSTANCE.GetLoadingCanvas().IsFading())
            {
                yield return new WaitForEndOfFrame();
            }

            StartCoroutine(MySQLWrapper.INSTANCE.DeleteTodoCoroutine(m_TodosIDs[TodoIndex]));
            StartCoroutine(MySQLWrapper.INSTANCE.GetTodosCoroutine());

            MenuController.INSTANCE.GetLoadingCanvas().FadeOut();
            while (MenuController.INSTANCE.GetLoadingCanvas().IsFading())
            {
                yield return new WaitForEndOfFrame();
            }

            GetComponent<CustomCanvasGroup>().FadeIn();
        }
    }
	//////////////////////////////////////////////////////////////////////////
	#endregion
}
//////////////////////////////////////////////////////////////////////////