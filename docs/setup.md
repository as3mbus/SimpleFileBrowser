---
Layout: page
title: Setup
permalink: /setup/

---

# Setup

***

The FileBrowser is not meant to be used as a standalone tool. 

Its function is to provide an interactive way to the user to determine paths. 

These paths are then passed on to a caller script which can use them to, for example, save/load files.

***

**Screen Orientation**

After instantiating the FileBrowser prefab, get the script using 

```csharp
GetComponent<FileBrowser>()
```

To setup the screen orientation, call 

```csharp
fileBrowserScript.SetupFileBrowser(ViewMode.Portrait or ViewMode.Landscape)_
```

Example given in the DemoCaller script.

***

**How to Create a FileBrowser Dialog:**

```csharp
// Open a file browser to save and load files
public void OpenFileBrowser(FileBrowserMode fileBrowserMode) {
    // Create the file browser and name it
    GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, this.transform);
    fileBrowserObject.name = "FileBrowser";
    // Set the mode to save or load
    FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
    if (fileBrowserMode == FileBrowserMode.Save) {
        fileBrowserScript.SaveFilePanel(this, "SaveFileUsingPath", "DemoText", FileExtension);
    } else {
        fileBrowserScript.OpenFilePanel(this, "LoadFileUsingPath", FileExtension);
    }
}
```
***

**UI Options**

The UI has a few options for scaling and coloring panels, buttons and fonts. 

These are self-explanatory and can be setup easily by the public variables in the UI prefabs.

***

**For more illustration see the DemoCaller prefab and script**
