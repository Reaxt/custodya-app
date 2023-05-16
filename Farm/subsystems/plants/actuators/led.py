"""This module controls the led state"""
import sys
from typing import Any

sys.path.append("..")

from grove.grove_ws2813_rgb_led_strip import GroveWS2813RgbStrip
from time import sleep
from rpi_ws281x import Color
import json
from InterFaces.actuators import IActuator, ACommand


OUTPIN = 16
COUNT = 10
OPEN_COMMAND = True
CLOSE_COMMAND = False
TARGET = "Led"


class LedController(IActuator):
    """This class controls the led state"""

    def __init__(
        self, gpio: int = OUTPIN, initial_state: dict = {"value": CLOSE_COMMAND}
    ):
        self.led = GroveWS2813RgbStrip(gpio, COUNT)
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
            for i in range(self.led.numPixels()):
                self.led.setPixelColor(i, Color(255, 255, 255))
                self.led.show()
                sleep(50/1000.0)
        elif data["value"] == CLOSE_COMMAND:
            for i in range(self.led.numPixels()):
                self.led.setPixelColor(i, Color(0, 0, 0))
                self.led.show()
                sleep(50/1000.0)
        self._current_state = data
        return True


    def get_current_state(self) -> Any:
        return True if self._current_state["value"] == OPEN_COMMAND else False
    def get_actuator_name(self) -> str:
        return TARGET

if __name__ == "__main__":
    ledtest = LedController()
    while True:
        ledtest.control_actuator({"value": OPEN_COMMAND})
        sleep(2)
        ledtest.control_actuator({"value": CLOSE_COMMAND})
        sleep(2)
