"""This module controls the servo motor state
"""
from gpiozero import Servo
from time import sleep
import json
from actuators import IActuator, ACommand


OUTPIN = 16
OPEN_COMMAND = "on"
CLOSE_COMMAND = "off"
TARGET = "door"
class ServoController(IActuator):
    """This class controls the servo motor that acts for the door"""
    def __init__(self,
                 gpio:int=OUTPIN,
                 initial_state:dict = {"value":CLOSE_COMMAND}):
        self.servo = Servo(gpio)
        self._current_state = {"value":"NEITHER"}
        starting_command = ACommand(TARGET, json.dumps(initial_state))

    def validate_command(self, command: ACommand) -> bool:
        if(command.target_type != TARGET):
            return False
        if not command.data["value"] in (OPEN_COMMAND, CLOSE_COMMAND):
            return False
        return True
    def control_actuator(self, data: dict) -> bool:
        if data["value"] == self._current_state["value"]:
            return False
        if data["value"] == OPEN_COMMAND:
            self.servo.max()
        elif data["value"] == CLOSE_COMMAND:
            self.servo.min()
        return True
    
if __name__ == "__main__":
    servotest = ServoController()
    while True:

        servotest.control_actuator({"value":OPEN_COMMAND})
        sleep(2)
        servotest.control_actuator({"value":CLOSE_COMMAND})
        sleep(2)