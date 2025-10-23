public static class RuntimeInstaller
{
    public static void InstallAll()
    {
        var audio = new AudioManager();
        var save = new SaveManager();
        var scene = new SceneLoader();

        ServiceLocator.Register<IAudioManager>(audio);
        ServiceLocator.Register<ISaveManager>(save);
        ServiceLocator.Register<ISceneLoader>(scene);
    }
}