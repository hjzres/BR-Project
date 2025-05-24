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
            root.Add(mainMenu);

            Button optionsButton = mainMenu.Q<Button>("optionsButton");
            optionsButton.clicked += ShowOptionsMenu;
        }

        public void ShowHostMenu()
        {
            root.Clear();
            hostMenu = hostMenuAsset.CloneTree();
            root.Add(hostMenu);

            Button backButton = optionsMenu.Q<Button>("BackButton");
            backButton.clicked += ShowMainMenu;
        }

        public void ShowJoinMenu()
        {
            root.Clear();
            joinMenu = joinMenuAsset.CloneTree();
            root.Add(joinMenu);

            Button backButton = optionsMenu.Q<Button>("BackButton");
            backButton.clicked += ShowMainMenu;
        }

        public void ShowOptionsMenu()
        {
            root.Clear();
            optionsMenu = optionsMenuAsset.CloneTree();
            root.Add(optionsMenu);

            Button backButton = optionsMenu.Q<Button>("BackButton");
            backButton.clicked += ShowMainMenu;
        }
    }
}

