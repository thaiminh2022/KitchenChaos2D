using Godot;
using System;

public partial class DeliveryResultUI : Control
{

	private const string POP_UP_ANIMATION = "popup";

	private const string PANEL_STYLEBOX = "panel";

	[ExportGroup("Settings")]
	[Export] TextureRect deliveryTexure;
	[Export] TextureRect icon;
	[Export] Panel panel;
	[Export] AnimationPlayer anim;

	[ExportGroup("Success")]
	[Export] Texture2D deliverySuccessTexture;
	[Export] Texture2D successIcon;
	[Export] Color sucessColor;

	[ExportGroup("Failed")]
	[Export] Texture2D deliveryFailedTextture;
	[Export] Texture2D failedIcon;
	[Export] Color failedColor;





	public override void _Ready() {
		DeliveryManager.Instance.OnRecipeSucceeded += DeliveryManager_OnRecipeSucceeded;
		DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

		Hide();
	}

	private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e) {
		PlayAnimation();
		ChangePanelColor(failedColor);

		deliveryTexure.Texture = deliveryFailedTextture;
		icon.Texture = failedIcon;
	}

	private void DeliveryManager_OnRecipeSucceeded(object sender, EventArgs e) {
		PlayAnimation();
		ChangePanelColor(sucessColor);

		deliveryTexure.Texture = deliverySuccessTexture;
		icon.Texture = successIcon;
	}

	private void PlayAnimation() {
		Show();
		anim.Stop();
		anim.Play(POP_UP_ANIMATION);
	}

	private void ChangePanelColor(Color color) {
		var styleBox = panel.GetThemeStylebox(PANEL_STYLEBOX) as StyleBoxFlat;
		styleBox.BgColor = color;
		panel.AddThemeStyleboxOverride(PANEL_STYLEBOX, styleBox);
	}
}
