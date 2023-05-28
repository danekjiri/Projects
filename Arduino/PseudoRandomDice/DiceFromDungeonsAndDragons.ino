#include "funshield.h"

//states of dice (generating/modifying)
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

    //return true if button was pressed last 'loop' cycle
    bool WasPreviouslyPressed(){
      return _previously_clicked;
    }

    //return true if button is pressed in this 'loop' cycle
    bool IsButtonPressed(){
      return _is_pressed;
    }

    //set triggered button to true and store its previous state
    void ScanState(){
      _previously_clicked = _is_pressed;
      _is_pressed = isPressed();
    }

  private:
    bool _previously_clicked;
    unsigned short _pin;
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

    //show text when gathering random seed
    void ShowAnimation(){
      incrementPositionAndModulo();

      writeGlyph(_word_dice[_position]);
    } 

    //every 'loop' cycle show one digit of generated number
    void ShowGeneratedNumber(unsigned int random_number){
      incrementPositionAndModulo();

      if (disableLeadingZeros(random_number) == true) //disable leading zeros
        return;
      
      unsigned short extraxt_digit = (unsigned short)getGlyphPosition(random_number, _display_width - _position); //the smallest digit is on the right, the display is enumerate from left though
      byte glyph = digits[extraxt_digit]; //get byte number representation from funshield.h array
      writeGlyph(glyph);
    }

    //every 'loop' cycle show one info of current dice configuration
    void ShowConfiguration(unsigned short number_of_throws, unsigned short number_of_sides){
      incrementPositionAndModulo();
      
      switch(_position){
        case 0: //most left module
          writeGlyph(digits[number_of_throws]);
          break;
        case 1:
          writeGlyph(_letter_d);
          break;
        default: //two modules from right - number of its sides
          if (number_of_sides < 10){ //representing sigle digit numbers shifted module left, eg. 4-side dice as 40 but the zero not shown -> look like just 4
            if (_position == 3)
              return;
            number_of_sides *= 10;
          }
          unsigned int extraxt_digit = (unsigned short)getGlyphPosition(number_of_sides, _display_width - _position);
          byte glyph = digits[extraxt_digit];
          writeGlyph(glyph);
          break;
      }
    }
    
  private:
    unsigned short _position;
    const byte _letter_d = 0b10100001; //'d'
    const byte _word_dice[4] = {0b10100001, 0b11111001, 0b11000110, 0b10000110}; //{ d, i, c, e }
    const unsigned short _display_width = 4; //number of modules

    void incrementPositionAndModulo(){
      _position++;
      _position %= _display_width;
    }

    //writes given byte of data to given module - position {0, 1, 2, 3}
    void writeGlyph(byte glyph){
      digitalWrite(latch_pin, LOW);
      shiftOut(data_pin, clock_pin, MSBFIRST, glyph);
      shiftOut(data_pin, clock_pin, MSBFIRST, digit_muxpos[_position]);
      digitalWrite(latch_pin, HIGH);
    }
    
    //extract cypher on given position and return it
    int getGlyphPosition(unsigned short number, unsigned short position){
      unsigned short digit = 0;

      for (unsigned short i = 0; i < position; ++i){
        digit = number % 10;
        number /= 10;
      }
      return digit;
    }

    //
    bool disableLeadingZeros(unsigned int random_number){
      unsigned int number_of_zeros = _display_width - _position - 1; //prepare decimal exponent
      if (random_number < pow(10, number_of_zeros)) //current position and number cypher condition, eg. if ((_position == 1) && (random_number < 100)){...}
        return true;
      return false;
    }
};

class Dice {
  public:
    Dice(){
      _state = NORMAL; //starting state -> generating
      _dice_sides_point = 1; //starting dice -> first in const array _dice_sides
      _number_of_throws = 3; //starting dice throws
    }

    void IncrementDiceSides(){
      _dice_sides_point++;
      _dice_sides_point %= _dice_types_count;
    }

    void IncrementThrows(){
      _number_of_throws++;
      if (_number_of_throws == _max_throws)
        _number_of_throws = 1;
    }

    unsigned short GetThrowsCount(){
      return _number_of_throws;
    }

    unsigned short GetSidesCount(){
      return _dice_sides[_dice_sides_point];
    }

