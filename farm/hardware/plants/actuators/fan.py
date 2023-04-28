"""This module controls the fan state"""
import sys

sys.path.append("..")
from gpiozero import DigitalOutputDevice
from time import sleep
import json
from base.actuators import IActuator, ACommand


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
        return True


if __name__ == "__main__":
    fantest = FanController()
    while True:
        fantest.control_actuator({"value": OPEN_COMMAND})
        sleep(2)
        fantest.control_actuator({"value": CLOSE_COMMAND})
        sleep(2)