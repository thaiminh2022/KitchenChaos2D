using Godot;

[GlobalClass]
public partial class AudioClipRefRes : Resource {

    [Export] public AudioStream[] chop;
    [Export] public AudioStream[] deliveryFailed;
    [Export] public AudioStream[] deliverySucceeded;
    [Export] public AudioStream[] footsteps;
    [Export] public AudioStream[] objectDrop;
    [Export] public AudioStream[] objectPickup;

    [Export] public AudioStream stoveSizzle;


    [Export] public AudioStream[] trash;
    [Export] public AudioStream[] warning;

}
