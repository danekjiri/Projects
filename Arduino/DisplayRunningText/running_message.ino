#include "funshield.h"
#include "input.h"

class Display {
  public:
    int curr_msg_position;
    int curr_msg_lenght;
    //setting new message into data structure format,
    //init curr_msg_position, init pointing array
    void setMessage(const char *msg){ 
      int postfix = 4; //blanket spaces after word - required animation
      curr_msg_lenght = getStringLenght(msg) + postfix;
      
      for (int i = 0; i < curr_msg_lenght; ++i){ //copy word into display structure + add spaces
        if (i < curr_msg_lenght - postfix){
          _message[i] = msg[i];
        }
        else{
          _message[i] = ' ';
        }
      }

      curr_msg_position = 0; //init latest letter's index counter
      for (int i = 0; i < 4; ++i){ //init start state: first letter the most left segment, last letter the second most left one, last but not least letter the 3 most left one etc...
                                  //and then in shifting incrementing modulo message lenght -> left to right message animation 
        _disp_pos[3-i] = (curr_msg_lenght + (-i % curr_msg_lenght)) % curr_msg_lenght;
      }
    }
    //basic multiplexing
    void showMessage(){ 
      _position++;
      _position %= 4;
      
      displayChar(_message[_disp_pos[_position]], _position);
    }
    //increment each position and modulo message lenght so the message rotates
    void ShiftMsg(){
      int positionsCount = 4;
      
      for (int i = 0; i < positionsCount; ++i){ 
        _disp_pos[i]++;
        _disp_pos[i] %= curr_msg_lenght;
      }
      curr_msg_position++; //increment to be aware of message ending
    }

  private:
    char _message [100]; //limited space for message 97 characters + 4 ending spaces
    int _position;
    int _disp_pos [4];
    const byte EMPTY_GLYPH = 0b11111111;
    const byte LETTER_GLYPH [26] {
      0b10001000,   // A
      0b10000011,   // b
      0b11000110,   // C
      0b10100001,   // d
      0b10000110,   // E
      0b10001110,   // F
      0b10000010,   // G
      0b10001001,   // H
      0b11111001,   // I
      0b11100001,   // J
      0b10000101,   // K
      0b11000111,   // L
      0b11001000,   // M
      0b10101011,   // n
      0b10100011,   // o
      0b10001100,   // P
      0b10011000,   // q
      0b10101111,   // r
      0b10010010,   // S
      0b10000111,   // t
      0b11000001,   // U
      0b11100011,   // v
      0b10000001,   // W
      0b10110110,   // ksi
      0b10010001,   // Y
      0b10100100,   // Z
    };
    //computes new message lenght in characters
    size_t getStringLenght(const char *str) { 
      size_t counter = 0;
      while (str[counter] != '\0'){
      ++counter;
      }

      return counter;
    }
    //displaying characters onto display 7-segment
    void displayChar(char ch, byte pos){ //ch - showing character, pos - which display positiom 0=left
      byte glyph = EMPTY_GLYPH;
      if (isAlpha(ch)) {
        glyph = LETTER_GLYPH[ ch - (isUpperCase(ch) ? 'A' : 'a') ];
      }
      
      digitalWrite(latch_pin, LOW);
      shiftOut(data_pin, clock_pin, MSBFIRST, glyph);
      shiftOut(data_pin, clock_pin, MSBFIRST, 1 << pos);
      digitalWrite(latch_pin, HIGH);
    }
};

class Timer {
  public:
  //setting shifting interval time
    Timer(int interval){ 
      _interval = interval;
    }
    
    //if time greater than or equal to interval - returns true
    bool timeToShift(){ 
      unsigned long current_time = millis();
      if (current_time - _last_pressed >= _interval){
        _last_pressed = millis();
        return true;
      }
      return false;
    }

  private:
    unsigned long _last_pressed = 0;
    unsigned int _interval;
};

SerialInputHandler input;
Display disp;
Timer ti(300);
void setup() {
  pinMode(latch_pin, OUTPUT);
  pinMode(clock_pin, OUTPUT);
  pinMode(data_pin, OUTPUT);

  input.initialize();
}
//handles the time to shift message in display
void shiftManagement(){
  bool should_shift = ti.timeToShift();
  
  if (should_shift == true){
    disp.ShiftMsg();
  }
}
//handles message ends - repeat or new
void newMessageManagement(){
  int current_letter = disp.curr_msg_position;
  int last_letter = disp.curr_msg_lenght;

  if (current_letter == last_letter){
    disp.setMessage(input.getMessage());
  }
}

void loop() {
  input.updateInLoop();
  
  newMessageManagement();
  shiftManagement();
  
  disp.showMessage();
}