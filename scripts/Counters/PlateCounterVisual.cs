
using Godot;

public partial class PlateCounterVisual : AnimatedSprite2D
{
    const string PLATE_ANIMATION = "plate_";

    [Export] private PlateCounter plateCounter;
    [Export] private AnimatedSprite2D plateCounterSelected;

    int visualPlateAmount = 0;

    public override void _Ready()
    {
        plateCounter.OnPlateSpawn += PlateCounter_OnPlateSpawn;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;

    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        visualPlateAmount--;

        // visual only support 0 to 4 so clamp it
        if(visualPlateAmount < 0)
        {
            visualPlateAmount = 0;
        }

        ChangeVisual();
    }

    private void PlateCounter_OnPlateSpawn(object sender, System.EventArgs e)
    {
        visualPlateAmount++;

        // visual only support 0 to 4 so clamp it
        if (visualPlateAmount > 4)
        {
            visualPlateAmount = 4;
        }

        ChangeVisual();
    }
    private void ChangeVisual() {
        // Hard setup animation because shading in 2D using gameengine is not pratical
        Play(PLATE_ANIMATION + visualPlateAmount);
        plateCounterSelected.Play(PLATE_ANIMATION + visualPlateAmount);
    }

}
