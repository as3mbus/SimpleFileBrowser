using SimpleFileBrowser.Scripts.GracesGames;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SimpleFileBrowser.Scripts.GracesGames.UI {

	public abstract class SetupUserInterface : MonoBehaviour {

		// The file browser using this user interface
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
		protected GameObject DirectoriesParent;

		// Game object used as the parent for all the Files of the current path
		protected GameObject FilesParent;

		// Input field and variable to allow file search
		private InputField _searchInputField;

		// The default font size for labels such as current path, save file name etc.
		private int _userInterfaceFontSize = 14;

		// The height of the directoy and file buttons
		protected int ItemButtonHeight = 70;

		// Setup the file browser user interface (get values from file browser prefab)
		public void Setup(FileBrowser fileBrowser, float uiWindowScale, int userInterfaceFontSize, int itemButtonHeight) {
			_fileBrowser = fileBrowser;
			name = "FileBrowserUI";
			transform.localScale = new Vector3(uiWindowScale, uiWindowScale, 1f);
			_userInterfaceFontSize = userInterfaceFontSize;
			ItemButtonHeight = itemButtonHeight;
			SetupClickListeners();
			SetupTextLabels();
			SetupParents();
			SetupSearchInputField();
			_fileBrowser.SetUiGameObjects(_selectFileButton, _pathText, _loadFileText, _saveFileText, _saveFileTextInputFile,
				DirectoriesParent, FilesParent);
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
			// Find the path and file label (path label optional in Portrait UI)
			GameObject pathLabel = GameObject.Find("PathLabel");
			GameObject fileLabel = FindGameObjectOrError("FileLabel");

			// Find pathText game object to update path on clicks
			_pathText = FindGameObjectOrError("PathText");
			// Find loadText game object to update load file text on clicks
			_loadFileText = FindGameObjectOrError("LoadFileText");

			// Find saveFileText game object to update save file text 
			// and hook up onValueChanged listener to check the name using CheckValidFileName method
			_saveFileText = FindGameObjectOrError("SaveFileText");
			_saveFileTextInputFile = _saveFileText.GetComponent<InputField>();
			_saveFileTextInputFile.onValueChanged.AddListener(_fileBrowser.CheckValidFileName);

			// Set font size for labels and texts
			if (pathLabel != null) {
				pathLabel.GetComponent<Text>().fontSize = _userInterfaceFontSize;
			}
			fileLabel.GetComponent<Text>().fontSize = _userInterfaceFontSize;
			_pathText.GetComponent<Text>().fontSize = _userInterfaceFontSize;
			_loadFileText.GetComponent<Text>().fontSize = _userInterfaceFontSize;
			foreach (Text textComponent in _saveFileText.GetComponentsInChildren<Text>()) {
				textComponent.fontSize = _userInterfaceFontSize;
			}
		}

		// Setup parents object to hold directories and files (implemented in Landscape and Portrait version)
		protected abstract void SetupParents();

		// Setup search filter
		private void SetupSearchInputField() {
			// Find search input field and get input field component
			// and hook up onValueChanged listener to update search results on value change
			_searchInputField = FindGameObjectOrError("SearchInputField").GetComponent<InputField>();
			foreach (Text textComponent in _searchInputField.GetComponentsInChildren<Text>()) {
				textComponent.fontSize = _userInterfaceFontSize;
			}
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

		// Sets the height of a GridLayoutGroup located in the game object (parent of directies and files object)
		protected void SetButtonParentHeight(GameObject parent, int height) {
			Vector2 cellSize = parent.GetComponent<GridLayoutGroup>().cellSize;
			cellSize = new Vector2(cellSize.x, height);
			parent.GetComponent<GridLayoutGroup>().cellSize = cellSize;
		}
	}
}
