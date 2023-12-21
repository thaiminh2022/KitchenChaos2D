using Godot;

public interface IHasProgress
{
    [Signal]
    delegate void ProgressChangedEventHandler(float progressNormalized);
}