    void ChangeState(STATES state){
      _state = state;
    }

    STATES GetState(){
      return _state;
    }

    unsigned long GetStartTime(){
      return _start_press;
    }

    void AnullTime(){
      _start_press = millis();
    }

    unsigned int GetRandomNumber(){
      return _random_number;
    }

    //generate pseudo-random sum of pseudo-random numbers in given interval of dice sides
    void GenerateRandomNumber(unsigned long pressing_time){
      unsigned int random_number = 0;
      randomSeed(pressing_time);

      for (unsigned short i = 0; i < _number_of_throws; ++i){ //sum of 1-9 random generated numbers in interval [4-100]
        random_number += random(1, _dice_sides[_dice_sides_point] + 1); //to be inclusive
      }
      _random_number = random_number;
    }

  private:
    //dice configuration
    unsigned long _start_press;
    unsigned short _dice_sides_point;
    const unsigned short _dice_sides[7] = { 4, 6, 8, 10, 12, 20, 100 };
    const unsigned short _dice_types_count = sizeof(_dice_sides) / sizeof(_dice_sides[0]);
    unsigned short _number_of_throws;
    const unsigned short _max_throws = 10;
    unsigned int _random_number;
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
constexpr unsigned short btns_count = sizeof(btns) / sizeof(btns[1]);
unsigned short generating_button = 0;
unsigned short increment_throws_button = 1;
unsigned short increment_sides_button = 2;

void setup() {
  // put your setup code here, to run once:
}

//find out if button pressed and do given action
void ButtonsManagement(unsigned int btns_count, STATES curr_state){
  for (unsigned int button_id = 0; button_id < btns_count; ++button_id){
    btns[button_id].ScanState();
    if (btns[button_id].IsButtonPressed() == true){ //if pressed continue to action
      ButtonSwitchCase(button_id, curr_state);
      return; //priority action -> the normal/generating always executed
    }
    if ((btns[generating_button].IsButtonPressed() == false) && (btns[generating_button].WasPreviouslyPressed() == true)){ //after triggering generating button => current state off && previous on
      unsigned long pressing_time = (unsigned long)millis() - dice.GetStartTime(); //time difference between first press and let go
      dice.GenerateRandomNumber(pressing_time); //generate random number with given seed
    }
  }
}

//the main logical states of dice and its actions
void ButtonSwitchCase(unsigned short button_id, STATES curr_state){
  switch (curr_state) {
    case NORMAL:
      if (button_id == generating_button){
        if (btns[generating_button].WasPreviouslyPressed() == false) //first trigger of generating button
          dice.AnullTime();
        else
          disp.ShowAnimation(); //show the text 'd i c e' while holding
      }
      else
        dice.ChangeState(CONFIG); //go to config mode - button 2/3 triggered
      break;

    case CONFIG:
      if (button_id == generating_button)
        dice.ChangeState(NORMAL); //go to config mode - button 1 triggered
      else
        Increment(button_id); //do button 2/3 incremental action
      break;
  }
}

//increment throws or sides of cube
void Increment(unsigned short button_id){
  if ((button_id == increment_throws_button) && (btns[button_id].WasPreviouslyPressed() == false)) //to prevent debouncing - trigger just once a press
    dice.IncrementThrows();
  else if ((button_id == increment_sides_button) && (btns[button_id].WasPreviouslyPressed() == false))
    dice.IncrementDiceSides();
}

//decide which mode and behave so
void DisplayManagement(STATES curr_state){
  if (curr_state == NORMAL){
    unsigned int random_number = dice.GetRandomNumber(); //get stored generated number
    disp.ShowGeneratedNumber(random_number); //show number on display
  }
  else{
    unsigned short number_of_throws = dice.GetThrowsCount();
    unsigned short number_of_sides = dice.GetSidesCount();
    disp.ShowConfiguration(number_of_throws, number_of_sides); //show current configuration
  }
}

void loop(){
  // put your main code here, to run repeatedly:
  STATES curr_state = dice.GetState(); //findout current state of dice
  
  ButtonsManagement(btns_count, curr_state);
  if (!((btns[generating_button].IsButtonPressed() == true) && (btns[generating_button].WasPreviouslyPressed() == true))) //to prevent showing previous generated number
    DisplayManagement(curr_state);
}