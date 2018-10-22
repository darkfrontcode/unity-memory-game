using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [SerializeField]
    private Transform puzzleField;

    [SerializeField]
    private GameObject btn;

	public void Awake ()
	{
		for(int i = 0; i < 8; i++)
        {
            GameObject button = Instantiate(btn);
            button.name = i.ToString();
            button.transform.SetParent(puzzleField, false);
        }
	}
}
