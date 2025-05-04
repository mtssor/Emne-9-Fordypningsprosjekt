using Godot;
using NewGameProject.Utilities.Strategy;

namespace NewGameProject.Entities.Items;

[GlobalClass]
public partial class Upgrade : Area2D
{
	[Export] private Label _upgradeLabel;
	[Export] private Sprite2D _sprite;
	[Export] private BaseWeaponStrategy _strategy = new();
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		_sprite.Texture = _strategy.Texture;
		_sprite.Hframes = 1;
		_sprite.Vframes = 2;
		_upgradeLabel.Text = _strategy.UpgradeText;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
			if (_strategy != null)
			{
				_sprite.Texture = _strategy.Texture;
				_upgradeLabel.Text = _strategy.UpgradeText;
			}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player.Player player)
		{
			player.Upgrades.Add(_strategy);
			QueueFree();
		}
	}
}