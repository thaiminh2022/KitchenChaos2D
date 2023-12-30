using Godot;

public partial class ResetStaticManager : Node {
    
    public override void _EnterTree() {
        DeliveryManager.ResetStatic();
        Player.ResetStatic();

        CuttingCounter.ResetStatic();
        BaseCounter.ResetStatic();
        TrashCounter.ResetStatic();
        PlayerSound.ResetStatic();

        SettingsUI.ResetStatic();
        PauseMenuUI.ResetStatic();

        GameManager.ResetStatic();
        SoundManager.ResetStatic();
        InputManager.ResetStatic();
    }
}
