#include "funshield.h"

//globals
enum STATES {
  NORMAL,
  CONFIG
};

class Button {
  public:
    Button(int pin){
      pinMode(pin, INPUT);
      _pin = pin;
      _is_pressed = false;
      _previously_clicked = false;
    }

    //returns true if button was pressed last 'loop' cycle
    bool WasPreviouslyPressed(){
      return _previously_clicked;
    }

    //returns true if button is pressed in this 'loop' cycle
    bool IsButtonPressed(){
      return _is_pressed;
    }

    //sets triggered button to true and store its previous state
    void ScanState(){
      _previously_clicked = _is_pressed;
      _is_pressed = isPressed();
    }

  private:
    bool _previously_clicked;
    int _pin;
    bool _is_pressed;

    //build-in finds out if pin is HIGH
    bool isPressed(){
      return !((bool)digitalRead(_pin)); //inversion logic
    }
};

class Display {
  public:
    Display(){
      pinMode(latch_pin, OUTPUT);
      pinMode(clock_pin, OUTPUT);
      pinMode(data_pin, OUTPUT);
      _position = 0;
    }

    void ShowAnimation(){
      _position++;
      _position %= _display_width;
      writeGlyph(_word_dice[_position]);
    } 

    void ShowGeneratedNumber(int random_number){
      _position++;
      _position %= _display_width;
      if (disableLeadingZeros(random_number) == true) //disable leading zeros problem
        return;
      int extraxt_digit = (int)getGlyphPosition(random_number, _display_width - _position);
      byte glyph = digits[extraxt_digit];
      writeGlyph(glyph);
    }

    void ShowConfiguration(int number_of_throws, int number_of_sides){
      _position++;
      _position %= _display_width;
      
      switch(_position){
        case 0:
          writeGlyph(digits[number_of_throws]);
          break;
        case 1:
          writeGlyph(_letter_d);
          break;
        default:
          if (number_of_sides < 10){
            if (_position == 3)
              return;
            number_of_sides *= 10;
          }
          int extraxt_digit = (int)getGlyphPosition(number_of_sides, _display_width - _position);
          byte glyph = digits[extraxt_digit];
          writeGlyph(glyph);
          break;
      }
    }
    
  private:
    const byte _letter_d = 0b10100001;
    const byte _word_dice[4] = {0b10100001, 0b11111001, 0b11000110, 0b10000110};
    int _position;
    const int _display_width = 4;

    void writeGlyph(byte glyph){
      digitalWrite(latch_pin, LOW);
      shiftOut(data_pin, clock_pin, MSBFIRST, glyph);
      shiftOut(data_pin, clock_pin, MSBFIRST, digit_muxpos[_position]);
      digitalWrite(latch_pin, HIGH);
    }
    
    int getGlyphPosition(int number, int position){
      int digit = 0;

      for (int i = 0; i < position; ++i){
        digit = number % 10;
        number /= 10;
      }
      return digit;
    }

    bool disableLeadingZeros(int random_number){
      if ((_position == 0) && (random_number < 1000))
        return true;
      else if ((_position == 1) && (random_number < 100))
        return true;
      else if ((_position == 2) && (random_number < 10))
        return true;
      return false;
    }
};

class Dice {
  public:
    Dice(){
      _state = NORMAL;
      _dice_sides_point = 1;
      _number_of_throws = 3;
    }

    void ChangeSidesCount(){
      _dice_sides_point++;
      _dice_sides_point %= _dice_types_count;
    }

    void ChangeThrowsCount(){
      _number_of_throws++;
      if (_number_of_throws == _max_throws)
        _number_of_throws = 1;
    }

    int GetThrowsCount(){
      return _number_of_throws;
    }

    int GetSidesCount(){
      return _dice_sides[_dice_sides_point];
    }

    void ChangeState(STATES state){
      _state = state;
    }

    STATES GetState(){
      return _state;
    }

    long GetStartTime(){
      return _start_press;
    }

    void AnullTime(){
      _start_press = millis();
    }

    int GetRandomNumber(){
      return _random_number;
    }

    void GenerateRandomNumber(long pressing_time){
      int random_number = 0;
      randomSeed(pressing_time);

      for (int i = 0; i < _number_of_throws; ++i){
        random_number += random(1, _dice_sides[_dice_sides_point] + 1);
      }
      _random_number = random_number;
    }

  private:
    long _start_press;
    int _dice_sides_point;
    const int _dice_sides[7] = { 4, 6, 8, 10, 12, 20, 100 };
    const int _dice_types_count = sizeof(_dice_sides) / sizeof(_dice_sides[0]);
    int _number_of_throws;
    const int _max_throws = 10;
    int _random_number;
    STATES _state;
};

//globals 
Dice dice;
Display disp;
Button btns[3] = {
  Button(button1_pin), //normal - roll dice
  Button(button2_pin), //config - change throw count
  Button(button3_pin) //config - change dice
  };
constexpr int btns_count = sizeof(btns) / sizeof(btns[1]);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
}

void ButtonsManagement(int btns_count, STATES curr_state){
  for (int button_id = 0; button_id < btns_count; ++button_id){
    btns[button_id].ScanState();
    if (btns[button_id].IsButtonPressed() == true){
      ButtonSwitchCase(button_id, curr_state);
      return;
    }
    if ((btns[0].IsButtonPressed() == false) && (btns[0].WasPreviouslyPressed() == true)){
      int pressing_time = (long)millis() - dice.GetStartTime();
      dice.GenerateRandomNumber(pressing_time);
    }
  }
}

void ButtonSwitchCase(int button_id, STATES curr_state){
  switch (curr_state) {
    case NORMAL:
      if (button_id == 0){
        if (btns[button_id].WasPreviouslyPressed() == false)
          dice.AnullTime();
        else
          disp.ShowAnimation();
      }
      else
        dice.ChangeState(CONFIG);
      break;
    case CONFIG:
      if (button_id == 0)
        dice.ChangeState(NORMAL);
      else
        Increment(button_id);
      break;
  }
}

void Increment(int button_id){
  if ((button_id == 1) && (btns[button_id].WasPreviouslyPressed() == false)){
    dice.ChangeThrowsCount();
  }
  else if ((button_id == 2) && (btns[button_id].WasPreviouslyPressed() == false)){
    dice.ChangeSidesCount();
  }
}

void DisplayManagement(STATES curr_state){
  if (curr_state == NORMAL){
    int random_number = dice.GetRandomNumber();
    disp.ShowGeneratedNumber(random_number);
  }
  else{
    int number_of_throws = dice.GetThrowsCount();
    int number_of_sides = dice.GetSidesCount();
    disp.ShowConfiguration(number_of_throws, number_of_sides);
  }
}

void loop(){
  // put your main code here, to run repeatedly:
  STATES curr_state = dice.GetState();
  ButtonsManagement(btns_count, curr_state);
  if (!((btns[0].IsButtonPressed() == true) && (btns[0].WasPreviouslyPressed() == true)))
    DisplayManagement(curr_state);
}