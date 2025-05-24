using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] VisualTreeAsset mainMenuAsset;
        [SerializeField] VisualTreeAsset multiplayerMenuAsset;
        [SerializeField] VisualTreeAsset optionsMenuAsset;

        private VisualElement root;
        private VisualElement mainMenu;
        private VisualElement multiplayerMenu;
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

        public void ShowMultiplayerMenu()
        {
            root.Clear();
            multiplayerMenu = multiplayerMenuAsset.CloneTree();
            root.Add(multiplayerMenu);

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

