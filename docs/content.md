---
Layout: page
title: Content
permalink: /content/
---

# Content

***

**Common**

- FiniteStack: stack used to store the path for the backward/forward feature (see credits)
- Utilities: class defining some project-independent methods

***

**SimpleFileBrowser**

_DemoFiles_

- DemoText: text file with text
- TextFileWithNewLines: text file with text (including new lines)

_Prefabs_

- CallerObject: GameObject to interact with the FileBrowser and used to illustrate its usage
- FileBrowser: GameObject with the FileBrowser script attached to it
- UI
    - DirectoryButton: GameObject used to represent the directories in the file browser
    - FileButton: GameObject used to represent the files in the file browser (one column for directories and files)
    - FileBrowserLandscapeUI: the landscape user interface for the file browser (two columns for directories and files)
    - FileBrowserPortraitUI: the portrait user interface for the file browser


_Scenes_

- DemoScene: a demo environment to demonstrate the file browser (save and load text files)

_Scripts_

- DemoCaller: script used to illustrate the usage of the FileBrowser
- FileBrowser: the logic that keeps track of the current directory/files and changes by the user
- UI
    - UserInterface: the logic that sets up and updates the user interface based on information from the FileBrowser. Contains an abstract method implemented differently by the landscape and portrait version.
    - LandscapeUserInterface: the landscape version of the user interface
    - PortraitUserInterface: the portrait version of the user interface

_Sprites_

- Sprites for the user interface