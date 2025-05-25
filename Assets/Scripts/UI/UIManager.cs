using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset mainMenuAsset;
        [SerializeField] VisualTreeAsset hostMenuAsset;
        [SerializeField] VisualTreeAsset joinMenuAsset;
        [SerializeField] VisualTreeAsset optionsMenuAsset;

        private VisualElement root;
        private VisualElement mainMenu;
        private VisualElement hostMenu;
        private VisualElement joinMenu;
        private VisualElement optionsMenu;

        void OnEnable()
        {
            root = GetComponentInParent<UIDocument>().rootVisualElement;

            ShowMainMenu();
        }

        public void ShowMainMenu()
        {
            root.Clear();
            mainMenu = mainMenuAsset.CloneTree();
            mainMenu.style.flexGrow = 1;
            root.Add(mainMenu);

            Button hostButton = mainMenu.Q<Button>("HostButton");
            hostButton.clicked += ShowHostMenu;
            Button joinButton = mainMenu.Q<Button>("JoinButton");
            joinButton.clicked += ShowJoinMenu;
            Button optionsButton = mainMenu.Q<Button>("OptionsButton");
            optionsButton.clicked += ShowOptionsMenu;
        }

        public void ShowHostMenu()
        {
            root.Clear();
            hostMenu = hostMenuAsset.CloneTree();
            hostMenu.style.flexGrow = 1;
            root.Add(hostMenu);

            Button backButton = hostMenu.Q<Button>("BackButton");
            backButton.clicked += ShowMainMenu;
        }

        public void ShowJoinMenu()
        {
            root.Clear();
            joinMenu = joinMenuAsset.CloneTree();
            joinMenu.style.flexGrow = 1;
            root.Add(joinMenu);

            Button backButton = joinMenu.Q<Button>("BackButton");
            backButton.clicked += ShowMainMenu;
        }

        public void ShowOptionsMenu()
        {
            root.Clear();
            optionsMenu = optionsMenuAsset.CloneTree();
            optionsMenu.style.flexGrow = 1;
            root.Add(optionsMenu);

            Button backButton = optionsMenu.Q<Button>("BackButton");
            backButton.clicked += ShowMainMenu;
        }
    }
}

