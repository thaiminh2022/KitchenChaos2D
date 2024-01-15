using Godot;
using System;

public partial class MoneyCounterUI : Control
{
	const string MONEY_CHANGE_ANIMATION = "money_change";
    private const string TEXT_COLOR_THEME_OVERRIDE = "font_color";
    [Export] Label moneyText;
    [Export] Label moneyChangeText;
	[Export] Color lossMoneyColor;
    [Export] Color gainMoneyColor;


    [Export] AnimationPlayer moneyChangeAnimator;

	public override void _Ready() {
		UpdateVisual();
		MoneyManager.Instance.OnMoneyChange += MoneyManager_OnMoneyChange;
        MoneyManager.Instance.OnMoneyGain += MoneyManager_OnMoneyGain;
        MoneyManager.Instance.OnMoneyLoss += MoneyManager_OnMoneyLoss;
}

    private void MoneyManager_OnMoneyLoss(object sender, int e) {
        moneyChangeText.Text = $"-{e}";
		moneyChangeText.AddThemeColorOverride(TEXT_COLOR_THEME_OVERRIDE, lossMoneyColor);

		PlayerMoneyChangeAnimation();
    }

    private void MoneyManager_OnMoneyGain(object sender, int e) {
		moneyChangeText.Text = $"+{e}";
        moneyChangeText.AddThemeColorOverride(TEXT_COLOR_THEME_OVERRIDE, gainMoneyColor);
        PlayerMoneyChangeAnimation();
    }

	private void PlayerMoneyChangeAnimation() {
		if(moneyChangeAnimator.IsPlaying()) {
			moneyChangeAnimator.Stop();
		}

		moneyChangeAnimator.Play(MONEY_CHANGE_ANIMATION);
    }
  

    private void MoneyManager_OnMoneyChange(object sender, EventArgs e) {
		UpdateVisual();
	}

	void UpdateVisual() {
		moneyText.Text = MoneyManager.Instance.GetMoneyAmount().ToString();
	}
}
