using Godot;
using System;
using System.Collections.Generic;

public partial class AntCommunicationComponent {
  private HashSet<Vector2I> foodPositions = new();
  public HashSet<Vector2I> FoodPositions {get { return foodPositions; } }

  private HashSet<Vector2I> antPositions = new();
  public HashSet<Vector2I> AntPositions {get { return antPositions; } }

  private HashSet<Vector2I> deathPositions = new();
  public HashSet<Vector2I> DeathPositions {get { return deathPositions; } }

  public enum CommuniqueTypeEnum {
    Food,
    Position,
    Death
  }

  public void PushMassage(CommuniqueTypeEnum type, List<Vector2I> args) {
    switch (type) {
      case CommuniqueTypeEnum.Food:
        foodPositions.UnionWith(args);
        break;
      
      case CommuniqueTypeEnum.Position:
        antPositions.UnionWith(args);
        break;
      
      case CommuniqueTypeEnum.Death:
        deathPositions.UnionWith(args);
        break; 
    }
  }

  public void PopMassage(CommuniqueTypeEnum type, Vector2I position) {
    switch (type) {
      case CommuniqueTypeEnum.Food:
        foodPositions.Remove(position);
        break;
      
      case CommuniqueTypeEnum.Position:
        antPositions.Remove(position);
        break;

      case CommuniqueTypeEnum.Death:
        deathPositions.Remove(position);
        break;
    }
  }
}
