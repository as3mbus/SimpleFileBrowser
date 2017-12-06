public class SetupPortraitUserInterface : SetupUserInterface {

	protected override void SetupParents() {
		// Find directories parent to group directory buttons
		DirectoriesParent = FindGameObjectOrError("Items");
		// Find files parent to group file buttons
		FilesParent = FindGameObjectOrError("Items");
		SetButtonParentHeight(DirectoriesParent, DirectoryButtonHeight);
		SetButtonParentHeight(FilesParent, FilesButtonHeight);
	}
}
