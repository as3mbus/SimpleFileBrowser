using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Include these namespaces to use BinaryFormatter
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GracesGames {
	// Demo class to illustrate the usage of the FileBrowser script
	// Able to save and load files containing serialized data (e.g. text)
	public class DemoCaller : MonoBehaviour {

		// Use the file browser prefab
		public GameObject fileBrowserPrefab;
		// Define a file extension
		public string fileExtension;

		// Input field to get text to save
		private GameObject TextToSaveInputField;
		// Label to display loaded text
		private GameObject LoadedText;
		// Variable to save intermediate input result
		private string textToSave;

		// Find the input field, label objects and add a onValueChanged listener to the input field
		private void Start() {
			TextToSaveInputField = GameObject.Find ("TextToSaveInputField");
			TextToSaveInputField.GetComponent<InputField> ().onValueChanged.AddListener (UpdateTextToSave);

			LoadedText = GameObject.Find ("LoadedText");
		}

		// Updates the text to save with the new input (current text in input field)
		public void UpdateTextToSave(string text){
			textToSave = text;
		}

		// Open the file browser using boolean parameter so it can be called in GUI
		public void OpenFileBrowser(bool saving){
			if (saving) {
				OpenFileBrowser (FileBrowserMode.Save);
			} else {
				OpenFileBrowser (FileBrowserMode.Load);
			}
		}
			
		// Open a file browser to save and load files
		public void OpenFileBrowser(FileBrowserMode fileBrowserMode){
			// Create the file browser and name it
			GameObject fileBrowserObject = Instantiate (fileBrowserPrefab, this.transform);
			fileBrowserObject.name = "FileBrowser";
			// Set the mode to save or load
			FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser> ();
			if (fileBrowserMode == FileBrowserMode.Save) {
				fileBrowserScript.SaveFilePanel (this, "SaveFileUsingPath", "Level", fileExtension);
			} else {
				fileBrowserScript.OpenFilePanel (this, "LoadFileUsingPath", fileExtension);
			}
		}

		// Saves a file with the textToSave using a path
		private void SaveFileUsingPath(string path){
			if (path.Length != 0) {
				BinaryFormatter bFormatter = new BinaryFormatter ();
				// Create a file using the path
				FileStream file = File.Create (path);
				// Serialize the data (textToSave)
				bFormatter.Serialize (file, textToSave);
				// Close the created file
				file.Close ();
			} else {
				Debug.Log ("Invalid path given");
			}
		}

		// Loads a file using a path
		private void LoadFileUsingPath(string path){
			if (path.Length != 0) {
				BinaryFormatter bFormatter = new BinaryFormatter ();
				// Open the file using the path
				FileStream file = File.OpenRead (path);
				// Convert the file from a byte array into a string
				string fileData = bFormatter.Deserialize (file) as string;
				// We're done working with the file so we can close it
				file.Close ();
				// Set the LoadedText with the value of the file
				LoadedText.GetComponent<Text>().text = "Loaded data: \n" + fileData;
			} else {
				Debug.Log ("Invalid path given");
			}
		}
	}
}
