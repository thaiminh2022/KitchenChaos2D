using Godot;
using System;

public partial class MoneyManager : Node
{
	public static MoneyManager Instance { get; private set; }
	public static void ResetStatic() {
		Instance = null;
	}

	public event EventHandler OnMoneyChange;

	[Export]
	private int moneyAmount;

	public override void _EnterTree() {
		Instance = this;
	}

	public void AddMoney(int amount) {
		moneyAmount += amount;
		OnMoneyChange?.Invoke(this, EventArgs.Empty);

	}
	public void ReduceMoney(int amount) {
		moneyAmount -= amount;
		OnMoneyChange?.Invoke(this, EventArgs.Empty);
	}


	public int GetMoneyAmount() => moneyAmount;
}
