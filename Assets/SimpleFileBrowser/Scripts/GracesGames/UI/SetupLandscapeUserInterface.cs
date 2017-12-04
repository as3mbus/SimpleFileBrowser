public class SetupLandscapeUserInterface : SetupUserInterface {

	protected override void SetupParents() {
		// Find directories parent to group directory buttons
		_directoriesParent = FindGameObjectOrError("Directories");
		// Find files parent to group file buttons
		_filesParent = FindGameObjectOrError("Files");
	}
}
