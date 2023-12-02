using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace KitchenChaos
{
    public static class Loader
    {
        public enum SceneName
        {
            MainMenuScene,
            LobbyScene,
            CharacterSelectScene,
            LoadingScene,
            GameScene
        }

        private static SceneName targetSceneName;

        public static void Load(SceneName targetSceneName)
        {
            Loader.targetSceneName = targetSceneName;

            SceneManager.LoadScene(SceneName.LoadingScene.ToString());
        }

        public static void LoadNetwork(SceneName targetSceneName)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(targetSceneName.ToString(), LoadSceneMode.Single);
        }

        public static void LoaderCallback()
        {
            SceneManager.LoadScene(targetSceneName.ToString());
        }
    }
}