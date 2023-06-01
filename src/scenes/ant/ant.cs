using Godot;
using System;
using System.Collections.Generic;

public partial class ant : Sprite2D {
  private enum saturationStatesEnum {
    // Ant will only search for food
    OVERFED = 12,
    // Ant will eat food if it is 1 step away
    HUNGRY = 4,
    // Ant only will be eating food, no matter where it is
    UNDERFED = 1
  }

  public Vector2I gridPosition { get; set; }
  
  private Queue<Vector2I> stepHistory = new();

  private AntCommunicationComponent antCommunication;


  private int maxStauration = 15;
  private int saturation = 15;

  public override void _Ready() {
    var root = GetTree().Root;
    antCommunication = root.GetNode<AntCommunicationComponent>("AntCommunicationComponent");
  }

  public override void _UnhandledInput(InputEvent ev) {
    if (ev.IsActionPressed("MoveAnt")) {
      Move();
      GlobalPosition = gridPosition * new Vector2I(16, 16);
    }
  }

  public void Move() {
    antCommunication.AntPositions.Remove(gridPosition);
    
    stepHistory.Enqueue(gridPosition);
    if (stepHistory.Count > 5) {
      stepHistory.Dequeue();
    }

    gridPosition = new (
      Mathf.Clamp(gridPosition.X + GetDirectionToFood().X, 0, 20),
      Mathf.Clamp(gridPosition.Y + GetDirectionToFood().Y, 0, 15)
    );

    for (int i = gridPosition.X - 1; i <= gridPosition.X + 1; i++) {
      for (int k = gridPosition.Y - 1; k <= gridPosition.Y + 1; k++) {
        
      }
    }
    antCommunication.PushPosition(gridPosition);

    // if (saturation >= (int)saturationStatesEnum.OVERFED) {
    //   // Seach for food
    // } else if (saturation >= (int)saturationStatesEnum.HUNGRY) {
    //   // Eat food and then search
    // } else if (saturation >= (int)saturationStatesEnum.UNDERFED) {
    //   // Eat food
    // } else {
    //   // Die
    // }

    saturation -= 1;
  }

  public Vector2I GetNearestFoodPosition() {
    var foodPositions = antCommunication.FoodPlaces;
    
    if (foodPositions.Count <= 0) {
      return -Vector2I.One;
    }
    
    Vector2I nearest = new(17, 17);

    foreach (var foodPosition in foodPositions) {
      if (nearest >= foodPosition) {
        nearest = foodPosition;
      }
    }
    
    return nearest;
  }

  private Vector2I GetDirectionToFood() {
    var nearestFood = GetNearestFoodPosition();

    if (nearestFood == -Vector2I.One) {
      var rng = new RandomNumberGenerator();
      return new((int)((rng.Randi() % 3) - 1), (int)((rng.Randi() % 3) - 1));
    }

    var direction = ((Vector2)(nearestFood - gridPosition)).Normalized();

    var dirX = Mathf.Sign(direction.X);
    var dirY = Mathf.Sign(direction.Y);
  
    return new(dirX, dirY);
  }
}
