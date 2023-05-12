from abc import ABC, abstractmethod

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
    def process_command(self, commands:list[ACommand]) -> None:
        """Validate and process a command that may be for this subsystem.
        """
        for command in commands:
            for actuator in self._actuators:
                actuator.try_command(command)
    def serialize_state(self) -> dict:
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