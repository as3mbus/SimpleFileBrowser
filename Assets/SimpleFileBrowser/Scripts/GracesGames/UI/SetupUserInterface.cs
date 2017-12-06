using SimpleFileBrowser.Scripts.GracesGames;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class SetupUserInterface : MonoBehaviour {

	private FileBrowser _fileBrowser;
	
	// Button used to select a file to save/load
	private GameObject _selectFileButton;

	// Game object that represents the current path
	private GameObject _pathText;

	// Game object  and  InputField that represents the name of the file to save
	private GameObject _saveFileText;
	private InputField _saveFileTextInputFile;

	// Game object (Text) that represents the name of the file to load
	private GameObject _loadFileText;

	// Game object used as the parent for all the Directories of the current path
	protected GameObject _directoriesParent;

	// Game object used as the parent for all the Files of the current path
	protected GameObject _filesParent;
	
	// Input field and variable to allow file search
	private InputField _searchInputField;
	
	// Dimension used to set the scale of the UI
	// Represented using a 0-1 slider in the editor
	[Range(0.0f, 1.0f)] public float FileBrowserScale = 1f;

	// Use this for initialization
	public void Setup(FileBrowser fileBrowser) {
		_fileBrowser = fileBrowser;
		name = "FileBrowserUI";
		transform.localScale = new Vector3(FileBrowserScale, FileBrowserScale, 1f);
		SetupClickListeners();
		SetupTextLabels();
		SetupParents();
		SetupSearchInputField();
		_fileBrowser.SetUiGameObjects(_selectFileButton, _pathText, _loadFileText, _saveFileText, _saveFileTextInputFile,
			_directoriesParent, _filesParent);
	}

	// Setup click listeners for buttons
	private void SetupClickListeners() {
		// Hook up DirectoryBackward method to DirectoryBackwardButton
		FindButtonAndAddOnClickListener("DirectoryBackButton", _fileBrowser.DirectoryBackward);
		// Hook up DirectoryForward method to DirectoryForwardButton
		FindButtonAndAddOnClickListener("DirectoryForwardButton", _fileBrowser.DirectoryForward);
		// Hook up DirectoryUp method to DirectoryUpButton
		FindButtonAndAddOnClickListener("DirectoryUpButton", _fileBrowser.DirectoryUp);
		// Hook up CloseFileBrowser method to CloseFileBrowserButton
		FindButtonAndAddOnClickListener("CloseFileBrowserButton", _fileBrowser.CloseFileBrowser);
		// Hook up SelectFile method to SelectFileButton
		_selectFileButton = FindButtonAndAddOnClickListener("SelectFileButton", _fileBrowser.SelectFile);
	}

	// Setup path, load and save file text
	private void SetupTextLabels() {
		// Find pathText game object to update path on clicks
		_pathText = FindGameObjectOrError("PathText");
		// Find loadText game object to update load file text on clicks
		_loadFileText = FindGameObjectOrError("LoadFileText");

		// Find saveFileText game object to update save file text 
		// and hook up onValueChanged listener to check the name using CheckValidFileName method
		_saveFileText = FindGameObjectOrError("SaveFileText");
		_saveFileTextInputFile = _saveFileText.GetComponent<InputField>();
		_saveFileTextInputFile.onValueChanged.AddListener(_fileBrowser.CheckValidFileName);
	}

	// Setup parents object to hold directories and files
	protected abstract void SetupParents();

	// Setup search filter
	private void SetupSearchInputField() {
		// Find search input field and get input field component
		// and hook up onValueChanged listener to update search results on value change
		_searchInputField = FindGameObjectOrError("SearchInputField").GetComponent<InputField>();
		_searchInputField.onValueChanged.AddListener(_fileBrowser.UpdateSearchFilter);
	}
	
	// Finds and returns a game object by name or prints an error and return null
	protected GameObject FindGameObjectOrError(string objectName) {
		GameObject foundGameObject = GameObject.Find(objectName);
		if (foundGameObject != null) {
			return foundGameObject;
		} 			
		Debug.LogError("Make sure " + objectName + " is present");
		return null;
	}
	
	// Tries to find a button by name and add an on click listener action to it
	// Returns the resulting button 
	private GameObject FindButtonAndAddOnClickListener(string buttonName, UnityAction listenerAction) {
		GameObject button = FindGameObjectOrError(buttonName);
		button.GetComponent<Button>().onClick.AddListener(listenerAction);
		return button;
	}
}
