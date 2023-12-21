
using Godot;
using System.Collections.Generic;

public partial class PlateCounterVisual : AnimatedSprite2D
{
    const string PLATE_ANIMATION = "plate_";

    [Export] private Marker2D counterTopPoint;
    [Export] private PlateCounter plateCounter;

    int visualPlateAmount = 0;

    public override void _Ready()
    {
        plateCounter.OnPlateSpawn += PlateCounter_OnPlateSpawn;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;

    }

    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        visualPlateAmount--;

        if(visualPlateAmount < 0)
        {
            visualPlateAmount = 0;
        }
    }

    private void PlateCounter_OnPlateSpawn(object sender, System.EventArgs e)
    {
        visualPlateAmount++;

        if(visualPlateAmount > 4)
        {
            visualPlateAmount = 4;
        }
    }
    private void ChangeVisual() {
        // Hard setup animation because shading in 2D is not too pratical
        Play(PLATE_ANIMATION + visualPlateAmount);
    }

}
