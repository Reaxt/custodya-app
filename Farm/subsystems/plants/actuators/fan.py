"""This module controls the fan state"""
import sys
from typing import Any

sys.path.append("..")
from gpiozero import DigitalOutputDevice
from time import sleep
import json
from InterFaces.actuators import IActuator, ACommand


OUTPIN = 18
OPEN_COMMAND = "on"
CLOSE_COMMAND = "off"
TARGET = "fan"


class FanController(IActuator):
    """This class controls the fan state"""

    def __init__(
        self, gpio: int = OUTPIN, initial_state: dict = {"value": CLOSE_COMMAND}
    ):
        self.fan = DigitalOutputDevice(gpio)
        self._current_state = {"value": "NEITHER"}
        starting_command = ACommand(TARGET, json.dumps(initial_state))

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
            self.fan.on()
        elif data["value"] == CLOSE_COMMAND:
            self.fan.off()
        self._current_state = data
        return True
    def get_current_state(self) -> Any:
        return True if self._current_state == OPEN_COMMAND else False
    def get_actuator_name(self) -> str:
        return "Fan"
if __name__ == "__main__":
    fantest = FanController()
    while True:
        fantest.control_actuator({"value": OPEN_COMMAND})
        sleep(2)
        fantest.control_actuator({"value": CLOSE_COMMAND})
        sleep(2)
