using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
    MainMenuScene,
    GameScene,
    LoadingScene
   }

   private static Scene targetScene;

   public static void LoadTargetScene(Scene targetScene) {
    Loader.targetScene = targetScene;
    SceneManager.LoadScene(Scene.LoadingScene.ToString());
   }

   public static void LoadCallback() {
    SceneManager.LoadScene(targetScene.ToString());
   }
    
}
