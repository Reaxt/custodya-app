import asyncio
import os
import time
from typing import Any
from InterFaces.actuators import ACommand, IActuator
from configuration_manager import Configuration
from subsystems.security.SecuritySubSystem import SecuritySubSystem
from subsystems.plants.plantsSubSystem import PlantsSubSystem
from subsystems.GeoLocation.GeoLocationSubSystem import GeoLocation 
from InterFaces.sensors import AReading, ISensor
from InterFaces.subsystem import ASubsystem
class Farm:
    def __init__(self, configuration:Configuration) -> None:
        self._subsystems: list[ASubsystem] = list()
        if configuration.geolocation_enabled:
            self._subsystems.append(GeoLocation())
        if configuration.security_enabled:
            self._subsystems.append(SecuritySubSystem())
        if configuration.plants_enabled:
            self._subsystems.append(PlantsSubSystem())
    def read_sensors(self) -> list[AReading]:
        readings = list()
        for subsystem in self._subsystems:
            readings.extend(subsystem.read_sensors())
        return readings
    
    def serialize_state(self) -> dict[str, Any]:
        state: dict[str, Any] = dict()
        state["timestamp"] = int(time.time())
        for subsystem in self._subsystems:
            state[subsystem.get_name()] = subsystem.serialize_state()
        return state
    def get_actuator_states(self) -> dict[str, Any]:
        state: dict[str, Any] = dict()
        for subsystem in self._subsystems:
            sys_actuators = subsystem.get_actuator_states()
            for item in sys_actuators:
                state[item] = sys_actuators[item]
        return state
    def run_command(self, command:ACommand) -> bool:
        res:bool = False
        for subsystem in self._subsystems:
            temp = subsystem.process_command([command])
            res = True if temp else res
        return res