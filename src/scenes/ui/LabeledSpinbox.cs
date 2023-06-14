using Godot;

public partial class LabeledSpinbox : VBoxContainer {
  [Signal] public delegate void valueChangedEventHandler(int value);

  [Export] private string settingName = "Setting";
  [Export] private int maxValue = 10;
  [Export] private int minValue = 0;

  private int value;
  [Export] public int Value {get {return value;} set { this.value = value; }}

  [Export] private Label setName;
  [Export] private SpinBox setVal;

  public override void _Ready() {
    setName.Text = settingName;

    setVal.MaxValue = maxValue;
    setVal.MinValue = minValue;
    setVal.Value = value;
  }

  private void OnSettingValueChanged(float value) {
    this.value = (int)value;
    EmitSignal(nameof(valueChanged), value);
  }
}
