using Godot;
using System;

public interface IHasProgress
{
    public event EventHandler<float> OnProgressChanged;
}

