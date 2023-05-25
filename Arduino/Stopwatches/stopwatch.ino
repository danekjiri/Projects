#include "funshield.h"

//ENUM
enum _sw_states { //possible states of stopwatches - button/display control
        STOPPED,
        RUNNING,
        LAPPED
      };

//classes
class Display {
  public:
    void set_value(long time){ //setting display value outside of class
      _curr_time = time;
    }

    void show_value(){ //showing every digit of time - multiplexing
      _position++;
      _position %= 4;
      if (show_leading_zeros() == false) //disable leading zeros problem
        return;
      _glyph = get_glyph(_curr_time);
      write_glyph();
    }

  private:
    int _glyph;
    long _curr_time;
    int _position;

    int get_glyph(long time){ //getting one digit of _curr_time => given _position
      time /= 100; //convert to format 1100ms -> 1.1s
      int digit = 0;
      int position = 4 - _position;

      for (int i = 0; i < position; ++i){
        digit = time % 10;
        time /= 10;
      }

      return digit;
    }

    void write_glyph(){
      constexpr int dot = 128;
      digitalWrite(latch_pin, LOW);
      shiftOut(data_pin, clock_pin, MSBFIRST, digits[_glyph]);
      if (_position == 2) //overwrites with decimal separator if _position==2
        shiftOut(data_pin, clock_pin, MSBFIRST, digits[_glyph] + dot);
      shiftOut(data_pin, clock_pin, MSBFIRST, digit_muxpos[_position]);
      digitalWrite(latch_pin, HIGH);
    }

    bool show_leading_zeros(){
      if ((_curr_time < 10000) && (_position == 1)) //represented in ms -> 10s
          return false;
      if ((_curr_time < 100000) && (_position == 0)) //100s
        return false;

      return true;
    }
};

class Button {
  private:
    long _last_pressed;
    int _press_time;
    int _pin;

    bool is_pressed(){ //inversion logic of hw - negation
      return !((bool)digitalRead(_pin));
    }

  public:
    Button(int pin, int press_time){ //init pin and necessary ms time to hold - debouncing
      _pin = pin;
      _press_time = press_time;
    }
    bool flag_click;
    

     void scan_state(){ //debouncing management
      if ((bool)is_pressed() == true){   
        if ((long)millis() - _last_pressed > _press_time) //optional debouncing
          flag_click = true;
        _last_pressed = millis();
        }
    }
};

class StopWatch {
  public:
    long display_time; //readable displaying time in eg.: 1.1s
    enum _sw_states _sw_state = STOPPED; //state of stopwatch according button pressing
    
    void reset_time(){
      display_time = 0;
      _sw_state = STOPPED;
    }
    void start_time(){
      _start_time = millis() - display_time;
      _sw_state = RUNNING;
    }
    void stop_time(){
      _sw_state = STOPPED;
    }
    void lap_time(){
      _sw_state = LAPPED;
    }
    void resume_time(){
      _sw_state = RUNNING;
    } 

    int update_time(){ //time difference if running
      _time_since_poweron = millis();
      display_time = _time_since_poweron - _start_time;

      return display_time;
    }
    
    private:
    long _time_since_poweron;
    long _start_time;
};

//Globals
constexpr int debouncing_time = 50;
constexpr int btns_arr[3] = {button1_pin, button2_pin, button3_pin};
StopWatch stopw;
Display disp;
Button btns[3] = {
  Button(button1_pin, debouncing_time),
  Button(button2_pin, debouncing_time), 
  Button(button3_pin, debouncing_time)
  };
constexpr int btns_count = sizeof(btns) / sizeof(btns[1]);

void setup() {
  // put your setup code here, to run once:
      pinMode(latch_pin, OUTPUT); //init 7-segment
      pinMode(clock_pin, OUTPUT);
      pinMode(data_pin, OUTPUT);
      
      for (int i = 0; i < btns_count; i++){
        pinMode(btns_arr[i], INPUT);
      }
}

void btn_switch_case(int btn_number){ //main stopwatch-buttons logic
  switch (btn_number) {
    case 0:
      btns[0].flag_click = false;
      if (stopw._sw_state == STOPPED)
        stopw.start_time();
      else if (stopw._sw_state == RUNNING)
        stopw.stop_time();
      break;
    case 1:
      btns[1].flag_click = false;
      if (stopw._sw_state == RUNNING)
        stopw.lap_time();
      else if (stopw._sw_state == LAPPED)
        stopw.resume_time();
      break;
    case 2:
      btns[2].flag_click = false;
      if (stopw._sw_state == STOPPED){
        stopw.reset_time();
        int time = stopw.display_time;
        disp.set_value(time);
        disp.show_value();
      }
      break;
  }
}

void btn_management(int count){ //findout clicked button => do its methods
  for (int i = 0; i < count; ++i){
    btns[i].scan_state();
    if (btns[i].flag_click == true)
      btn_switch_case(i);
  }
}

void update_if_running(){ //update the time on display if running
  long time;
  if (stopw._sw_state == RUNNING){
    time = stopw.update_time();
    disp.set_value(time);
  }
}

void loop() {
  // put your main code here, to run repeatedly:
  btn_management(btns_count);
  
  update_if_running();
  disp.show_value();
}
