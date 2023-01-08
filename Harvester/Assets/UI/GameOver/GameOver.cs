using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var gameOver = gameObject.GetComponent<UIDocument>();
        var playAgain = gameOver.rootVisualElement.Q<Button>("play-again-button");
        playAgain.RegisterCallback<ClickEvent>(e => SceneManager.LoadScene(0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
