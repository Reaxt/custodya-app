import time
import seeed_python_reterminal.core as rt
class Buzzer():
    def __init__(self):
        self.buzzer = rt.buzzer
    
    def check_buzzer_status(self):
        return self.buzzer
    
    def turn_buzzer_on(self):
        rt.buzzer = True
        # code to turn buzzer on
    
    def turn_buzzer_off(self):
        rt.buzzer = False
        # code to turn buzzer off

def main():
    buzzer = Buzzer()
    #rt.buzzer = False
    # check buzzer status
    print("Initial buzzer status:", buzzer.check_buzzer_status())
    start_time = time.time()
    print("Start Time: " , start_time)
    while time.time() - start_time < 10:
        #Check the buzzer status
        print("Current buzzer status:",buzzer.check_buzzer_status())
        if int(time.time() - start_time) % 2 == 0:
            buzzer.turn_buzzer_off()
        else:
            buzzer.turn_buzzer_on()
        time.sleep(1)

    # check buzzer status again
    buzzer.turn_buzzer_off()
    print("Final buzzer status:", buzzer.check_buzzer_status())

if __name__ == '__main__':
    main()
