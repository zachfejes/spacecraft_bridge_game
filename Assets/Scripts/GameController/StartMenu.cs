using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	public GameObject creditsModal;

	public void NavigateToGame() {
		SceneManager.LoadScene("Scene0");
	}

	public void ExitGame() {
		#if UNITY_EDITOR
         	UnityEditor.EditorApplication.isPlaying = false;
		#else
         	Application.Quit();
		#endif
	}

	public void ToggleCredits() {
		if(creditsModal != null) {
			creditsModal.SetActive(!creditsModal.activeSelf);
		}
	}

}
