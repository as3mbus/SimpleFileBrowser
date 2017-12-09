using GracesGames.Common.Scripts;

namespace GracesGames.SimpleFileBrowser.Scripts.UI {

    public class FileBrowserLandscapeUserInterface : FileBrowserUserInterface {

        protected override void SetupParents() {
            // Find directories parent to group directory buttons
            DirectoriesParent = Utilities.FindGameObjectOrError("Directories");
            // Find files parent to group file buttons
            FilesParent = Utilities.FindGameObjectOrError("Files");
            SetButtonParentHeight(DirectoriesParent, ItemButtonHeight);
            SetButtonParentHeight(FilesParent, ItemButtonHeight);
        }
    }
}

