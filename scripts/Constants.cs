using Godot;

public static class Constants
{
	public const string MOVE_LEFT = "move_left";
	public const string MOVE_RIGHT = "move_right";
	public const string MOVE_UP = "move_up";
	public const string MOVE_DOWN = "move_down";
	public const string INTEREACT = "interact";
	public const string ALT_INTEREACT = "alt_interact";



	public static void SetParent(this Node node, Node newParent)
	{
		var nodeParent = node.GetParent();
		if (nodeParent is not null)
		{
			nodeParent.RemoveChild(node);
			newParent.AddChild(node);
		}
		else
		{
			newParent.AddChild(node);
		}
	}

}
