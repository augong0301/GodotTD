using Godot;
using System;
using static System.Formats.Asn1.AsnWriter;

public partial class WTower : MarginContainer
{
	public Action OnMouseLeftClick;

	public PackedScene P_TOWER { get; set; }


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		P_TOWER =  ResourceLoader.Load<PackedScene>("res://src/action/Tower.tscn");
		GuiInput += WTower_GuiInput;
	}

	private void WTower_GuiInput(InputEvent input)
	{
		if (input is InputEventMouseButton button && input.IsReleased())
		{
			if (button.ButtonIndex == MouseButton.Left)
			{
				OnMouseLeftClick?.Invoke();
			}
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
