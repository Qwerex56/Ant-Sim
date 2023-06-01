using Godot;
using System;
using System.Collections.Generic;

public partial class grid : Node2D {
  readonly int cellSize = 16;

  [Export]
  public PackedScene antScene;

  [Export]
  private int antCount = 5;

  [Export]
  private int foodCount = 3;

  private Vector2I gridSize;

  private AntCommunicationComponent antCommunication;

  private HashSet<Vector2I> foodPositions = new HashSet<Vector2I>();

  public override void _Ready() {
    var root = GetTree().Root;
    antCommunication = root.GetNode<AntCommunicationComponent>("AntCommunicationComponent");

    gridSize = root.GetWindow().Size / 16;
    var gridField = gridSize.X * gridSize.Y;

    if (antCount + foodCount >= gridField) {
      antCount %= gridField;
      foodCount = gridField - antCount - 1;
    }

    CreateGrid();

    foreach (var antPosition in antCommunication.AntPositions) {
      ant ant = antScene.Instantiate<ant>();
      ant.gridPosition = antPosition;
      ant.GlobalPosition = antPosition * cellSize;
      AddChild(ant);
    }
  }

  public override void _Draw() {
    foreach (var foodPosition in foodPositions) {
      DrawCircle(foodPosition * cellSize, 6f, Colors.DarkRed);
    }
  }

  public void CreateGrid() {
    var rng = new RandomNumberGenerator();

    antCommunication.AntPositions.UnionWith(getRandPositions(antCount, rng));
    foodPositions.UnionWith(getRandPositions(foodCount, rng));
  }

  public void SpawnFood(int count) {
    var rng = new RandomNumberGenerator();

    foodPositions.UnionWith(getRandPositions(count, rng));
  }

  private HashSet<Vector2I> getRandPositions(int count, RandomNumberGenerator rng) {
    var positions = new HashSet<Vector2I>();
    var occupiedPositions = antCommunication.AntPositions.Count + foodPositions.Count;
      if (count >= gridSize.X * gridSize.Y - occupiedPositions) {
        return positions;
      }

      while (positions.Count < count) {
        int x = (int)(rng.Randi() % gridSize.X);
        int y = (int)(rng.Randi() % gridSize.Y);
        var pos = new Vector2I(x, y);

        if (antCommunication.AntPositions.Contains(pos) || foodPositions.Contains(pos)) {
          continue;
        }

        positions.Add(new Vector2I(x, y));
      }
    return positions;
  }
}
