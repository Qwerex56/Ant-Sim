using Godot;
using System;
using System.Collections.Generic;

public partial class grid : Node2D {
  [Export]
  public PackedScene antScene;

  private Environment env;

  public override void _Ready() {
    var root = GetTree().Root;
    env = root.GetNode<Environment>("GlobalEnvironment");
    env.InitializeGame();
    foreach (var item in env.AntPositons) {
      var ant = antScene.Instantiate<Ant>();
      ant.gridPosition = item;
      AddChild(ant);
    }
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed("MoveAnt", true)) {
      for (int i = GetChildCount() - 1; i >= 0; i--) {
        var ant = GetChild<Ant>(i);
        ant.Move();
        if (ant.saturation <= 0) {
          ant.QueueFree();
        }
      }
      env.SpawnFood();
      QueueRedraw();
    }
  }

  public override void _Draw()
  {
    foreach (var item in env.FoodPositions) {
      DrawCircle(item * env.CELL_SIZE + Vector2I.One * env.CELL_SIZE / 2, 7f, Colors.DarkRed);
    }
  }
}
