public class SetupPortraitUserInterface : SetupUserInterface {

	protected override void SetupParents() {
		// Find directories parent to group directory buttons
		_directoriesParent = FindGameObjectOrError("Items");
		// Find files parent to group file buttons
		_filesParent = FindGameObjectOrError("Items");
	}
}
