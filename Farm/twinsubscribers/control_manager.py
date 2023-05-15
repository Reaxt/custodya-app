from enum import Enum
import json
from typing import Union
from Farm import Farm
from InterFaces.actuators import ACommand
from connection_manager import ConnectionManager
from InterFaces.twins import ITwinSubscriber
CONTROL_KEY = "actuatorControl"
class ControlManager(ITwinSubscriber):
    """Responsible for managing actuator controls, and device twin communication."""

    #enums
    class ControlMethods(str, Enum):
        """Enum defining the control methods available"""
        MANUAL = "manual"
        RULES = "rules"
    class Comparisons(str, Enum):
        """Enum defining the comparison methods available"""
        EQUAL = "=="
        GREATER = ">"
        LESS = "<"

    def __init__(self, farm:Farm) -> None:
        self._farm = farm

    def handle_desired(self, data: dict):
        if data[CONTROL_KEY] is None:
            print("No actuator control key!")
        else:
            for key in data[CONTROL_KEY]:
                self.parse_actuator(key, data[CONTROL_KEY][key])
    def generate_report(self, data: dict) -> dict:
        raise NotImplementedError("Not implemented")

    def parse_actuator(self, actuator_name:str, actuator: dict) -> None:
        command:Union[ACommand, None] = None
        if actuator[CONTROL_KEY] not in list(ControlManager.ControlMethods):
            raise ValueError("invalid control method!!")
        control_method = actuator[CONTROL_KEY]
        
        if control_method == ControlManager.ControlMethods.MANUAL:
            #manual control
            #right now all possible values are just one input, {"value":value}, so we will just pass that. In the near future we may want more versatility here
            value = actuator["manualState"]
            jsonval = json.dumps({"value":value})
            print(jsonval)
            command = ACommand(actuator_name, jsonval )
        if control_method == ControlManager.ControlMethods.RULES:
            #TODO:
            raise NotImplementedError("Not yet implemented")
        
        if command == None:
            print(f"No command found for actuator {actuator_name}, this should not happen!")
        else:
            if not self._farm.run_command(command):
                print(f"Command failed! Type:{actuator_name}, Values:{command.data}")