public class SetupLandscapeUserInterface : SetupUserInterface {

	protected override void SetupParents() {
		// Find directories parent to group directory buttons
		DirectoriesParent = FindGameObjectOrError("Directories");
		// Find files parent to group file buttons
		FilesParent = FindGameObjectOrError("Files");
		SetButtonParentHeight(DirectoriesParent, DirectoryButtonHeight);
		SetButtonParentHeight(FilesParent, FilesButtonHeight);
	}
}
