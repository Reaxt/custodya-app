import time
from typing import Any
import seeed_python_reterminal.core as rt
import sys

sys.path.append("..")
from InterFaces.actuators import IActuator, ACommand

OPEN_COMMAND = "True"
CLOSE_COMMAND = "False"
TARGET = "buzzer"
class Buzzer(IActuator):
    def __init__(self):
        self.buzzer = rt.buzzer
        self._current_state = {"value": "FALSE"}
    
    def validate_command(self, command: ACommand) -> bool:
        if command.target_type != TARGET:
            return False
        if not command.data["value"] in (OPEN_COMMAND, CLOSE_COMMAND):
            return False
        return True
    
    def control_actuator(self, data: dict) -> bool:
        if data["value"] == self._current_state["value"]:
            return False
        if data["value"] == OPEN_COMMAND:
            rt.buzzer = True
        elif data["value"] == CLOSE_COMMAND:
            rt.buzzer = False
        self._current_state = data
        return True
    def get_current_state(self) -> Any:
        return True if self._current_state["value"] == OPEN_COMMAND else False
    def get_actuator_name(self) -> str:
        return "Buzzer"

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
