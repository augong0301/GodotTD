using Godot;
using System;

public partial class GameForm : Control
{
	public Action<PackedScene> OnTowerReleased;

	public WTower Tower { get; set; }

	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		Tower = GetNode<WTower>("VBoxContainer/MarginContainer2/HBoxContainer/WTower");
		Tower.OnMouseLeftClick += OnTowerSelected;
	}

	private void OnTowerSelected()
	{
		OnTowerReleased?.Invoke(Tower.P_TOWER);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
