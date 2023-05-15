import asyncio
from enum import Enum
import json
import logging
from typing import Dict, Union

from typing import Any
from Farm import Farm
from InterFaces.actuators import ACommand
from connection_manager import ConnectionManager
from InterFaces.twins import ITwinSubscriber
from twinsubscribers.control_rule import ControlRule
CONTROL_KEY = "actuatorControl"
METHOD_KEY = "controlMethod"
TIMING_KEY = "ruleUpdateRate"
MANUAL_STATE_KEY = "manualState"
RULES_KEY = "rules"
class ControlManager(ITwinSubscriber):
    """Responsible for managing actuator controls, and device twin communication."""

    #enums
    DEFAULT_RULE_UPDATE = 5
    class ControlMethods(str, Enum):
        """Enum defining the control methods available"""
        MANUAL = "manual"
        RULES = "rules"


    def __init__(self, farm:Farm) -> None:
        self._farm: Farm = farm
        self._control_methods: dict[str, ControlManager.ControlMethods] = {}
        self._rules:dict[str, list[ControlRule]] = {}
        self._manualStates:dict[str,Any] = {}
        self._has_received_desired_values = False
        self._rule_update_rate = ControlManager.DEFAULT_RULE_UPDATE
        self._event_task = asyncio.create_task(self.rule_update_loop())
        
    def handle_desired(self, data: dict):
        if data[CONTROL_KEY] is None:
            print("No actuator control key!")
        else:
            for key in data[CONTROL_KEY]:
                self.parse_actuator(key, data[CONTROL_KEY][key])
        if data[TIMING_KEY] is None:
            print("No timing key!")
        else:
            self._rule_update_rate = data[TIMING_KEY]
        self._has_received_desired_values = True
    async def rule_update_loop(self):
        #may want this to have a cancellarion token
        while True:
            if self._has_received_desired_values:
                readings = self._farm.read_sensors()
                commands:list[ACommand] = []
                for actuator in self._rules:
                    if self._control_methods[actuator] == self.ControlMethods.RULES:
                        for rule in self._rules[actuator]:
                            command = rule.generate_command(readings)
                            if command != None:
                                commands.append(command)
                for command in commands:
                    self._farm.run_command(command)
            await asyncio.sleep(self._rule_update_rate)
    def generate_report(self, data: dict) -> dict:
        
        data[TIMING_KEY] = self._rule_update_rate
        control_data = {}
        actuatorStates = self._farm.get_actuator_states()
        for actuator in actuatorStates:
            actuator_data:dict[str, Any] = {METHOD_KEY: ControlManager.ControlMethods.MANUAL}
            actuator_data[MANUAL_STATE_KEY] = actuatorStates[actuator]
            if actuator in self._control_methods:
                actuator_data[METHOD_KEY] = self._control_methods[actuator]
            
            actuator_data[RULES_KEY] = []
            if actuator_data[METHOD_KEY] == ControlManager.ControlMethods.RULES:
                for rule in self._rules[actuator]:
                    actuator_data[RULES_KEY].append(rule.serialize())
            control_data[actuator] = actuator_data
        data[CONTROL_KEY]=control_data
        return data

    def parse_actuator(self, actuator_name:str, actuator: dict) -> None:
        #CURRENTLY, commands should never be more than one per actuator, but the logic is impleneted for if there ever are cases we want more
        commands:list[ACommand] = []
        
        if actuator[METHOD_KEY] not in list(ControlManager.ControlMethods):
            print("invalid control method!")
            raise ValueError("invalid control method!!")
        control_method:str = actuator[METHOD_KEY]
        self._control_methods[actuator_name] = ControlManager.ControlMethods(control_method)
        if control_method == ControlManager.ControlMethods.MANUAL:
            #manual control
            #right now all possible values are just one input, {"value":value}, so we will just pass that. In the near future we may want more versatility here
            value = actuator[MANUAL_STATE_KEY]
            jsonval = json.dumps({"value":value})
            commands.append(ACommand(actuator_name, jsonval))
        if control_method == ControlManager.ControlMethods.RULES:
            #rule based control.
            self._rules[actuator_name] = []
            if actuator["rules"] is None:
                raise ValueError("No rules!")
            for rule_dict in actuator["rules"]:
                try:
                    rule = ControlRule(rule_dict, actuator_name)
                    self._rules[actuator_name].append(rule)
                except:
                    logging.exception('Error making rule!')
        #rule commands are ran in our task loop!!
        for command in commands:
            self._farm.run_command(command)

                