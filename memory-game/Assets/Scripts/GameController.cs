using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public List<Button> btns = new List<Button>();
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new List<Sprite>();

    private bool firstGuess, secondGuess;

    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    [SerializeField]
    private Sprite bgImage;

    public void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites");
    }

    public void Start ()
	{
        GetButtons();
        AddListener();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
    }

    public void PickAPuzzle()
    {
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];
            countGuesses++;
            StartCoroutine(CheckIfThePuzzleMatch());
        }
    }

    public IEnumerator CheckIfThePuzzleMatch()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log(firstGuessPuzzle);
        Debug.Log(secondGuessPuzzle);

        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            btns[secondGuessIndex].image.color = new Color(0, 0, 0, 0);

            CheckIfTheGameIsFinished();
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;
        }

        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;
    }

    public void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;

        if(countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Game finished");
            Debug.Log($"It yook you {countGuesses} many guess(es) to finish the game.");
        }
    }

    public void Shuffle(List<Sprite> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void AddGamePuzzles()
    {
        int looper = btns.Count;
        int index = 0;

        for(int i = 0; i < looper; i++)
        {
            if(index == looper / 2)
            {
                index = 0;
            }

            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    private void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }

    private void AddListener()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(PickAPuzzle);
        }
    }
}
