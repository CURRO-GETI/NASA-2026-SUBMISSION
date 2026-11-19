using Ceffy.Bridge;

namespace GIBBUS.LaunchUI
{
    // C# methods exposed to the HTML page as window.ceffy.unity.*
    [UnityMethods]
    public interface IGibbusLaunchCommands
    {
        LaunchSnapshot GetSnapshot();
        void SetChecklistItem(string id, bool acknowledged);
        bool AuthorizeLaunch();
        void RequestReturnToDesign();

        bool RequestStageSeparation(string stageId);
    }

    // Unity methods exposed to the HTML page as window.ceffy.ui.*
    [UiMethods]
    public interface IGibbusLaunchUi
    {
        void UpdateSnapshot(LaunchSnapshot snapshot);
        void ShowNotice(string message);
    }
}
