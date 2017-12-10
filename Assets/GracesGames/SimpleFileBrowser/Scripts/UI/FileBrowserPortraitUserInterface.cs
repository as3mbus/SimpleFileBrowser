using GracesGames.Common.Scripts;

namespace GracesGames.SimpleFileBrowser.Scripts.UI {

	public class FileBrowserPortraitUserInterface : FileBrowserUserInterface {

		protected override void SetupParents() {
			// Find directories parent to group directory buttons
			DirectoriesParent = Utilities.FindGameObjectOrError("Items");
			// Find files parent to group file buttons
			FilesParent = Utilities.FindGameObjectOrError("Items");
			SetButtonParentHeight(DirectoriesParent, ItemButtonHeight);
			SetButtonParentHeight(FilesParent, ItemButtonHeight);
		}
	}
}
