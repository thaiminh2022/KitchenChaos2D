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

	private int moneyEarned;
	private int moneySpent;

	public override void _EnterTree() {
		Instance = this;
	}

	public void AddMoney(int amount) {
		moneyAmount += amount;
		moneyEarned += amount;
		OnMoneyChange?.Invoke(this, EventArgs.Empty);

	}
	public void ReduceMoney(int amount) {
		moneyAmount -= amount;
		moneySpent += amount;
		OnMoneyChange?.Invoke(this, EventArgs.Empty);
	}


	public int GetMoneyAmount() => moneyAmount;
	public int GetMoneyEarned() => moneyEarned;
	public int GetMoneySpent() => moneySpent;
}
