from abc import ABC, abstractmethod

from typing import Any

from InterFaces.sensors import AReading, ISensor
from InterFaces.actuators import ACommand, IActuator

class ASubsystem(ABC):
    """Abstract class for subsystems.
        IMPLEMENTATIONS MUST PLACE SENSORS IN self._sensors AND ACTUATORS IN self._actuators
    """
    def __init__(self):
        self._actuators:list[IActuator] = []
        self._sensors:list[ISensor] = []
    @abstractmethod
    def get_name(self) -> str:
        """Get the name of this subsystem

        Returns:
            str: Returns the name of this subsystem
        """
        pass
    def read_sensors(self) -> list[AReading]:
        """Get all the sensor readings

        Returns:
            list[AReading]: a list of all readings from sensors.
        """
        readings: list[AReading] = []
        for sensor in self._sensors:
            for reading in sensor.read_sensor():
                readings.append(reading)
        return readings
    def process_command(self, commands:list[ACommand]) -> bool:
        """Validate and process a command that may be for this subsystem.

        Args:
            commands (list[ACommand]): list of commands to attempt and process

        Returns:
            bool: False if zero commands where valid. otherwise true.
        """
        res:bool = False
        for command in commands:
            for actuator in self._actuators:
                temp = actuator.try_command(command)
                res = True if temp else res
        return res
    def serialize_state(self) -> dict[str, Any]:
        """Serialize this subsystem for telemetry

        Returns:
            dict: an object representing the state of this subsystem.
        """
        serialized = dict()
        for reading in self.read_sensors():
            serialized[reading.reading_type] = reading.value
        for actuator in self._actuators:
            serialized[actuator.get_actuator_name()] = actuator.get_current_state()
        return serialized
    def get_actuator_states(self) -> dict[str, Any]:
        """Get the state of all actuators

        Returns:
            dict: state of all the actuators in this subsystem
        """
        state = dict()
        for actuator in self._actuators:
            state[actuator.get_actuator_name()] = actuator.get_current_state()
        return state