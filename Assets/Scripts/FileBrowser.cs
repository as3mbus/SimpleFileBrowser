using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace GracesGames {

	// Enum used to define save and load mode
	public enum FileBrowserMode{ Save,Load }

	public class FileBrowser : MonoBehaviour {

		// Counter for errors
		private int errorCounter = 0;

		// The current path of the file browser
		// Starts using the current directory of the Unity Project
		private string currentPath = Directory.GetCurrentDirectory ();
		// The currently selected file
		private string currentFile;
		// The name for file to be saved
		private string saveFileName;

		// The parent object of the File Browser UI as prefab
		public GameObject fileBrowserUIPrefab;

		// Button used to close the file browser
		private GameObject closeFileBrowserButton;
		// Button used to select a file to save/load
		private GameObject selectFileButton;
		// Button used to go one directory up
		private GameObject directoryUpButton;
		// Allow 3 clicks per second for the directory up button
		public float timeBetweenClicks = 0.3f;  
		private float timestamp;

		// Game object that represents the current path
		private GameObject pathText;
		// Game object (InputField) that represents the name of the file to save
		private GameObject saveFileText;
		// Game object (Text) that represents the name of the file to load
		private GameObject loadFileText;

		// Game object used as the parent for all the Directories of the current path
		private GameObject directoriesParent;
		// Game object used as the parent for all the Files of the current path
		private GameObject filesParent;

		// Button Prefab used to create a button for each directory in the current path
		public GameObject fileBrowserDirectoryButtonPrefab;
		// Button Prefab used to create a button for each file in the current path
		public GameObject fileBrowserFileButtonPrefab;

		// Sprite used to represent the save button
		public Sprite saveImage;
		// Sprite used to represent the load button
		public Sprite loadImage;

		// Variable to set save or load mode
		private FileBrowserMode mode;

		// MonoBehaviour script used to call this script
		// Saved for the call back or cancellation
		private MonoBehaviour callerScript = null;
		// Method to be called of the callerScript when selecting a file or closing the file browser
		private string callbackMethod;
		// String file extension to filter results and save new files
		private string fileExtension;

		// On Awake, set up the File Browser
		void Awake () {
			SetupFileBrowser ();
		}

		// Finds and returns a game object by name or prints and error and increments error counter
		private GameObject FindGameObjectOrError(string name){
			GameObject gameObject = GameObject.Find (name);
			if (gameObject == null) {
				errorCounter++;
				Debug.LogError ("Make sure " + name + " is present");
				return null;
			} else {
				return gameObject;
			}
		}

		private void SetupFileBrowser(){
			// Find the canvas so UI elements can be added to it
			GameObject canvas = GameObject.Find("Canvas");
			if (canvas == null) {
				errorCounter++;
				Debug.LogError ("Make sure there is a canvas GameObject present in the Hierarcy (Create UI/Canvas)");
			}

			// Instantiate the file browser UI using the transform of the canvas and name it
			GameObject fileBrowserUITInstance = Instantiate (fileBrowserUIPrefab, canvas.transform);
			fileBrowserUITInstance.name = "FileBrowserUI";

			// Hook up DirectoryUp method to DirectoryUpButton
			directoryUpButton = FindGameObjectOrError ("DirectoryUpButton");
			directoryUpButton.GetComponent<Button> ().onClick.AddListener (() => {
				DirectoryUp (currentPath);
			});

			// Hook up CloseFileBrowser method to CloseFileBrowserButton
			closeFileBrowserButton = FindGameObjectOrError ("CloseFileBrowserButton");
			closeFileBrowserButton.GetComponent<Button> ().onClick.AddListener (() => {
				CloseFileBrowser ();
			});

			// Hook up SelectFile method to SelectFileButton
			selectFileButton = FindGameObjectOrError ("SelectFileButton");
			selectFileButton.GetComponent<Button> ().onClick.AddListener (() => {
				SelectFile ();
			});

			// Find pathText game object to update path on clicks
			pathText = FindGameObjectOrError ("PathText");
			// Find loadText game object to update load file text on clicks
			loadFileText = FindGameObjectOrError ("LoadFileText");

			// Find saveFileText game object to update save file text 
			// and hook up onValueChanged listener to check the name using CheckValidFileName method
			saveFileText = FindGameObjectOrError ("SaveFileText");
			saveFileText.GetComponent<InputField> ().onValueChanged.AddListener (CheckValidFileName);

			// Find directories parent to group directory buttons
			directoriesParent = FindGameObjectOrError ("Directories");
			// Find files parent to group file buttons
			filesParent = FindGameObjectOrError ("Files");
		}

		// Checks the current value of the InputField. If it is an empty string, disable the save button
		public void CheckValidFileName(string inputFieldValue){
			if (inputFieldValue == "") {
				selectFileButton.SetActive (false);
			} else {
				selectFileButton.SetActive (true);
			}
		}

		// Updates the input field value with a file name and extension
		public void SetFileNameInputField(string fileName, string fileExtension){
			saveFileText.GetComponent<InputField> ().text = fileName + "." + fileExtension;
		}

		// Updates the file browser by updating the path, file name, directories and files
		private void UpdateFileBrowser(){
			// Update the path text
			if (pathText != null && pathText.GetComponent<Text> () != null) {
				pathText.GetComponent<Text> ().text = currentPath;
			}
			// Update the file to load text
			if (loadFileText != null && loadFileText.GetComponent<Text> () != null) {
				loadFileText.GetComponent<Text> ().text = currentFile;
			}

			// Remove all current game objects under the directories parent
			if (directoriesParent.transform.childCount > 0) {
				foreach (Transform child in directoriesParent.transform) {
					GameObject.Destroy(child.gameObject);
				}
			}
			// For each directory in the current directory, create a DirectoryButton and hook up the DirectoryClick method
			foreach (string dir in Directory.GetDirectories (currentPath)) {
				GameObject button = Instantiate (fileBrowserDirectoryButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				button.GetComponent<Text> ().text = new DirectoryInfo(dir).Name;
				button.transform.SetParent (directoriesParent.transform, false);
				button.transform.localScale = Vector3.one;
				button.GetComponent<Button> ().onClick.AddListener (() => {
					DirectoryClick (dir);
				});
			}

			// Remove all current game objects under the files parent
			if (filesParent.transform.childCount > 0) {
				foreach (Transform child in filesParent.transform) {
					GameObject.Destroy(child.gameObject);
				}
			}
			// For each file in the current directory, create a FileButton and hook up the FileClick method
			foreach (string file in Directory.GetFiles (currentPath)) {
				GameObject button = Instantiate (fileBrowserFileButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				// When in Load mode, disable the buttons with different extension than the given file extension
				if (mode == FileBrowserMode.Load) {
					disableWrongExtensionFiles (button, file);
				}
				button.GetComponent<Text> ().text = Path.GetFileName (file);
				button.transform.SetParent (filesParent.transform, false);
				button.transform.localScale = Vector3.one;
				button.GetComponent<Button> ().onClick.AddListener (() => {
					FileClick (file);
				});
			}
		}

		// Disables file buttons with files that have a different file extension (than given to the OpenFilePanel)
		private void disableWrongExtensionFiles(GameObject button, string file){
			if (!file.EndsWith ("." + fileExtension)) {
				button.GetComponent<Button> ().interactable = false;
			}
		}

		// When a directory is clicked, update the path and the file browser
		private void DirectoryClick (string path)
		{
			currentPath = path;
			UpdateFileBrowser ();
		}

		// When a file is click, validate and update the save file text or current file and update the file browser
		private void FileClick (string clickedFile)
		{
			// When in save mode, update the save name to the clicked file name
			// Else update the current file text
			if (mode == FileBrowserMode.Save) {
				string clickedFileName = Path.GetFileNameWithoutExtension(clickedFile);
				CheckValidFileName(clickedFileName);
				SetFileNameInputField (clickedFileName, fileExtension);
			} else {
				currentFile = clickedFile;
			}
			UpdateFileBrowser ();
		}

		// Moves one directory up and update file browser
		// Limited to 3 click per second to not skip directories
		private void DirectoryUp(string path){
			if (Time.time >= timestamp) {
				timestamp = Time.time + timeBetweenClicks;
				if (Directory.GetParent (path) != null) {
					currentPath = Directory.GetParent (path).FullName;
					UpdateFileBrowser ();
				}
			}
		}

		// When a file is selected (save/load button clicked), 
		// send a message to the caller script
		private void SelectFile(){
			// When saving, send the path and new file name, else the selected file
			if (mode == FileBrowserMode.Save) {
				string inputFieldValue = saveFileText.GetComponent<InputField> ().text;
				// Additional check for invalid input field value
				// Should never be true due to onValueChanged check with toggle on save button
				if (inputFieldValue == null || inputFieldValue == "") {
					Debug.LogError ("Invalid file name given");
				} else {
					SendCallbackMessage(currentPath + "/" + inputFieldValue);
				}
			} else {
				SendCallbackMessage(currentFile);
			}
		}


		// Closes the file browser and send back an empty string
		private void CloseFileBrowser(){
			SendCallbackMessage("");
		}

		// Sends back a message to the callerScript and callbackMethod
		// Then destroys the FileBrowser
		private void SendCallbackMessage(string message){
			callerScript.SendMessage (callbackMethod, message);
			Destroy ();
		}

		// Opens a file browser in save mode
		// Requires a caller script and a method for the callback result
		// Also requires a default file and a file extension
		public void SaveFilePanel (MonoBehaviour callerScript, string callbackMethod, string defaultName, string fileExtension){
			// Make sure the file extension is not null, else set it to "" (no extension for the file to save)
			if (fileExtension == null) {
				fileExtension = "";
			}
			mode = FileBrowserMode.Save;
			saveFileText.SetActive (true);
			loadFileText.SetActive (false);
			selectFileButton.GetComponent<Image>().sprite = saveImage;
			// Update the input field with the default name and file extension
			SetFileNameInputField (defaultName, fileExtension);
			FilePanel (callerScript, callbackMethod, fileExtension);
		}

		// Opens a file browser in load mode
		// Requires a caller script and a method for the callback result 
		// Also a file extension used to filter the loadable files
		public void OpenFilePanel(MonoBehaviour callerScript, string callbackMethod, string fileExtension){
			// Make sure the file extension is not invalid, else set it to * (no filter for load)
			if (fileExtension == null || fileExtension == "") {
				fileExtension = "*";
			}
			mode = FileBrowserMode.Load;
			loadFileText.SetActive (true);
			selectFileButton.GetComponent<Image>().sprite = loadImage;
			saveFileText.SetActive (false);
			FilePanel (callerScript, callbackMethod, fileExtension);
		}

		// Generic file browser panel to remove duplicate code
		private void FilePanel(MonoBehaviour callerScript, string callbackMethod, string fileExtension){
			// Set values
			this.fileExtension = fileExtension;
			this.callerScript = callerScript;
			this.callbackMethod = callbackMethod;
			// Call update once to set all files for initial directory
			UpdateFileBrowser ();
		}

		// Destroy this file browser (the UI and the GameObject)
		private void Destroy (){
			Destroy (GameObject.Find ("FileBrowserUI"));
			Destroy (GameObject.Find ("FileBrowser"));
		}
	}
}