"""This module controls the led state"""
from grove.grove_ws2813_rgb_led_strip import GroveWS2813RgbStrip
from time import sleep
from rpi_ws281x import Color
import json
from ..base.actuators import IActuator, ACommand


OUTPIN = 12
COUNT = 10
OPEN_COMMAND = "on"
CLOSE_COMMAND = "off"
TARGET = "ws2813"


class LedController(IActuator):
    """This class controls the led state"""

    def __init__(
        self,
        gpio: int = OUTPIN,
        initial_state: dict = {"value": CLOSE_COMMAND}
    ):
        self.led = GroveWS2813RgbStrip(gpio, COUNT)
        self._current_state = {"value": "NEITHER"}
        starting_command = ACommand(TARGET, json.dumps(initial_state))

    def validate_command(self, command: ACommand) -> bool:
        if (command.target_type != TARGET):
            return False
        if not command.data["value"] in (OPEN_COMMAND, CLOSE_COMMAND):
            return False
        return True

    def control_actuator(self, data: dict) -> bool:
        if data["value"] == self._current_state["value"]:
            return False
        if data["value"] == OPEN_COMMAND:
            self.led.on()
        elif data["value"] == CLOSE_COMMAND:
            self.led.off()
        return True

    def color_wipe(led: GroveWS2813RgbStrip, color: Color, wait_ms=50):
        """Wipe color across display a pixel at a time."""
        for i in range(led.numPixels()):
            led.setPixelColor(i, color)
            led.show()
            sleep(wait_ms/1000.0)


if __name__ == "__main__":
    ledtest = LedController()
    while True:
        ledtest.control_actuator({"value": OPEN_COMMAND})
        sleep(2)
        ledtest.control_actuator({"value": CLOSE_COMMAND})
        sleep(2)
