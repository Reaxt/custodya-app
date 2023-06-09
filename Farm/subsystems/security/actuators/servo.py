"""This module controls the servo motor state
"""
from typing import Any
from gpiozero import Servo
from time import sleep
import json
from InterFaces.actuators import IActuator, ACommand

OUTPIN = 16
OPEN_COMMAND = True
CLOSE_COMMAND = False
class DoorController(IActuator):
    """This class controls the servo motor that acts for the door"""
    TARGET = "DoorLock"
    def __init__(self,
                 gpio:int=OUTPIN,
                 initial_state:dict = {"value":CLOSE_COMMAND}):
        self.servo = Servo(gpio)
        self._current_state:dict = {"value":"NEITHER"}
        starting_command = ACommand(DoorController.TARGET, json.dumps(initial_state))
        self.control_actuator(starting_command.data)

    def validate_command(self, command: ACommand) -> bool:
        if(command.target_type != DoorController.TARGET):
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
        self._current_state = data
        return True
    def get_current_state(self) -> Any:
        return True if self._current_state["value"] == OPEN_COMMAND else False
    def get_actuator_name(self) -> str:
        return DoorController.TARGET

if __name__ == "__main__":
    servotest = DoorController()
    while True:

        servotest.control_actuator({"value":OPEN_COMMAND})
        sleep(2)
        servotest.control_actuator({"value":CLOSE_COMMAND})
        sleep(2)